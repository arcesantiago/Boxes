namespace Boxes.Application.DTOs
{
    public record WorkshopDto(
        int Id,
        string Name,
        string? Address,
        bool IsActive
    );
}
