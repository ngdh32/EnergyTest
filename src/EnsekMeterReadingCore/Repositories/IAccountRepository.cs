using System;
using EnsekMeterReadingCore.Entities;

namespace EnsekMeterReadingCore.Repositories;

public interface IAccountRepository : IRepository<AccountEntity>
{
    public Task<AccountEntity> GetByIdAsync(int id);
}
