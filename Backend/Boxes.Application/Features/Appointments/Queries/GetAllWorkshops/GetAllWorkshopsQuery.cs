using Boxes.Application.Common.Interfaces;
using Boxes.Application.DTOs;
using MediatR;

namespace Boxes.Application.Features.Appointments.Queries.GetAllWorkshops
{
    public record GetAllWorkshopsQuery : IRequest<IEnumerable<WorkshopDto>>, ICacheableQuery
    {
        public string CacheKey => nameof(GetAllWorkshopsQuery);
        public TimeSpan? Expiration => TimeSpan.FromMinutes(1);
    }
}
