namespace Boxes.Application.DTOs;

/// <summary>
/// DTO que representa la respuesta de la API externa de talleres
/// </summary>
public record WorkshopDto(
    int Id,
    string? Name,
    string? Email,
    string? Email2,
    string? Phone,
    string? Phone2,
    string? Phone3,
    AddressResponse? Address,
    string? DefaultAddress,
    string? Website,
    string? SocialFacebook,
    string? SocialTwitter,
    string? SocialLinkedIn,
    TimeZoneInfoResponseDto? TimeZone,
    IReadOnlyList<ScheduleResponseDto>? Schedules,
    IReadOnlyList<RelationshipResponseDto>? Relationships,
    string? Type,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    DateTime? RemovedAt,
    string? Group,
    string? FormattedAddress,
    bool Active, // La API usa "Active", no "IsActive"
    string? DefaultFormattedAddress,
    string? AreaCode,
    string? CountryCode,
    string? ZoneInformation,
    string? MakeCode,
    int? TimePerShift,
    decimal? AmountPerShift,
    int? MaximumPerDay,
    int? MinimumAnticipationDays,
    object? ExternalsCrm,
    object? Externals,
    string? ResourceType
);

public record TimeZoneInfoResponseDto(
    string? Id,
    bool? HasIanaId,
    string? DisplayName,
    string? StandardName,
    string? DaylightName,
    string? BaseUtcOffset,
    bool? SupportsDaylightSavingTime
);

public record ScheduleResponseDto(
    string? Day,
    string? StartTime,
    string? EndTime,
    bool? IsActive
);

public record RelationshipResponseDto(
    int? Id,
    string? Type,
    string? Name
);

public sealed record AddressResponse(
    IReadOnlyList<AddressComponentResponse> AddressComponents,
    string AdrAddress,
    string FormattedAddress,
    GeometryResponse Geometry,
    string Icon,
    string IconBackgroundColor,
    string IconMaskBaseUri,
    string Name,
    string PlaceId,
    string Reference,
    IReadOnlyList<string> Types,
    string Url,
    string Vicinity,
    IReadOnlyList<string> HtmlAttributions,
    int UtcOffsetMinutes,
    IReadOnlyList<PhotoResponse>? Photos,
    string? Website
);

public sealed record AddressComponentResponse(
    string LongName,
    string ShortName,
    IReadOnlyList<string> Types
);

public sealed record GeometryResponse(
    LocationResponse Location,
    ViewportResponse Viewport
);

public sealed record LocationResponse(
    double Lat,
    double Lng
);

public sealed record ViewportResponse(
    double South,
    double West,
    double North,
    double East
);

public sealed record PhotoResponse(
    int Height,
    int Width,
    IReadOnlyList<string> HtmlAttributions
);
