using LinkUp.Application.DTos.Admin;
using LinkUp.Domain.Enum;
using LinkUp.Domain.Models;

namespace LinkUp.Application.Mappers;

public static class AdminMapper
{
    public static AdminDto ToDto(this Admin admin)
    {
        return new AdminDto(
            Id: admin.Id,
            FirstName: admin.FirstName,
            LastName: admin.LastName,
            UserName: admin.UserName,
            Email: admin.Email,
            ProfilePhoto: admin.ProfilePhoto,
            Status: admin.Status
        );
    }
    
    public static Admin ToEntity(this CreateAdminDto dto)
    {
        return new Admin
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.UserName,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Status = AdminStatus.Active,
            ConfirmedAccount = true
        };
    }
    
    public static void UpdateEntity(this Admin admin, UpdateAdminDto dto)
    {
        admin.FirstName = dto.FirstName;
        admin.LastName = dto.LastName;
        admin.ProfilePhoto = dto.ProfilePhoto;
    }
}