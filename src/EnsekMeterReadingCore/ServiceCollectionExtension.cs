using EnsekMeterReadingCore.Actions;
using EnsekMeterReadingCore.Actions.Implementations;
using EnsekMeterReadingCore.Helpers;
using EnsekMeterReadingCore.Helpers.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace EnsekMeterReadingCore;

public static class ServiceCollectionExtension
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IMeterReadingUploadCsvParser, MeterReadingUploadCsvParser>();
        services.AddScoped<IMeterReadingUploadsAction, MeterReadingUploadsAction>();
    }
}
