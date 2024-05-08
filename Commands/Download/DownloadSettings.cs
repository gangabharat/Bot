using Bot.Commands.Settings;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace Bot.Commands.Download
{
    public class DownloadSettings : ApplicationSettings
    {
        //[Description("BatchSize file(s).")]
        //[CommandArgument(0, "[BatchSize]")]
        //public int BatchSize { get; set; }

        [Description("Batch size Pattern.")]
        [CommandOption("-b|--batch")]
        public int BatchSize { get; set; }
    }
}
