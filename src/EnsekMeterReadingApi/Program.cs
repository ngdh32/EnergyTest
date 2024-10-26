using EnsekMeterReadingApi;
using EnsekMeterReadingCore.Actions;
using EnsekMeterReadingCore;
using EnsekMeterReadingInfra;
using EnsekMeterReadingCore.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfraServices(builder.Configuration);
builder.Services.AddCoreServices();

// Definitely need to change it in prod
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("AllowAllOrigins");

new DataSeeding(builder.Configuration).Seed(app);

app.MapPost("/meter-reading-uploads", async (IMeterReadingUploadsAction action, HttpContext httpContext) => {
    if (!httpContext.Request.HasFormContentType)
    {
        return Results.BadRequest("Not Form Request");
    }
    
    var form = await httpContext.Request.ReadFormAsync();
    if (form.Files.Any() != true)
    {
        return Results.BadRequest("No file uploaded.");
    }

    var file = form.Files.First();
    if (file.ContentType != "text/csv" || Path.GetExtension(file.FileName).ToLower() != ".csv")
    {
        return Results.BadRequest("File is not a .csv file.");
    }

    await using var fileStream = file.OpenReadStream();
    var result = await action.RunAsync(fileStream);

    return Results.Ok(result);
});

app.MapGet("/accounts", async (IAccountRepository accountRepository) => {
    var result = await accountRepository.GetAllAsync();
    return Results.Ok(result);
});

app.MapGet("/meters", async (IMeterReadingRepository meterReadingRepository) => {
    var result = await meterReadingRepository.GetAllAsync();
    return Results.Ok(result);
});

app.Run();