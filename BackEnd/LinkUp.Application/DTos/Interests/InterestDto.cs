namespace LinkUp.Application.DTos.Interests;

public sealed record InterestDto(
    Guid? InterestId,
    string? Name,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);