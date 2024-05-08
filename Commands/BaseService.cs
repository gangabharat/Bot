using Bot.AppSettings;
using Microsoft.Extensions.Configuration;

namespace Bot.Commands
{
    public abstract class BaseService
    {
        public BaseService(IConfiguration configuration)
        {
            Configuration = configuration;
            Options = Configuration.GetRequiredSection(nameof(ApplicationOptions)).Get<ApplicationOptions>();
        }

        public IConfiguration Configuration { get; }
        public DateTime Now => DateTime.UtcNow;

        protected readonly ApplicationOptions? Options;
    }
}
