namespace OrderInventory.Application.Abstractions;

public interface IUnitOfWork
{
    public Task<int> CommitChangesAsync(CancellationToken cancellationToken = default);

    public Task ExecuteInTransactionAsync(
        Func<CancellationToken, Task> operation,
        CancellationToken cancellationToken = default);
}
