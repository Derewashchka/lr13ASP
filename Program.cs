using lr13.Services;
using Serilog;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Async(a => a.File("logs/log-.txt", rollingInterval: RollingInterval.Day))
    .Enrich.WithExceptionDetails()
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(dispose: true);

builder.Services.AddSingleton<UserService>();

var app = builder.Build();

try
{
    var userService = app.Services.GetRequiredService<UserService>();

    userService.CreateUser("John Doe", "john@example.com");

    try
    {
        userService.CreateUser("", "invalid@example.com");
    }
    catch
    {
    }

    userService.SimulateError();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}

app.Run();