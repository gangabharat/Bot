using Spectre.Console.Cli;
using System.ComponentModel;

namespace Bot.Commands.Settings
{
    public class BaseSettings : CommandSettings
    {
        //[Description("The number of weather forecasts to display.")]
        //[CommandArgument(0, "[count]")]
        //public int Count { get; set; }

        [Description("File Search Pattern.")]
        [CommandOption("-s|--search")]
        public string? FileSearchPattern { get; set; }

        [Description("Application Output Path.")]
        [CommandOption("-i|--input")]
        public string? ApplicationInputPath { get; set; }

        [Description("Application Output Path.")]
        [CommandOption("-o|--output")]
        public string? ApplicationOutputPath { get; set; }

    }
}
