using Bot.AppSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace Bot.Commands.Encrypt
{
    public class EncryptCommand : Command<EncryptSettings>
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<EncryptCommand> _logger;
        private readonly EncryptService _encryptService;
        private readonly ApplicationOptions _options;
        public EncryptCommand(IConfiguration configuration, ILogger<EncryptCommand> logger, EncryptService encryptService)
        {
            _configuration = configuration;
            _logger = logger;
            _encryptService = encryptService;
            _options = _configuration.GetRequiredSection(nameof(ApplicationOptions)).Get<ApplicationOptions>()!;
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] EncryptSettings settings)
        {
            var filesPah = settings.ApplicationInputPath ?? _options.GetApplicationDataPath();
            _logger.LogInformation("Reading Path {0}", filesPah);

            var files = Directory.GetFiles(filesPah!, settings.FileSearchPattern ?? "*", SearchOption.AllDirectories);

            _logger.LogTrace("file(s) found {0}", files.Count());

            var outputPath = settings.ApplicationOutputPath ?? _options.GetApplicationDataPath();

            var lstTasks = files.Select(file =>
            {
                var encryptFileName = $"{Path.Combine(outputPath, Path.GetFileName(file))}";
                return _encryptService.Encrypt(file, encryptFileName, settings.SecretKey);
            });

            Task.WhenAll(lstTasks);

            return 0;
        }
    }
}
