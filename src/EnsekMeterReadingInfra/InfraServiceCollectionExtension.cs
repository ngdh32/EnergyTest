using System;
using EnsekMeterReadingCore.Repositories;
using EnsekMeterReadingInfra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnsekMeterReadingInfra;

public static class InfraServiceCollectionExtension
{
    public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("EnsekDb");
        services.AddDbContext<EnsekDbContext>(options => options.UseSqlite(connectionString));  
        services.AddScoped<IAccountRepository, EfAccountRepository>();
        services.AddScoped<IMeterReadingRepository, EfMeterReadingRepository>();

        return services;
    }
}

