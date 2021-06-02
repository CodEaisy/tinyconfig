using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyConfig.Core.Extensions;
using TinyConfig.SqlServer.Core;

namespace TinyConfig.SqlServer.Extensions
{
    /// <summary>
    /// service collection extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// add database configuration reloader
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddSqlServerConfigReloader(this IServiceCollection services,
            IConfiguration configuration) =>
                services.AddTinyConfigReloader<SqlServerConfigOptions, SqlServerConfigStore>(configuration);
    }
}
