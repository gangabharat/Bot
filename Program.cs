using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bot.Common;
using Spectre.Console;
using Spectre.Console.Cli;
using Polly.Extensions.Http;
using Polly;
using Bot.Services;
using Bot.Commands.Download;
using Bot.Commands.Encrypt;
using Bot.Commands.Decrypt;
using Serilog;
using Bot.Commands.Html2Pdf;
using Bot.Commands.WeatherForecasts;
using Bot.WeatherForecasts;
using Microsoft.Extensions.Configuration;

AnsiConsole.Write(new FigletText("Bot"));
AnsiConsole.WriteLine("Bot Command-line Tools 1.1.0");
AnsiConsole.WriteLine();

var builder = Host.CreateDefaultBuilder(args);

//Read from UserSecrets 
builder.ConfigureAppConfiguration(app =>
{
    app.AddUserSecrets<Program>();
});


//UseSerilog configuratrion & read from appsettings
builder.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

#region Dependancy Services
// Add services to the container
builder.ConfigureServices(services =>
{

    //HttpClientFactory Retry Logic for the httpclient name bot this example
    services.AddHttpClient("bot").
    AddPolicyHandler(GetRetryPolicy());

    services.AddSingleton<WeatherForecastService>();
    services.AddSingleton<Html2PdfService>();

    //services.AddSingleton<IBaseService<>, BaseService<>>();
    //services.AddSingleton(typeof(IBaseService<>), typeof(BaseService<>));

    services.AddSingleton<IHttpClientService, HttpClientService>();
    services.AddSingleton<ITwilioService, TwilioService>();
    services.AddSingleton<IEmailService, EmailService>();
    services.AddSingleton<IFileSecureService, FileSecureService>();

    services.AddSingleton<EncryptService>();
    services.AddSingleton<DecryptService>();
    services.AddSingleton<DownloadService>();
});
#endregion

var registrar = new TypeRegistrar(builder);

var app = new CommandApp(registrar);

app.Configure(config =>
{
    // Register available commands
    config.AddCommand<WeatherForecastCommand>("forecasts")
    .WithDescription("Display local weather forecasts.")
    .WithExample(new[] { "forecasts", "5" });

    config.AddCommand<Html2PdfCommand>("html2pdf")
    .WithDescription("Convert source path file(s) to pdf in destination path.")
    .WithExample(new[] { "html2pdf", "sourcePath", "destinationPath" });

    config.AddCommand<DownloadCommand>("download")
     .WithDescription("Convert source path file(s) to pdf in destination path.")
     .WithExample(new[] { "download", "sourcePath", "destinationPath" });

    config.AddCommand<EncryptCommand>("encrypt")
    .WithDescription("Convert source path file(s) to pdf in destination path.")
    .WithExample(new[] { "encrypt", "sourcePath", "destinationPath" });

    config.AddCommand<DecryptCommand>("decrypt")
    .WithDescription("Convert source path file(s) to pdf in destination path.")
    .WithExample(new[] { "decrypt", "sourcePath", "destinationPath" });

});

return app.Run(args);

//retry Policy for httpclientFactory
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(response => response.StatusCode == System.Net.HttpStatusCode.NotFound) // Example additional condition
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(3, retryAttempt)));
}