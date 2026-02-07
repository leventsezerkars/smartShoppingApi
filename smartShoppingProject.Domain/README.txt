================================================================================
  smartShoppingProject.Domain — Katman Rehberi
================================================================================

Bu klasör, uygulamanın "kalbi" olan Domain katmanını içerir. Burada e-ticaret
iş kuralları, varlıklar ve kavramlar framework’ten bağımsız şekilde tanımlanır.
ASP.NET, EF Core veya veritabanı bu katmana referans vermez; tam tersi olur.
Böylece iş mantığı tek bir yerde toplanır ve test etmek ya da ileride farklı
bir altyapıya taşımak kolaylaşır.


--------------------------------------------------------------------------------
  Klasör Yapısı
--------------------------------------------------------------------------------

Common      → Tüm entity’lerin ortak tabanı (BaseEntity, AggregateRoot)
Entities    → İş varlıkları: Order, OrderItem, Product, Category
ValueObjects→ Kimliği olmayan, değerle tanımlanan tipler (örn. Money)
Enums       → Sınırlı seçenekler: OrderStatus, Currency
Exceptions  → Domain’e özel hata tipleri
Events      → "Şu olay gerçekleşti" kayıtları (OrderCreated, OrderCancelled vb.)


--------------------------------------------------------------------------------
  BaseEntity ve AggregateRoot
--------------------------------------------------------------------------------

BaseEntity, tüm entity’lerin temelidir. Id, CreatedAt, UpdatedAt gibi ortak
alanları ve domain event listesini taşır. Event’ler, "kaydettikten sonra
bir şeyler tetiklemek" istediğimizde kullanılır.

AggregateRoot ise BaseEntity’den türeyen özel bir işaret: "Bu sınıf, kendi
sınırı içindeki tutarlılıktan sorumlu kök varlıktır" der. Dış dünya (API,
application service) sadece aggregate root’a dokunur; içindeki parçalara
doğrudan erişmez.

Örnek: Order bir Aggregate Root’tur. Sipariş kalemleri (OrderItem) yalnızca
Order üzerinden eklenip çıkarılır. Böylece toplam tutar ve kurallar hep
Order’da toplanır. Product da kendi başına bir aggregate root’tur; fiyat
ve stok kuralları burada yaşar. Category ise sadece BaseEntity’den türer;
karmaşık bir alt modeli yoktur.


--------------------------------------------------------------------------------
  Value Object: Money
--------------------------------------------------------------------------------

Para tutarı ve birimi her yerde birlikte gezer. Bu yüzden "decimal fiyat"
yerine Money tipi kullanıyoruz. Money’nin kimliği yoktur; 10 TRY ile 10 TRY
aynı "değeri" ifade eder. Bu da onu bir Value Object yapar.

Money, Amount >= 0 kontrolü yapar, para birimini Currency enum’u ile
sınırlar (TRY, USD, EUR, GBP). Toplama ve "fiyat × adet" gibi işlemler
Money üzerinden yapılır; farklı para birimleri karıştırılmaz.

  Örnek:
    var birimFiyat = new Money(99.50m, Currency.TRY);
    var toplam = birimFiyat * 3;   // 298.50 TRY


--------------------------------------------------------------------------------
  Entity’ler ve İş Kuralları
--------------------------------------------------------------------------------

Kurallar entity’nin içinde, setter’ların ardına gizlenmiştir. Dışarıdan
"stok = -5" atayamazsın; DecreaseStock metodu negatife düşmeyi engeller
ve gerekirse InsufficientStockException fırlatır. Benzer şekilde Order
yalnızca Create(...) ile oluşturulur, en az bir kalem zorunludur; durum
geçişleri (Beklemede → Ödendi → Kargoya verildi → Tamamlandı) metotlarla
yapılır ve "ödemeden kargoya verme" gibi geçişler engellenir.

Yani Domain hem veriyi hem davranışı taşır; anemic (sadece get/set) model
yoktur.


--------------------------------------------------------------------------------
  Domain Event’ler
--------------------------------------------------------------------------------

Bir aggregate’ta önemli bir şey olduğunda (sipariş oluşturuldu, iptal
edildi, ürün fiyatı değişti) ilgili event nesnesi oluşturulup aggregate’ın
DomainEvents listesine eklenir. Persistence katmanı (ör. EF Core) aggregate’ı
kaydettikten sonra bu listeyi okuyup event’leri yayımlar.

Böylece "sipariş oluşturuldu" dendiğinde stok düşümü, ödeme tetiklemesi,
e-posta veya loglama gibi işler uygulama/infrastructure tarafında
handler’larla yapılır; domain sadece "ne oldu" bilgisini üretir. İleride
bu event’ler bir message queue’ya (RabbitMQ, Azure Service Bus vb.)
gönderilerek diğer servisler de haberdar edilebilir.


--------------------------------------------------------------------------------
  Exception’lar
--------------------------------------------------------------------------------

DomainException tüm domain hatalarının temelidir. InvalidPriceException,
InsufficientStockException, InvalidOrderStateException, InvalidOrderItemException
gibi tipler belirli kurallar ihlal edildiğinde fırlatılır. Mesajlar Türkçe
ve loglama / kullanıcıya iletme için anlamlı tutulur. API veya application
katmanı bu exception’ları yakalayıp uygun HTTP yanıtına veya sonuç
nesnesine dönüştürür.


--------------------------------------------------------------------------------
  Kısa Özet
--------------------------------------------------------------------------------

• Domain, framework’ten ve altyapıdan bağımsızdır.
• İş kuralları entity ve value object’lerin içinde yaşar.
• Aggregate root’lar (Order, Product) tutarlılık sınırının giriş noktasıdır.
• Event’ler "ne oldu" bilgisini taşır; "ne yapılacak" uygulama tarafında karar verilir.
• Para birimi Currency enum, tutar ise Money value object ile ifade edilir.

Bu katmana yeni özellik eklerken: "Bu kural nerede yaşamalı?" sorusunu
domain’de cevaplayın; veritabanı, API veya UI detaylarını buraya taşımayın.

================================================================================
