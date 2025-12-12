namespace Boxes.Application.Models
{
    public record Workshop(
        int Id,
        string Name,
        string? Address,
        bool IsActive
    );
}
