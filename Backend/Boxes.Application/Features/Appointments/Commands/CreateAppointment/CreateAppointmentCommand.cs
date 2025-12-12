using Boxes.Application.DTOs;
using MediatR;

namespace Boxes.Application.Features.Appointments.Commands.CreateAppointment
{
    public record CreateAppointmentCommand(
        int PlaceId,
        DateTime AppointmentAt,
        string ServiceType,
        ContactDto Contact,
        VehicleDto? Vehicle
    ) : IRequest<int>;
}
