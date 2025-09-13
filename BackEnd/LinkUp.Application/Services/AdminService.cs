using LinkUp.Application.DTos.Admin;
using LinkUp.Application.Helpers;
using LinkUp.Application.Interfaces.Repository;
using LinkUp.Application.Interfaces.Services;
using LinkUp.Application.Mappers;
using LinkUp.Application.Pagination;
using LinkUp.Application.Utils;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace LinkUp.Application.Services;

public class AdminService(
    ILogger<AdminService> logger,
    IDistributedCache cache,
    IAdminRepository adminRepository,
    IUserRepository userRepository,
    ICloudinaryService cloudinaryService
    ): IAdminService
{
    public async Task<ResultT<AdminDto>> CreateAsync(CreateAdminDto entity, CancellationToken cancellationToken)
    {
        var adminExist = await adminRepository.EmailExistAsync(entity.Email ?? String.Empty, cancellationToken);
        if (adminExist)
        {
            logger.LogWarning("Email already exist");
            return ResultT<AdminDto>.Failure(Error.Conflict("409","Email already exist"));
        }

        var userNameExist = await adminRepository.UserNameExistAsync(entity.UserName ?? String.Empty, cancellationToken);
        if (userNameExist)
        {
            logger.LogWarning("UserName already exist");
            return ResultT<AdminDto>.Failure(Error.Failure("409","UserName already exist"));
        }
        
        string profilePhotoUrl = "";
        if (entity.ProfilePhoto is not null)
        {
            using var stream = entity.ProfilePhoto.OpenReadStream();
            profilePhotoUrl = await cloudinaryService.UploadImageAsync(
                stream,
                entity.ProfilePhoto.FileName,
                cancellationToken
            );
            logger.LogInformation("Profile photo uploaded for user {UserName}.", entity.UserName);
        }
        
        var admin = AdminMapper.ToEntity(entity);
        admin.ProfilePhoto = profilePhotoUrl;
        
        await adminRepository.CreateAsync(admin, cancellationToken);
        logger.LogInformation("Created admin");
        
        return ResultT<AdminDto>.Success(AdminMapper.ToDto(admin));
    }

    public async Task<Result> UpdateAsync(Guid id, UpdateAdminDto entity, CancellationToken cancellationToken)
    {
        var adminResult = await EntityHelper.GetEntityByIdAsync(
            adminEntity => adminRepository.GetByIdAsync(adminEntity, cancellationToken),
            id,
            "Admin",
            logger
        );

        if (!adminResult.IsSuccess) 
            return Result.Failure(adminResult.Error!);

        var admin = adminResult.Value!; 

        AdminMapper.UpdateEntity(admin, entity);

        await adminRepository.UpdateAsync(admin, cancellationToken);

        return Result.Success();
    }


    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var admin = await EntityHelper.GetEntityByIdAsync(
            adminEntity => adminRepository.GetByIdAsync(adminEntity, cancellationToken), id, "Admin", logger);

        if (!admin.IsSuccess) return Result.Failure(admin.Error!);

        await adminRepository.DeleteAsync(admin.Value, cancellationToken);
        logger.LogWarning("Admin deleted");
        
        return Result.Success();
    }

    public async Task<ResultT<AdminDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var adminExist = await EntityHelper.GetEntityByIdAsync(
            adminEntity => adminRepository.GetByIdAsync(adminEntity, cancellationToken), id, "Admin", logger);

        if(!adminExist.IsSuccess) return ResultT<AdminDto>.Failure(adminExist.Error!);

        logger.LogInformation("Admin found");

        return ResultT<AdminDto>.Success(AdminMapper.ToDto(adminExist.Value));
    }

    public async Task<ResultT<PagedResult<AdminDto>>> GetPagedAsync(int page, int size, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultT<string>> BanUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await EntityHelper.GetEntityByIdAsync(
            userEntity => userRepository.GetByIdAsync(userEntity, cancellationToken), userId, "User", logger);

        if (!user.IsSuccess) return ResultT<string>.Failure(user.Error!);
        
        await adminRepository.BanUserAsync(userId, cancellationToken);
        logger.LogInformation("User banned");
        
        return ResultT<string>.Success("User banned successfully");
        
    }

    public async Task<ResultT<string>> UnbanUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await EntityHelper.GetEntityByIdAsync(
            userEntity => userRepository.GetByIdAsync(userEntity, cancellationToken), userId, "User", logger);
        
        if (!user.IsSuccess) return ResultT<string>.Failure(user.Error!);
        
        await adminRepository.UnbanUserAsync(userId, cancellationToken);
        logger.LogInformation("User unbanned");
        
        return ResultT<string>.Success("User unbanned successfully");
    }
}