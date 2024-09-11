using EnsekMeterReadingCore.Actions;
using EnsekMeterReadingCore.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace EnsekMeterReadingCore;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IMeterReadingUploadCsvParser, MeterReadingUploadCsvParser>();
        services.AddScoped<IMeterReadingUploadsAction, MeterReadingUploadsAction>();

        return services;
    }
}
