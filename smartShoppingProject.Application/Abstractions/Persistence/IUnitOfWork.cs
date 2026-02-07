namespace smartShoppingProject.Application.Abstractions.Persistence;

/// <summary>
/// İş birimi sözleşmesi. Transaction ve kaydetme Infrastructure'da uygulanır.
/// Transaction yönetimi handler içinde değil, TransactionBehavior ile yapılır.
/// </summary>
public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
