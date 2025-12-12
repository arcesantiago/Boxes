using Boxes.Application.DTOs;

namespace Boxes.Application.Contracts.Interfaces
{
    public interface IWorkshopService
    {
        Task<IEnumerable<WorkshopDto>> GetActiveWorkshopsAsync(CancellationToken cancellationToken = default);
        Task<WorkshopDto?> GetWorkshopByIdAsync(int placeId, CancellationToken cancellationToken = default);
    }
}
