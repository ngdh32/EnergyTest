using EnsekMeterReadingInfra;

namespace EnsekMeterReadingApi;

public class DataSeeding
{
    private readonly IConfiguration _configuration;
    
    public DataSeeding(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Seed(WebApplication app)
    {
        var isDataSeedingText = _configuration["DataSeeding"];
        if (string.IsNullOrEmpty(isDataSeedingText)
            || (bool.TryParse(isDataSeedingText, out var isDataSeeding) && !isDataSeeding))
        {
            return;
        }
    
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EnsekDbContext>();
        InfraServiceCollectionExtension.DataSeeding(context, _configuration["DataSeedingPath"]!);
    }
}