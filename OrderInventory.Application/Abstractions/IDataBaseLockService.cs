namespace OrderInventory.Application.Abstractions;

public interface IDataBaseLockService
{
    public Task<IAsyncDisposable> AcquireLockAsync(string resource, TimeSpan timeout, CancellationToken cancellationToken = default);
}
