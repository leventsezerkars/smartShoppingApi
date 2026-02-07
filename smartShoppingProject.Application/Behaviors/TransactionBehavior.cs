namespace smartShoppingProject.Application.Behaviors;

using smartShoppingProject.Application.Abstractions;
using smartShoppingProject.Application.Abstractions.Persistence;
using MediatR;

/// <summary>
/// Yalnızca command'lar için transaction yönetimi. Handler öncesi transaction başlatır, sonrası commit; exception'da rollback.
/// Handler içinde UnitOfWork çağrılmaz; bu behavior sorumludur.
/// </summary>
public sealed class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ICommand<TResponse>)
        {
            return await next();
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var response = await next();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return response;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
