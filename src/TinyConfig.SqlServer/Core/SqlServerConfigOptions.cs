using System.Diagnostics.CodeAnalysis;
using TinyConfig.Abstractions;

namespace TinyConfig.SqlServer.Core
{
    /// <summary>
    /// simple db config options
    /// </summary>
    public class SqlServerConfigOptions : ITinyConfigOptions
    {
        /// <summary>
        /// connection string
        /// </summary>
        [NotNull]
        public string ConnectionString { get; }

        /// <summary>
        /// table name for storing settings
        /// </summary>
        [NotNull]
        public string TableName { get; }

        /// <summary>
        /// encryption key for sensitive credentials
        /// </summary>
        [NotNull]
        public string EncryptionKey { get; set; }

        /// <summary>
        /// indicate whether to reload on change
        /// </summary>
        public bool ReloadOnChange { get; set; }

        /// <summary>
        /// wait time before retrying reloading for changes
        /// </summary>
        public int ReloadDelay { get; set; } = 60;
    }
}
