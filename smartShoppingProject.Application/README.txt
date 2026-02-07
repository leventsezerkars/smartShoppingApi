================================================================================
  smartShoppingProject.Application — Katman Rehberi
================================================================================

Bu katman, Domain ile dış dünya (API, veritabanı, mesaj kuyruğu) arasındaki
"use case" katmanıdır. İş akışları burada orkestre edilir; fakat iş kurallarının
kendisi Domain'de kalır. Application, Domain'e referans verir; Infrastructure
ve API ise Application'a referans verir. Böylece use case'ler altyapıdan
bağımsız yazılır ve test edilir.


--------------------------------------------------------------------------------
  Klasör Yapısı
--------------------------------------------------------------------------------

Abstractions   → Repository, UnitOfWork, EventBus, IBusinessLogger vb. arayüzler
Behaviors      → MediatR pipeline: Logging, Validation, Transaction
Common         → Response<T>, PagedResponse, IResponse (standart dönüş tipleri)
Events         → Domain event → MediatR notification map + handler'lar
Feature        → CQRS use case'leri (Orders, Products, Categories)
DependencyInjection.cs → MediatR, FluentValidation, mapper kayıtları


--------------------------------------------------------------------------------
  CQRS ve MediatR
--------------------------------------------------------------------------------

Her işlem bir Command veya Query ile temsil edilir. CreateOrderCommand,
GetOrderByIdQuery gibi. Handler'lar IRequestHandler<TRequest, TResponse>
uygular; MediatR isteği ilgili handler'a yönlendirir.

Command'lar ICommand<TResponse> ile işaretlenir. TransactionBehavior yalnızca
command'lar için transaction açar; query'ler read-only olduğundan transaction
açılmaz. Validator'lar FluentValidation ile request başına çalışır; hata
durumunda ValidationException fırlatılır ve GlobalExceptionMiddleware
bunu 400 + Response formatında döner.


--------------------------------------------------------------------------------
  Pipeline Sırası (Behavior'lar)
--------------------------------------------------------------------------------

1. ValidationBehavior  → FluentValidation; geçersiz istekler erken elenir.
2. LoggingBehavior     → Sadece exception loglanır (teknik log, Serilog).
3. TransactionBehavior → Sadece ICommand için UnitOfWork transaction + SaveChanges.

Handler içinde repository ve domain kullanılır; UnitOfWork.SaveChanges
handler tarafından çağrılmaz, TransactionBehavior sorumludur.


--------------------------------------------------------------------------------
  Response ve DTO'lar
--------------------------------------------------------------------------------

Tüm handler'lar Response<T> veya PagedResponse<T> döner. Success, ErrorMessage,
Data alanları API'de aynı JSON yapısıyla kullanılır. Domain entity'ler
doğrudan dışarı verilmez; DTO veya ReadModel kullanılır. Query'lerde
repository bazen OrderReadModel gibi okuma modelleri döner; handler bunları
DTO'ya map edip Response.Ok(dto) ile döner.


--------------------------------------------------------------------------------
  Loglama Ayrımı (Teknik vs İş)
--------------------------------------------------------------------------------

• ILogger (Serilog) → Exception, pipeline, EF, HTTP hataları. ApplicationLog
  tablosuna gider. "Sistem neden bozuldu?" sorusuna cevap verir.

• IBusinessLogger → OrderCreated, OrderCancelled, ProductPriceChanged gibi
  iş anlamı taşıyan olaylar. BusinessLogs tablosuna yazılır. "Sistem ne
  yaptı?" sorusuna cevap verir. Domain event handler'ları ve önemli command
  sonuçları burada loglanır.

CorrelationId, middleware tarafından set edilir; ICorrelationIdAccessor ile
okunur. Hem teknik hem iş loglarında aynı istek takip id'si kullanılabilir.


--------------------------------------------------------------------------------
  Domain Event'ler ve Handler'lar
--------------------------------------------------------------------------------

Domain'deki event'ler (OrderCreatedEvent, OrderCancelledEvent,
ProductPriceChangedEvent) Outbox ile persist edilir; Infrastructure'daki
OutboxMessageProcessor bunları IEventBus ile yayımlar. Application katmanında:

• IDomainEventToNotificationMapper → Domain event'i MediatR INotification'a
  çevirir (OrderCreatedNotification vb.).
• INotificationHandler<T> → Her event tipi için handler; bildirim, entegrasyon,
  cache invalidation vb. Bu handler'lar iş anlamını IBusinessLogger ile
  BusinessLogs tablosuna yazar; Serilog kullanılmaz.

InMemoryEventBus ve RabbitMQ consumer aynı mapper + MediatR.Publish ile
aynı handler'ları tetikler.


--------------------------------------------------------------------------------
  Abstractions (Arayüzler)
--------------------------------------------------------------------------------

Repository'ler (IOrderRepository, IProductRepository, ICategoryRepository),
IUnitOfWork, IEventBus, IBusinessLogger, ICorrelationIdAccessor bu katmanda
tanımlanır. Implementasyonlar Infrastructure veya API'de (ör. CorrelationIdAccessor)
yer alır. Application hiçbir zaman Infrastructure'a referans vermez; DI
composition root (API) tarafından çözülür.


--------------------------------------------------------------------------------
  Kısa Özet
--------------------------------------------------------------------------------

• Use case'ler Command/Query + Handler + Validator ile yazılır.
• Domain kuralları entity'de kalır; Application orkestrasyon yapar.
• Teknik log = ILogger (Serilog); iş/audit log = IBusinessLogger.
• Domain event'ler notification'a map edilip aynı handler'larla işlenir.
• Response<T> ile tutarlı API yanıt formatı sağlanır.

Bu katmana yeni özellik eklerken: Yeni bir iş akışı için Command/Query + Handler
ekleyin; repository arayüzü zaten varsa kullanın, yoksa Abstractions altında
tanımlayıp Infrastructure'da implemente edin.

================================================================================
