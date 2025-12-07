namespace OrderInventory.Application.Abstractions;

public interface IUnitOfWork
{
    public Task<int> CommitChangesAsync(CancellationToken cancellationToken = default);
}
