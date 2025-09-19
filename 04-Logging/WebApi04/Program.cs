using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddLogging(builder => builder.AddConsole());


builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.File("log-app04-.log",
                   rollingInterval: RollingInterval.Day)
        .WriteTo.Console()); 

//var log = new LoggerConfiguration()
//    .WriteTo.File("log-app04-.log", 
//                    rollingInterval: RollingInterval.Day)
//    .WriteTo.Console()
//    .CreateLogger();





var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (ILogger<Program> logger) =>
{

    logger.LogInformation("Paso 1) Ingreso al weatherforecast");

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    logger.LogInformation("Paso 2) se cargo el clima");

    logger.LogError("No se que paso pero hubo un error");

    return forecast;
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
