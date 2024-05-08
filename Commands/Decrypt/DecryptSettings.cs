using Bot.Commands.Settings;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace Bot.Commands.Decrypt
{
    public class DecryptSettings : ApplicationSettings
    {
        [Description("Batch size Pattern.")]
        [CommandOption("-k|--key")]
        public string? SecretKey { get; set; }
    }
}
