using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TinyConfig.Core.Extensions;
using TinyConfig.SimpleDB.Core;

namespace TinyConfig.SimpleDB.Extensions
{
    /// <summary>
    /// configuration builder extensions
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// add db configuration to list of config
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration">existing configuration value</param>
        /// <param name="logger">logger instance</param>
        /// <returns><see cref="IConfigurationBuilder" /></returns>
        public static IConfigurationBuilder AddSimpleDbConfig(
            this IConfigurationBuilder builder, IConfiguration configuration,
            ILogger logger = null) =>
            builder.AddTinyConfig<SimpleDbConfigOptions, SimpleDbConfigStore>(configuration, logger);
    }
}
