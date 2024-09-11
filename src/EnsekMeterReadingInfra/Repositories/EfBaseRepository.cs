using System;

namespace EnsekMeterReadingInfra.Repositories;

public abstract class EfBaseRepository
{
    protected readonly EnsekDbContext _dbContext;

    public EfBaseRepository(EnsekDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task Save() {
        await _dbContext.SaveChangesAsync();
    }
}
