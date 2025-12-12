namespace Boxes.Application.DTOs
{
    public record CreateAppointmentDto(
        int PlaceId,
        DateTime AppointmentAt,
        string ServiceType,
        ContactDto Contact,
        VehicleDto? Vehicle
    );

    public record ContactDto(
        string Name,
        string Email,
        string? Phone
    );

    public record VehicleDto(
        string? Make,
        string? Model,
        int? Year,
        string? LicensePlate
    );
}
