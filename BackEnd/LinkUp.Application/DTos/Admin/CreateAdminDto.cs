using Microsoft.AspNetCore.Http;

namespace LinkUp.Application.DTos.Admin;

public record CreateAdminDto(
    string? FirstName,
    string? LastName,
    string? UserName,
    string? Email,
    string? Password,
    IFormFile? ProfilePhoto
    );