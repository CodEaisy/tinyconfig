using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyConfig.Abstractions;

namespace TinyConfig.Core.Extensions
{
    /// <summary>
    /// service collection extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// add tiny configuration reloader
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <typeparam name="TOptions">concrete implementation for config</typeparam>
        /// <typeparam name="TStore">concrete implementation for store</typeparam>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddTinyConfigReloader<TOptions, TStore>(this IServiceCollection services,
            IConfiguration configuration)
            where TOptions : class, ITinyConfigOptions, new()
            where TStore : class, ITinyConfigStore =>
            services.AddSingleton((IConfigurationRoot) configuration)
                .AddHostedService<TinyConfigChangeWatcher<TOptions, TStore>>();
    }
}
