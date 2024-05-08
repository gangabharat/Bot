using Bot.AppSettings;
using Bot.Services;
using Microsoft.Extensions.Configuration;

namespace Bot.Commands.Encrypt
{
    public class DecryptService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileSecureService _fileSecure;
        private readonly ApplicationOptions _options;
        public DecryptService(IConfiguration configuration, IFileSecureService fileSecure)
        {
            _configuration = configuration;
            _fileSecure = fileSecure;
            _options = _configuration.GetRequiredSection(nameof(ApplicationOptions)).Get<ApplicationOptions>()!;

            if (_options.Encrypt is not null)
            {
                _fileSecure.Init(_options.Encrypt.SecretKey, _options.GetApplicationTempPath());
            }
        }

        public Task<string> Decrypt(string sourceFileName, string? secretKey = null)
        {
            if (secretKey is not null)
            {
                _fileSecure.SecretKey = secretKey;
            }

            var result = _fileSecure.Decrypt(sourceFileName);

            return Task.FromResult(result);
        }
    }
}
