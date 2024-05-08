using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace Bot.Commands.Download
{
    public class DownloadCommand : Command<DownloadSettings>
    {
        private readonly DownloadService _downloadService;
        public DownloadCommand(DownloadService downloadService)
        {
            _downloadService = downloadService;
        }
        public override int Execute([NotNull] CommandContext context, [NotNull] DownloadSettings settings)
        {
            var files = Directory.GetFiles(settings.ApplicationInputPath!, settings.FileSearchPattern ?? "*", SearchOption.AllDirectories);

            //_downloadService.Download(File.ReadAllLines(files[0]).ToList(), 2, settings.ApplicationInputPath!).Wait();

            var lstTasks = files.Select(file =>
            {
                var lstUrls = File.ReadAllLines(file).ToList();
                return _downloadService.Download(lstUrls, settings.BatchSize, settings.ApplicationOutputPath!);
                //return 1;
                //return Task.CompletedTask;
            });

            Task.WhenAll(lstTasks);

            return 0;
        }
    }
}
