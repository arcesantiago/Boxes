using AutoMapper;
using Boxes.Application.Contracts.Persistence;
using Boxes.Application.DTOs;
using MediatR;

namespace Boxes.Application.Features.Appointments.Queries.GetAllAppointments
{
    public class GetAllAppointmentsQueryHandler(
        IAppointmentRepository appointmentRepository,
        IMapper mapper) : IRequestHandler<GetAllAppointmentsQuery, IEnumerable<AppointmentDto>>
    {
        private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<AppointmentDto>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetListAsync(cancellationToken: cancellationToken));
        }
    }
}
