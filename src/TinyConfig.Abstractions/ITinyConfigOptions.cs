
namespace TinyConfig.Abstractions
{
    /// <summary>
    /// tiny config options
    /// </summary>
    public interface ITinyConfigOptions
    {
        /// <summary>
        /// indicate whether tiny config is enabled, false by default
        /// </summary>
        bool Enabled { get => false; }

        /// <summary>
        /// indicate whether configuration should be reloaded when a change occurs
        /// Has default value "false"
        /// </summary>
        bool ReloadOnChange { get => false; }

        /// <summary>
        /// number of seconds to wait before reloading after a change
        /// Has default value "60" seconds
        /// </summary>
        int ReloadDelay { get => 60; }
    }
}
