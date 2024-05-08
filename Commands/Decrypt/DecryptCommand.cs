using Bot.AppSettings;
using Bot.Commands.Encrypt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace Bot.Commands.Decrypt
{
    public class DecryptCommand : Command<DecryptSettings>
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<DecryptCommand> _logger;
        private readonly DecryptService _decryptService;
        private readonly ApplicationOptions _options;
        public DecryptCommand(IConfiguration configuration, ILogger<DecryptCommand> logger, DecryptService encryptService)
        {
            _configuration = configuration;
            _logger = logger;
            _decryptService = encryptService;
            _options = _configuration.GetRequiredSection(nameof(ApplicationOptions)).Get<ApplicationOptions>()!;
            _options.ApplicationOutputPath = _options.GetApplicationDataPath();
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] DecryptSettings settings)
        {
            //var files = Directory.GetFiles(_options.GetApplicationDataPath(), settings.FileSearchPattern ?? "*", SearchOption.AllDirectories);

            //_logger.LogInformation("File(s) found {0}", files.Count());

            //var lstTasks = files.Select(file =>
            //{
            //    return _decryptService.Decrypt(file, settings.SecretKey);
            //});

            //Task.WhenAll(lstTasks);


            var filesPah = settings.ApplicationInputPath ?? _options.ApplicationOutputPath;
            _logger.LogInformation("Reading Path {0}", filesPah);

            var files = Directory.GetFiles(filesPah!, settings.FileSearchPattern ?? "*", SearchOption.AllDirectories);

            _logger.LogTrace("file(s) found {0}", files.Count());

            var outputPath = settings.ApplicationOutputPath ?? _options.ApplicationOutputPath;

            var lstTasks = files.Select(file =>
            {
                var encryptFileName = $"{Path.Combine(outputPath!, Path.GetFileNameWithoutExtension(file))}";
                var content = _decryptService.Decrypt(file, settings.SecretKey);
                File.WriteAllText(encryptFileName, content.Result);
                _logger.LogTrace("decrypt {0}", encryptFileName);
                return Task.FromResult(encryptFileName);
            });

            Task.WhenAll(lstTasks);

            return 0;
        }
    }
}
