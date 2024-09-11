using EnsekMeterReadingCore.Actions;
using EnsekMeterReadingCore;
using EnsekMeterReadingInfra;
using EnsekMeterReadingCore.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCoreServices();
builder.Services.AddInfraServices(builder.Configuration);

var app = builder.Build();

// Perform database initialization
var isDataSeedingText = builder.Configuration["DataSeeding"];
if (!string.IsNullOrEmpty(isDataSeedingText) 
    && bool.TryParse(isDataSeedingText, out var isDataSeeding)
    && isDataSeeding)
{
    DataSeeding(builder.Configuration);
}


app.MapPost("/meter-reading-uploads", async (IMeterReadingUploadsAction action, HttpContext httpContext) => {
    var form = await httpContext.Request.ReadFormAsync();
    if (form?.Files?.Any() != true)
    {
        return Results.BadRequest("No file uploaded.");
    }

    // TODO Check if there is only one file uploaded or do we need to support multiple files uploaded 
    var file = form.Files.First();

    using var fileStream = file.OpenReadStream();
    var result = await action.RunAsync(fileStream);

    return Results.Ok(result);
});

app.MapGet("/accounts", async (IAccountRepository accountRepository) => {
    var result = await accountRepository.GetAllAsync();
    return Results.Ok(result);
});

app.Run();

void DataSeeding(IConfiguration configuration) {
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<EnsekDbContext>();
        InfraServiceCollectionExtension.DataSeeding(context, configuration["DataSeedingPath"]!);
    }
}