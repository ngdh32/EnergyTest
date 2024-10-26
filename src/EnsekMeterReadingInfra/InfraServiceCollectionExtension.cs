using System;
using EnsekMeterReadingCore.Entities;
using EnsekMeterReadingCore.Repositories;
using EnsekMeterReadingInfra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnsekMeterReadingInfra;

public static class InfraServiceCollectionExtension
{
    public static void AddInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("EnsekDb");
        services.AddDbContext<EnsekDbContext>(options => options.UseSqlite(connectionString));  
        services.AddScoped<IAccountRepository, EfAccountRepository>();
        services.AddScoped<IMeterReadingRepository, EfMeterReadingRepository>();
    }

    public static void DataSeeding(EnsekDbContext ensekDbContext, string dataSeedPath)
    {
        ensekDbContext.Database.EnsureCreated();

        using var fileStream = new FileStream(dataSeedPath, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(fileStream);
        var lineText = string.Empty;

        var index = 0;
        while((lineText = reader.ReadLine()) != null)
        {
            try {
                if (index == 0) {
                    continue;
                }

                var columnCells = lineText.Split(",");
                var account = new AccountEntity(){
                    Id = Convert.ToInt32(columnCells[0]),
                    FirstName = columnCells[1],
                    LastName = columnCells[2]
                };

                ensekDbContext.Accounts.AddAsync(account);
            }
            finally{
                index++;
            }
        }

        ensekDbContext.SaveChanges();
    }
}

