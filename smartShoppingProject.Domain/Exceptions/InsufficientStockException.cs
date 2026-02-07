namespace smartShoppingProject.Domain.Exceptions;

public sealed class InsufficientStockException : DomainException
{
    public InsufficientStockException(Guid productId, int available, int requested)
        : base($"Ürün için yetersiz stok: '{productId}'. Mevcut: {available}, talep edilen: {requested}.")
    {
    }
}
