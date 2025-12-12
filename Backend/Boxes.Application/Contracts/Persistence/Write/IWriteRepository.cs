using Boxes.Domain.Common;

namespace Boxes.Application.Contracts.Persistence.Write
{
    public interface IWriteRepository<T> where T : BaseDomainModel
    {
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    }
}
