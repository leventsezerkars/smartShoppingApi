namespace smartShoppingProject.Domain.Exceptions;

public sealed class InvalidOrderItemException : DomainException
{
    public InvalidOrderItemException(string message)
        : base(message)
    {
    }
}
