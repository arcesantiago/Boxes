using Boxes.Application.DTOs;
using MediatR;

namespace Boxes.Application.Features.Appointments.Queries.GetAllAppointments
{
    public record GetAllAppointmentsQuery : IRequest<IEnumerable<AppointmentDto>>;
}
