using System.Diagnostics.CodeAnalysis;
using Bot.WeatherForecasts;
using Spectre.Console;
using Spectre.Console.Cli;
using Microsoft.Extensions.Logging;

namespace Bot.Commands.WeatherForecasts;

public class WeatherForecastCommand : Command<WeatherForecastSettings>
{
    private readonly ILogger<WeatherForecastCommand> _logger;
    private readonly WeatherForecastService _weatherForecastService;
    public WeatherForecastCommand(ILogger<WeatherForecastCommand> logger, WeatherForecastService weatherForecastService)
    {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] WeatherForecastSettings settings)
    {
        var unit = settings.Unit;

        var forecasts = _weatherForecastService.GetForecasts(settings.Count);

        var table = new Table();

        table.AddColumn("Date");
        table.AddColumn("Temp");
        table.AddColumn("Summary");

        foreach (var forecast in forecasts)
        {
            var temperature = unit == TemperatureUnit.Fahrenheit
                ? forecast.TemperatureF
                : forecast.TemperatureC;

            table.AddRow(
                forecast.Date.ToShortDateString(),
                temperature.ToString(),
                forecast.Summary);
        }

        table.Expand();

        AnsiConsole.Write(table);
        return 0;
    }
}