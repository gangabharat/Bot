using Bot.Commands.Settings;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace Bot.Commands.Encrypt
{
    public class EncryptSettings : ApplicationSettings
    {
        [Description("Batch size Pattern.")]
        [CommandOption("-k|--key")]
        public string? SecretKey { get; set; }
    }
}
