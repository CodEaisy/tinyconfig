using TinyConfig.Abstractions;

namespace TinyConfig.SimpleDB.Core
{
    /// <summary>
    /// simple db config options
    /// </summary>
    public class SimpleDbConfigOptions : ITinyConfigOptions
    {
        /// <summary>
        /// connection string
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// table name for storing settings
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// encryption key for sensitive credentials
        /// </summary>
        public string EncryptionKey { get; set; }

        /// <summary>
        /// indicate whether to reload on change
        /// </summary>
        public bool ReloadOnChange { get; set; }

        /// <summary>
        /// wait time before retrying reloading for changes
        /// </summary>
        public int ReloadDelay { get; set; }
    }
}
