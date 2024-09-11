using System;
using EnsekMeterReadingCore.Entities;
using EnsekMeterReadingCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EnsekMeterReadingInfra.Repositories;

public class EfAccountRepository : EfBaseRepository, IAccountRepository
{   
    public EfAccountRepository(EnsekDbContext dbContext) : base(dbContext) {}

    public async Task AddAsync(AccountEntity entity)
    {
        await _dbContext.Accounts.AddAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbContext.Accounts.FindAsync(id);
        if (entity != null)
        {
            _dbContext.Accounts.Remove(entity);
        }
    }

    public async Task<IEnumerable<AccountEntity>> GetAllAsync()
    {
        return await _dbContext.Accounts.ToListAsync();
    }

    public async Task<AccountEntity?> GetByIdAsync(int id)
    {
        return await _dbContext.Accounts.FindAsync(id);
    }

    public async Task UpdateAsync(AccountEntity entity)
    {
        var existingEntity = await _dbContext.Accounts.FindAsync(entity.Id);
        if (existingEntity != null)
        {
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        }
    }
}
