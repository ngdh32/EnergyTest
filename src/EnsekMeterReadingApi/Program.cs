using EnsekMeterReadingApi;
using EnsekMeterReadingCore.Actions;
using EnsekMeterReadingCore;
using EnsekMeterReadingInfra;
using EnsekMeterReadingCore.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCoreServices();
builder.Services.AddInfraServices(builder.Configuration);

var app = builder.Build();
new DataSeeding(builder.Configuration).Seed(app);

app.MapPost("/meter-reading-uploads", async (IMeterReadingUploadsAction action, HttpContext httpContext) => {
    if (httpContext.Request.HasFormContentType)
    {
        return Results.BadRequest("Not Form Request");
    }
    
    var form = await httpContext.Request.ReadFormAsync();
    if (form.Files.Any() != true)
    {
        return Results.BadRequest("No file uploaded.");
    }

    var file = form.Files.First();

    await using var fileStream = file.OpenReadStream();
    var result = await action.RunAsync(fileStream);

    return Results.Ok(result);
});

app.MapGet("/accounts", async (IAccountRepository accountRepository) => {
    var result = await accountRepository.GetAllAsync();
    return Results.Ok(result);
});

app.Run();