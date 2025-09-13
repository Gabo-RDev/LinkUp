using LinkUp.Application.DTos.Interests;
using LinkUp.Application.Interfaces.Base;

namespace LinkUp.Application.Interfaces.Services;

public interface IInterestService : IGenericService<CreateUpdateInterestDto, CreateUpdateInterestDto, InterestDto>;