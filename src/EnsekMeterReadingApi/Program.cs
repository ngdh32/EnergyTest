using EnsekMeterReadingApi.Actions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IMeterReadingUploadsAction, MeterReadingUploadsAction>();

var app = builder.Build();

app.MapPost("/meter-reading-uploads", async (IMeterReadingUploadsAction action, HttpContext httpContext) => {
    await action.RunAsync();
});

app.Run();
