namespace Boxes.Infrastructure.Repositories;

public class InMemoryUnitOfWorkBase : IDisposable
{
    private bool _disposed = false;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(1);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;
        }
    }
}