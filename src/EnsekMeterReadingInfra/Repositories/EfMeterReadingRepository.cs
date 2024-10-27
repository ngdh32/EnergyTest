using System;
using EnsekMeterReadingCore.Entities;
using EnsekMeterReadingCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EnsekMeterReadingInfra.Repositories;

public class EfMeterReadingRepository : EfBaseRepository, IMeterReadingRepository
{
    public EfMeterReadingRepository(EnsekDbContext dbContext) : base(dbContext) {}

    public async Task AddAsync(MeterReadingEntity entity)
    {
        await _dbContext.MeterReadings.AddAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbContext.MeterReadings.FindAsync(id);
        if (entity != null)
        {
            _dbContext.MeterReadings.Remove(entity);
        }
    }

    public DateTime? GetAccountLastReadingTime(int accountId)
    {
        var entities = _dbContext.MeterReadings.Where(x => x.AccountId == accountId);
        return entities?.Any() == true ? entities.Max(x => x.ReadingTime) : null;
    }

    public async Task<IEnumerable<MeterReadingEntity>> GetAllAsync()
    {
        return await _dbContext.MeterReadings.ToListAsync();
    }

    public async Task UpdateAsync(MeterReadingEntity entity)
    {
        var existingEntity = await _dbContext.MeterReadings.FindAsync(entity.Id);
        if (existingEntity != null)
        {
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        }
    }

}
