using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TinyConfig.Abstractions;

namespace TinyConfig.Core
{
    /// <summary>
    /// tiny configuration provider
    /// </summary>
    /// <typeparam name="TOptions">concrete implementation of configuration option</typeparam>
    /// <typeparam name="TStore">concrete implementation of configuration store</typeparam>
    public class TinyConfigProvider<TOptions, TStore> : ConfigurationProvider
        where TOptions : class, ITinyConfigOptions
        where TStore : class, ITinyConfigStore
    {
        /// <summary>
        /// configuration source
        /// </summary>
        public readonly TinyConfigSource<TOptions, TStore> ConfigurationSource;
        private readonly ITinyConfigStore _store;
        private readonly ILogger _logger;
        private readonly Dictionary<string, byte[]> _versionsCache = new Dictionary<string, byte[]>();

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="store"></param>
        /// <param name="source"></param>
        /// <param name="logger"></param>
        public TinyConfigProvider(ITinyConfigStore store,
            TinyConfigSource<TOptions, TStore> source,
            ILogger logger = null)
        {
            ConfigurationSource = source;
            _store = store;
            _logger = logger;
        }

        /// <summary>
        /// load configuration from database
        /// </summary>
        public override void Load()
        {
            var anyCheck = _store.HasAny();
            anyCheck.Wait();
            if (anyCheck.Result)
            {
                _logger?.LogDebug("loading configuration from store: {0}", typeof(TStore));
                var reload = LoadDatabaseConfigs();
                _logger?.LogDebug("loaded configuration from store: {0}", typeof(TStore));
                if (reload) OnReload();
            }
        }

        /// <summary>
        /// reload config data
        /// </summary>
        public bool LoadDatabaseConfigs()
        {
            var reload = false;
            var allConfigs = _store.GetAll();
            allConfigs.Wait();

            foreach (var config in allConfigs.Result)
            {
                if (!_versionsCache.ContainsKey(config.Id))
                {
                    Set(config.Id, config.Value);
                    _versionsCache[config.Id] = config.Version;
                    reload = true;
                } else
                {
                    if (_versionsCache.TryGetValue(config.Id, out byte[] lastVersion)
                        && !lastVersion.SequenceEqual(config.Version))
                    {
                        Set(config.Id, config.Value);
                        _versionsCache[config.Id] = config.Version;
                        reload = true;
                    }
                }
            }

            return reload;
        }
    }
}
