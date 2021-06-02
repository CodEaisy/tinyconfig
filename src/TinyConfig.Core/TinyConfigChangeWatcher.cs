using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TinyConfig.Abstractions;

namespace TinyConfig.Core
{
    /// <summary>
    /// Background service to notify about configuration data changes.
    /// </summary>
    public class TinyConfigChangeWatcher<TOptions, TStore> : BackgroundService
        where TOptions : class, ITinyConfigOptions
        where TStore : class, ITinyConfigStore
    {
        private readonly ILogger<TinyConfigChangeWatcher<TOptions, TStore>> _logger;
        private readonly IEnumerable<TinyConfigProvider<TOptions, TStore>> _configProviders;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConfigChangeWatcher"/> class.
        /// test.
        /// </summary>
        /// <param name="configurationRoot" />
        /// <param name="logger" />
        public TinyConfigChangeWatcher(IConfigurationRoot configurationRoot, ILogger<TinyConfigChangeWatcher<TOptions, TStore>> logger)
        {
            if (configurationRoot == null)
            {
                throw new ArgumentNullException(nameof(configurationRoot));
            }

            _logger = logger;
            _configProviders = configurationRoot.Providers.OfType<TinyConfigProvider<TOptions, TStore>>().Where(p => p.ConfigurationSource.Options.ReloadOnChange).ToList() !;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           var timers = new Dictionary<int, int>(); // key - index of config provider, value - timer
            var minTime = 6000;
            var i = 0;
            foreach (var provider in _configProviders)
            {
                var waitForSec = provider.ConfigurationSource.Options.ReloadDelay;
                minTime = Math.Min(minTime, waitForSec);
                timers[i] = waitForSec;
                i++;
            }

            _logger.LogInformation($"DbConfigChangeWatcher will use {minTime} seconds interval");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(minTime), stoppingToken).ConfigureAwait(false);
                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                for (var j = 0; j < _configProviders.Count(); j++)
                {
                    var timer = timers[j];
                    timer -= minTime;
                    if (timer <= 0)
                    {
                        _configProviders.ElementAt(j).Load();
                        timers[j] = _configProviders.ElementAt(j).ConfigurationSource.Options.ReloadDelay;
                    }
                    else
                    {
                        timers[j] = timer;
                    }
                }
            }
        }
    }
}
