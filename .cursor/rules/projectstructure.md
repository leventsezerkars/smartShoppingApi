# Cursor Rules – Akıllı Sipariş Yönetim Sistemi (.NET)

Bu dosya Cursor AI ile **senior seviyede, production kalitesinde** bir Order Management API geliştirmek için hazırlanmıştır. Cursor’dan beklenti: sadece kod yazması değil, **neden–sonuç ilişkisi kurarak** karar almasıdır.

---

## 1. PROJE HİKÂYESİ (BUSINESS CONTEXT)

Bu proje, bir e-ticaret sisteminin **çekirdeği** olan sipariş yönetimini ele alır.

Gerçek hayatta yaşanan problemler:

* Aynı siparişin birden fazla kez oluşturulması (retry / timeout)
* Concurrency problemleri
* Transaction hataları
* Performans darboğazları
* Gözlemlenebilirlik eksikliği

Amaç: **Basit CRUD değil, production’da yaşayan bir sistem** inşa etmek.

---

## 2. TEKNOLOJİ SETİ

Cursor aşağıdaki teknolojileri **bilinçli şekilde** kullanmalıdır:

* .NET 8 (ASP.NET Core Web API)
* Entity Framework Core
* PostgreSQL (tercih) veya SQL Server
* MediatR (CQRS için)
* FluentValidation
* Serilog
* Polly
* Swagger (OpenAPI)

Opsiyonel (ileride):

* Redis (cache / idempotency)

---

## 3. MİMARİ YAKLAŞIM

### Seçilen Mimari: **Modüler Monolith + Clean Architecture Prensipleri**

Microservice YOK.
Gereksiz dağıtıklık YOK.
Net sınırlar VAR.

Katmanlar:

```
src
│
├── OrderService.Api
│
├── OrderService.Application
│   ├── Commands
│   ├── Queries
│   ├── DTOs
│   ├── Validators
│   ├── Interfaces
│
├── OrderService.Domain
│   ├── Entities
│   ├── Enums
│   ├── ValueObjects
│   ├── Exceptions
│
├── OrderService.Infrastructure
│   ├── Persistence
│   ├── Configurations
│   ├── Repositories
│   ├── Migrations
│
└── OrderService.Shared
    ├── Results
    ├── ErrorCodes
    ├── Extensions
```

---

## 4. DOMAIN MODEL (ZORUNLU KURALLAR)

### Order Entity

* Entity **anemic olmayacak**
* Business kuralları entity içinde korunacak

Alanlar:

* Id (Guid)
* OrderNumber (string, unique)
* CustomerId (Guid)
* Status (enum)
* TotalAmount (decimal)
* CreatedAt (UTC)
* RowVersion (byte[] – optimistic concurrency)

OrderStatus Enum:

* Pending
* Paid
* Shipped
* Completed
* Cancelled

State geçişleri **kontrollü** olacak.

---

## 5. DB HİKÂYESİ

### Temel Tablolar

#### Orders

* PK: Id
* Unique Index: OrderNumber
* RowVersion (concurrency)

#### OrderItems

* FK: OrderId
* ProductId
* Quantity
* UnitPrice

#### IdempotentRequests

* IdempotencyKey (unique)
* ResponsePayload
* CreatedAt

---

## 6. KRİTİK TEKNİK KURALLAR (Cursor MUTLAKA UYSUN)

### Asenkron Kurallar

* async void YASAK
* Task / Task<T> kullanılacak

### EF Core

* Read işlemlerinde AsNoTracking
* Projection kullanılacak
* SaveChangesAsync tek noktada

### Transaction

* Transaction scope minimumda tutulacak
* Deadlock riskine dikkat edilecek

### Idempotency

* Header üzerinden Idempotency-Key zorunlu
* Duplicate request DB seviyesinde engellenecek

### Exception Handling

* Controller içinde try-catch YOK
* Global Exception Middleware VAR

---

## 7. CQRS KULLANIMI

### Commands

* CreateOrderCommand
* PayOrderCommand

### Queries

* GetOrderByIdQuery
* GetOrdersByCustomerQuery

Command ve Query birbirine karışmayacak.

---

## 8. LOGGING & OBSERVABILITY

* RequestId loglanacak
* OrderId loglanacak
* Elapsed time ölçülecek

Serilog kullanılacak.

Log seviyesi:

* Information: business flow
* Warning: retry / beklenmeyen ama tolere edilen durumlar
* Error: exception

---

## 9. API TASARIMI

Endpoint örnekleri:

* POST /api/orders
* GET /api/orders/{id}
* POST /api/orders/{id}/pay

HTTP status code’lar **doğru** kullanılacak.

---

## 10. CURSOR'DAN BEKLENTİ

Cursor şunları yapmalı:

* Kod yazarken açıklama eklemeli
* "Neden böyle yaptığını" belirtmeli
* Alternatifleri kısaca not düşmeli

Cursor şunları YAPMAMALI:

* Over-engineering
* Gereksiz abstraction
* Microservice önermesi

---

## 11. GELİŞTİRME STRATEJİSİ (ADIM ADIM)

1. Solution ve katmanlar oluşturulacak
2. Domain model yazılacak
3. DbContext ve migration
4. CreateOrder flow
5. Idempotency mekanizması
6. Global exception middleware
7. Logging

Her adım **çalışır halde** commitlenecek.

---

## 12. GİTHUB BEKLENTİSİ

* Anlamlı commit mesajları
* README'de mimari açıklama
* "Known trade-offs" bölümü

---

Bu dosya, Cursor için bağlayıcıdır.
Kod üretirken bu kurallar referans alınacaktır.
