using Boxes.Application.Models;

namespace Boxes.Application.Contracts.Interfaces
{
    public interface IWorkshopService
    {
        Task<IEnumerable<Workshop>> GetActiveWorkshopsAsync(CancellationToken cancellationToken = default);
        Task<Workshop?> GetWorkshopByIdAsync(int placeId, CancellationToken cancellationToken = default);
    }
}
