namespace smartShoppingProject.Application.Abstractions;

using MediatR;

/// <summary>
/// Command işaretleyici arayüzü. TransactionBehavior yalnızca command'lar için çalışır.
/// </summary>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
