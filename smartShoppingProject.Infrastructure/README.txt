================================================================================
  smartShoppingProject.Infrastructure — Katman Rehberi
================================================================================

Bu katman, Application'ın tanımladığı arayüzlerin (repository, event bus,
logging, persistence) somut implementasyonlarını içerir. EF Core, PostgreSQL,
Serilog, MassTransit/RabbitMQ gibi teknik detaylar burada yaşar. Application
ve Domain bu projeye bağımlı değildir; tam tersi geçerlidir. Böylece veritabanı
veya mesajlaşma altyapısı değiştiğinde sadece Infrastructure etkilenir.


--------------------------------------------------------------------------------
  Klasör Yapısı
--------------------------------------------------------------------------------

BackgroundServices  → OutboxMessageProcessor (outbox kayıtlarını event bus'a gönderir)
Logging             → Serilog (ApplicationLog), BusinessLogger (BusinessLogs)
Messaging           → InMemoryEventBus, RabbitMqEventBus, DomainEventEnvelope, Consumer
Persistence         → AppDbContext, EF Configurations, Repositories, UnitOfWork
DependencyInjection.cs → DbContext, Repository'ler, EventBus, Serilog, Outbox kayıtları


--------------------------------------------------------------------------------
  Persistence (EF Core)
--------------------------------------------------------------------------------

AppDbContext, Order, OrderItem, Product, Category, OutboxMessage, ApplicationLog
ve BusinessLogs için DbSet tanımlar. OnModelCreating, Configurations klasöründeki
IEntityTypeConfiguration sınıflarını uygular.

SaveChangesAsync override'ında önce WriteDomainEventsToOutboxAsync çalışır:
ChangeTracker'daki BaseEntity kayıtlarının DomainEvents listesi okunur, her
event için bir OutboxMessage oluşturulup aynı transaction içinde kaydedilir.
Sonra entity'lerin event listesi temizlenir ve base.SaveChangesAsync ile
tüm değişiklikler commit edilir. Böylece "transactional outbox" pattern'i
uygulanmış olur; event'ler kayıtla birlikte atomik yazılır.

Repository'ler (OrderRepository, ProductRepository, CategoryRepository) generic
Repository<T> taban sınıfını kullanır. Query'lerde AsNoTracking ve projection
tercih edilir. UnitOfWork, transaction başlatma/commit/rollback ve SaveChanges
sorumluluğunu taşır; Application'daki TransactionBehavior ile birlikte kullanılır.


--------------------------------------------------------------------------------
  Loglama
--------------------------------------------------------------------------------

• Serilog (ApplicationLogSink, SerilogExtensions)
  Console ve veritabanı (ApplicationLogs tablosu) sink'leri. Teknik loglar
  (exception, middleware, EF, pipeline) buraya gider. Serilog konfigürasyonuna
  dokunulmadan CorrelationId, middleware tarafından LogContext ile eklenir.

• BusinessLogger
  IBusinessLogger implementasyonu. Event'leri Serilog veya ILogger kullanmadan
  doğrudan BusinessLogs tablosuna yazar. CorrelationId parametre olarak
  verilmezse ICorrelationIdAccessor (API'de implemente edilir) ile doldurulur.


--------------------------------------------------------------------------------
  Event Bus ve Outbox
--------------------------------------------------------------------------------

• InMemoryEventBus
  Domain event'leri IDomainEventToNotificationMapper ile MediatR notification'a
  çevirir ve IMediator.Publish ile Application'daki INotificationHandler'lara
  gönderir. Tek process içinde, outbox işlendikten sonra handler'lar çalışır.

• RabbitMqEventBus
  Her domain event'i DomainEventEnvelope (Type, Payload, OccurredOn) olarak
  MassTransit ile RabbitMQ'ya publish eder. Başka servisler veya aynı uygulama
  içindeki consumer'lar bu mesajları alabilir.

• DomainEventEnvelopeConsumer (MassTransit IConsumer<DomainEventEnvelope>)
  Kuyruktan gelen envelope'ı deserialize edip IDomainEvent'e çevirir; mapper
  ile notification'a dönüştürüp yine MediatR.Publish ile aynı Application
  handler'larına gönderir. Böylece InMemory ve RabbitMQ aynı iş mantığını
  paylaşır.

EventBus:Provider (appsettings) "InMemory" veya "RabbitMQ" olarak ayarlanır;
DependencyInjection buna göre ilgili implementasyonu kaydeder.


--------------------------------------------------------------------------------
  OutboxMessageProcessor (BackgroundService)
--------------------------------------------------------------------------------

Periyodik (ör. 5 saniyede bir) ProcessedOn = null olan OutboxMessage kayıtlarını
çeker. Her mesajı Type + Payload ile deserialize edip IDomainEvent elde eder;
IEventBus.PublishAsync ile yayımlar. Başarılı işlemde MarkProcessed() + SaveChanges
ile kayıt işlendi olarak işaretlenir. Hata durumunda log atılır, kayıt sonraki
döngüde tekrar denenir (retry). Böylece event'ler en az bir kez teslim edilir.


--------------------------------------------------------------------------------
  Bağımlılıklar
--------------------------------------------------------------------------------

• EF Core, Npgsql (PostgreSQL), MassTransit.RabbitMQ, Serilog, MediatR.
• Application'a referans: arayüzler ve mapper (IDomainEventToNotificationMapper)
  burada kullanılır. Domain'e doğrudan referans yok; Domain Application
  üzerinden gelir.

Yeni bir repository veya event consumer eklerken: arayüzü Application'da
tanımlayın, implementasyonu bu katmanda yazın ve DependencyInjection.cs
içinde kaydedin.


--------------------------------------------------------------------------------
  Kısa Özet
--------------------------------------------------------------------------------

• Persistence: AppDbContext, Repository'ler, UnitOfWork, Outbox yazımı.
• Logging: Serilog → ApplicationLogs; BusinessLogger → BusinessLogs.
• Event bus: InMemory (MediatR) veya RabbitMQ (MassTransit); consumer aynı
  handler'ları kullanır.
• Outbox processor: Outbox tablosunu okuyup event'leri bus'a gönderir.

Bu katmana yeni özellik eklerken: "Application'da arayüz var mı?" sorusunu
sorun; yoksa önce Application'da arayüzü tanımlayın, sonra burada
implemente edin. Domain entity'leri dışarı sızdırmayın; read model ve DTO
Application tarafında kalır.

================================================================================
