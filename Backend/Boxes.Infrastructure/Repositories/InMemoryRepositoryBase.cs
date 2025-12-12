using Boxes.Application.Contracts.Persistence.Read;
using Boxes.Application.Contracts.Persistence.Write;
using Boxes.Domain.Common;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Boxes.Infrastructure.Repositories;

public class InMemoryRepositoryBase<T> : IReadRepository<T>, IWriteRepository<T> where T : BaseDomainModel
{
    protected readonly ConcurrentDictionary<int, T> _store = new();
    private int _nextId = 1;
    private readonly object _lockObject = new();

    public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        lock (_lockObject)
        {
            if (entity.Id == 0)
            {
                var idProperty = typeof(BaseDomainModel).GetProperty(nameof(BaseDomainModel.Id));
                idProperty?.SetValue(entity, _nextId++);
            }

            if (entity.CreatedAt == default)
            {
                var createdAtProperty = typeof(BaseDomainModel).GetProperty(nameof(BaseDomainModel.CreatedAt));
                createdAtProperty?.SetValue(entity, DateTimeOffset.UtcNow);
            }

            var updatedAtProperty = typeof(BaseDomainModel).GetProperty(nameof(BaseDomainModel.UpdatedAt));
            updatedAtProperty?.SetValue(entity, DateTimeOffset.UtcNow);

            _store.TryAdd(entity.Id, entity);
        }

        return Task.FromResult(entity);
    }
    public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var compiledPredicate = predicate.Compile();
        var exists = _store.Values.Any(compiledPredicate);
        return Task.FromResult(exists);
    }

    public Task<T?> FindAsync(int id, CancellationToken cancellationToken = default)
    {
        _store.TryGetValue(id, out var entity);
        return Task.FromResult<T?>(entity);
    }

    public Task<IReadOnlyList<T>> GetListAsync(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, T>>? select = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        IEnumerable<Expression<Func<T, object>>>? includeProperties = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = _store.Values.AsQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }
        else
        {
            query = query.OrderByDescending(e => e.CreatedAt);
        }

        if (select != null)
        {
            query = query.Select(select);
        }

        var result = query.ToList().AsReadOnly();
        return Task.FromResult<IReadOnlyList<T>>(result);
    }
}