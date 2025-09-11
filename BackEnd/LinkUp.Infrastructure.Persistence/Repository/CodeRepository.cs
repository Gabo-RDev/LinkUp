using LinkUp.Application.Interfaces.Repository;
using LinkUp.Domain.Models;
using LinkUp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repository;

public class CodeRepository(LinkUpDbContext context): GenericRepository<Code>(context), ICodeRepository
{
    public async Task CreateCodeAsync(Code code, CancellationToken cancellationToken)
    {
        await context.Set<Code>().AddAsync(code, cancellationToken);
        await SaveAsync(cancellationToken);
    }

    public async Task<Code> GetCodeByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await context.Set<Code>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<Code> GetCodeByValueAsync(string value, CancellationToken cancellationToken) =>
        await context.Set<Code>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Value == value, cancellationToken);

    public async Task<bool> IsCodeValidAsync(string code, CancellationToken cancellationToken) =>
        await ValidateAsync(c => c.Value == code && c.Expiration > DateTime.UtcNow && !c.Used, cancellationToken);

    public async Task MarkCodeAsUsedAsync(string code, CancellationToken cancellationToken)
    {
        var userCode = await context.Set<Code>()
            .FirstOrDefaultAsync(c => c.Value == code, cancellationToken);

        if (userCode != null)
        {
            userCode.Used = true;
            await SaveAsync(cancellationToken);
        }
    }

    public async Task<bool> IsCodeUsedAsync(string code, CancellationToken cancellationToken) =>
        await ValidateAsync( c=> c.Value == code && c.Used, cancellationToken);

}