using System.Globalization;
using System.Linq;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


app.MapGet("/api/time/{country}", (string country) =>
{
    // Validate input
    if (string.IsNullOrWhiteSpace(country))
        return Results.BadRequest("Country code is required (ISO 3166-1 alpha-2, e.g. US, AR, GB).");

    country = country.Trim().ToUpperInvariant();

    RegionInfo region;
    try
    {
        region = new RegionInfo(country);
    }
    catch (ArgumentException)
    {
        return Results.BadRequest($"Invalid country code '{country}'. Use ISO 3166-1 alpha-2.");
    }

    // Try to pick a culture for formatting the time string
    var culture = CultureInfo.InvariantCulture;
    try
    {
        var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        var cultureMatch = cultures.FirstOrDefault(c =>
        {
            try
            {
                return new RegionInfo(c.Name).TwoLetterISORegionName.Equals(region.TwoLetterISORegionName, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        });
        if (cultureMatch != null)
            culture = cultureMatch;
    }
    catch
    {
        // ignore and use invariant culture
    }

    var utcNow = DateTime.UtcNow;

    // Attempt to find a TimeZoneInfo for the country by searching available system timezones
    var zones = TimeZoneInfo.GetSystemTimeZones();
    TimeZoneInfo? tz = zones.FirstOrDefault(z =>
        (!string.IsNullOrEmpty(z.Id) && z.Id.IndexOf(region.EnglishName, StringComparison.OrdinalIgnoreCase) >= 0)
        || (!string.IsNullOrEmpty(z.DisplayName) && z.DisplayName.IndexOf(region.EnglishName, StringComparison.OrdinalIgnoreCase) >= 0)
        || (!string.IsNullOrEmpty(z.StandardName) && z.StandardName.IndexOf(region.EnglishName, StringComparison.OrdinalIgnoreCase) >= 0)
    );

    // Small fallback dictionary for common countries (Windows timezone IDs)
    if (tz == null)
    {
        var fallback = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "US", "Eastern Standard Time" },
            { "AR", "Argentina Standard Time" },
            { "BR", "E. South America Standard Time" },
            { "GB", "GMT Standard Time" },
            { "ES", "Romance Standard Time" },
            { "FR", "Romance Standard Time" },
            { "DE", "W. Europe Standard Time" },
            { "IN", "India Standard Time" },
            { "CN", "China Standard Time" },
            { "JP", "Tokyo Standard Time" },
            { "AU", "AUS Eastern Standard Time" },
            { "CA", "Eastern Standard Time" },
            { "MX", "Central Standard Time (Mexico)" }
        };

        if (fallback.TryGetValue(region.TwoLetterISORegionName, out var tzId))
        {
            try
            {
                tz = TimeZoneInfo.FindSystemTimeZoneById(tzId);
            }
            catch
            {
                tz = null;
            }
        }
    }

    if (tz == null)
    {
        return Results.BadRequest($"No timezone mapping found for country '{country}' ({region.EnglishName}). Try a different country code or extend the server mapping.");
    }

    var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, tz);
    return Results.Ok(localTime.ToString("T", culture));
})
   .WithName("GetTimeCountry");



app.MapGet("/api/time", () =>
{
    return DateTime.Now.ToString("HH:mm:ss");
})
   .WithName("GetTime");
   

app.MapGet("/api/timeutc", () =>
{
    return DateTime.UtcNow.ToString("HH:mm:ss");
})
   .WithName("GetTimeUtc");
   

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
