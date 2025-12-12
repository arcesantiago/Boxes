using AutoMapper;
using Boxes.Application.Contracts.Interfaces;
using Boxes.Application.DTOs;
using MediatR;

namespace Boxes.Application.Features.Appointments.Queries.GetAllWorkshops
{
    public class GetAllWorkshopsQueryHandler(
        IWorkshopService workshopService,
        IMapper mapper) : IRequestHandler<GetAllWorkshopsQuery, IEnumerable<WorkshopDto>>
    {
        private readonly IWorkshopService _workshopService = workshopService;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<WorkshopDto>> Handle(GetAllWorkshopsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<WorkshopDto>>(await _workshopService.GetActiveWorkshopsAsync(cancellationToken));
        }
    }
}
