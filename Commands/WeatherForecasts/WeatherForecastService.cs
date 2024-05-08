using Bot.Commands;
using Bot.Commands.WeatherForecasts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bot.WeatherForecasts;

public class WeatherForecastService : BaseService<WeatherForecastService>
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherForecastService(IConfiguration configuration, ILogger<WeatherForecastService> logger) 
        : base(configuration, logger)
    {
       
    }

    public IEnumerable<WeatherForecast> GetForecasts(int count)
    {

        //Options.
        var config = Configuration;

        Logger.LogTrace("user gmail {0} password {1}", Options?.Smtp?.UserName, Options?.Smtp?.Password);

        Logger.LogInformation("Getting {count} forecasts.", count);
        var rng = new Random();

        var forecasts = Enumerable.Range(1, count).Select(index =>
            new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });

        return forecasts;
    }
}