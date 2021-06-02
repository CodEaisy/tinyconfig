using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TinyConfig.Abstractions;

namespace TinyConfig.Core.Extensions
{
    /// <summary>
    /// configuration builder extensions
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        private const string ENABLED_FLAG = "TinyConfig:Enabled";
        private const string CONFIG_SECTION = "TinyConfig";

        /// <summary>
        /// add db configuration to list of config
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration">existing configuration value</param>
        /// <param name="logger">logger instance</param>
        /// <returns><see cref="IConfigurationBuilder" /></returns>
        public static IConfigurationBuilder AddTinyConfig<TOptions, TStore>(
            this IConfigurationBuilder builder, IConfiguration configuration,
            ILogger logger = null)
            where TOptions : class, ITinyConfigOptions, new()
            where TStore : class, ITinyConfigStore
        {
            if (configuration.GetValue<bool>(ENABLED_FLAG))
            {
                var config = new TOptions();
                configuration.Bind(CONFIG_SECTION, config);
                return builder.Add(new TinyConfigSource<TOptions, TStore>(config, logger));
            }

            return builder;
        }
    }
}
