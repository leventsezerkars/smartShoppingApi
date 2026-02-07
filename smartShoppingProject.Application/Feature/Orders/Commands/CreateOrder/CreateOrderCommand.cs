namespace smartShoppingProject.Application.Orders.Commands.CreateOrder;

using Abstractions;
using smartShoppingProject.Application.Common.Responses;
using MediatR;

/// <summary>
/// Yeni sipariş oluşturma komutu. State değiştirir.
/// </summary>
public sealed record CreateOrderCommand(
    Guid CustomerId,
    IReadOnlyList<CreateOrderItemCommand> Items) : ICommand<Response<CreateOrderResponse>>;

/// <summary>
/// Sipariş kalemi bilgisi; komut tarafında sadece ürün ve miktar. Birim fiyat handler'da ürün üzerinden alınır.
/// </summary>
public sealed record CreateOrderItemCommand(Guid ProductId, int Quantity);
