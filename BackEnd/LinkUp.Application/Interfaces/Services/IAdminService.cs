using LinkUp.Application.DTos.Admin;
using LinkUp.Application.Interfaces.Base;
using LinkUp.Application.Pagination;
using LinkUp.Application.Utils;

namespace LinkUp.Application.Interfaces.Services;

public interface IAdminService: IGenericService<CreateAdminDto, UpdateAdminDto, AdminDto>
{
    Task<ResultT<string>> BanUserAsync(Guid userId,CancellationToken cancellationToken);
    Task<ResultT<string>> UnbanUserAsync(Guid userId, CancellationToken cancellationToken);
}