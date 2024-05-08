using Spectre.Console.Cli;
using System.ComponentModel;

namespace Bot.Commands.Html2Pdf
{
    public class Html2PdfSettings : CommandSettings
    {
        [Description("Source Path for html file(s).")]
        [CommandArgument(0, "[htmlSourcePath]")]
        public string? HtmlSourcePath { get; set; }

        [Description("Destination Path for pdf file(s).")]
        [CommandArgument(1, "[PdfDestinationPath]")]
        public string? PdfDestinationPath { get; set; }

        //[Description("The unit of temperature.")]
        //[CommandOption("-u|--unit")]
        //public TemperatureUnit? Unit { get; set; }
    }
}
