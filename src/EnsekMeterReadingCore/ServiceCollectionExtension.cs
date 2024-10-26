using EnsekMeterReadingCore.Actions;
using EnsekMeterReadingCore.Helpers;
using EnsekMeterReadingCore.Helpers.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace EnsekMeterReadingCore;

public static class ServiceCollectionExtension
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IMeterReadingUploadCsvParser, MeterReadingUploadCsvParser>();
        services.AddScoped<IMeterReadingUploadsAction, MeterReadingUploadsAction>();
    }
}
