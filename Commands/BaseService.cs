using Bot.AppSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bot.Commands
{
    public abstract class BaseService<T> where T : class
    {
        public BaseService(IConfiguration configuration, ILogger<T> logger)
        {
            Configuration = configuration;
            Logger = logger;
            Options = Configuration.GetRequiredSection(nameof(ApplicationOptions)).Get<ApplicationOptions>();
        }

        public IConfiguration Configuration { get; }
        public ILogger<T> Logger { get; }
        public DateTime Now => DateTime.UtcNow;
        public ApplicationOptions? Options { get; }
    }
}
