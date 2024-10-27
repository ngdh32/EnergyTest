using System;
using EnsekMeterReadingCore.Entities;

namespace EnsekMeterReadingCore.Repositories;

public interface IMeterReadingRepository : IRepository<MeterReadingEntity>
{
    public MeterReadingEntity? GetAccountLastReadingTime(int accountId);
}
