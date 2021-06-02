using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TinyConfig.Abstractions;

namespace TinyConfig.Core
{
    /// <summary>
    /// configuration source builder
    /// </summary>
    /// <typeparam name="TOptions">concrete implementation of configuration options</typeparam>
    /// <typeparam name="TStore">concrete implementation of configuration store</typeparam>
    public class TinyConfigSource<TOptions, TStore> : IConfigurationSource
        where TOptions : class, ITinyConfigOptions
        where TStore : class, ITinyConfigStore
    {
        /// <summary>
        /// configuration options
        /// </summary>
        public TOptions Options;
        private readonly ILogger _logger;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public TinyConfigSource(TOptions options, ILogger logger = null)
        {
            Options = options;
            _logger = logger;
        }

        /// <summary>
        /// build configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <returns><see cref="IConfigurationProvider"/></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            var constructor = typeof(TStore).GetConstructor(new Type[] { typeof(TOptions) });
            ITinyConfigStore store = constructor.Invoke(new object[] { Options }) as ITinyConfigStore;
            return new TinyConfigProvider<TOptions, TStore>(store, this, _logger);
        }
    }
}
