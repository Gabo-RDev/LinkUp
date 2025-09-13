using LinkUp.Application.DTos.Interests;
using LinkUp.Domain.Models;

namespace LinkUp.Application.Mappers;

public static class InterestMapper
{
    public static InterestDto ToDto(Interest interest)
    {
        return new InterestDto(
            InterestId: interest.Id,
            Name: interest.Name,
            CreatedAt: interest.CreatedAt,
            UpdatedAt: interest.UpdatedAt
        );
    }

    public static Interest ToEntity(CreateUpdateInterestDto createUpdateInterestDto)
    {
        return new Interest
        {
            Id = Guid.NewGuid(),
            Name = createUpdateInterestDto.Name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}