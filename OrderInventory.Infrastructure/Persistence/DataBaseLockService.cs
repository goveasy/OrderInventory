using Medallion.Threading;
using OrderInventory.Application.Abstractions;
using System;

namespace OrderInventory.Infrastructure.Persistence;

public class DataBaseLockService : IDataBaseLockService
{
    private readonly IDistributedLockProvider _distributedLockProvider;

    public DataBaseLockService(IDistributedLockProvider distributedLockProvider)
    {
        _distributedLockProvider = distributedLockProvider;
    }

    public async Task<IAsyncDisposable> AcquireLockAsync(string resource, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        return await _distributedLockProvider.AcquireLockAsync(resource, timeout, cancellationToken);
    }
}
