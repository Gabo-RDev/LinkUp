using LinkUp.Domain.Enum;

namespace LinkUp.Application.DTos.Admin;

public record AdminDto(
    Guid Id,
    string? FirstName,
    string? LastName,
    string? UserName,
    string? Email,
    string? ProfilePhoto,
    AdminStatus Status
    );