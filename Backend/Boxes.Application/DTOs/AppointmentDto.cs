namespace Boxes.Application.DTOs
{
    public record AppointmentDto(
        int Id,
        int PlaceId,
        DateTime AppointmentAt,
        string ServiceType,
        ContactDto Contact,
        VehicleDto? Vehicle,
        DateTimeOffset CreatedAt
    );
}
