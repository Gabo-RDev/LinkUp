namespace LinkUp.Application.DTos.Admin;

public record UpdateAdminDto(
    string? FirstName,
    string? LastName,
    string? ProfilePhoto
    );