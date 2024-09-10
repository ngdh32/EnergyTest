using EnsekMeterReadingApi.Actions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IMeterReadingUploadsAction, MeterReadingUploadsAction>();

var app = builder.Build();

app.MapPost("/meter-reading-uploads", async (IMeterReadingUploadsAction action, HttpContext httpContext) => {
    var form = await httpContext.Request.ReadFormAsync();
    if (form?.Files?.Any() != true)
    {
        return Results.BadRequest("No file uploaded.");
    }

    // TODO Check if there is only one file uploaded or do we need to support multiple files uploaded 
    var file = form.Files.First();

    using var fileStream = file.OpenReadStream();
    await action.RunAsync(fileStream);

    return Results.Ok();
});

app.Run();
