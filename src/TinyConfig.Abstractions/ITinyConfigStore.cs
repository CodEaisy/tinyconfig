using System.Collections.Generic;
using System.Threading.Tasks;

namespace TinyConfig.Abstractions
{
    /// <summary>
    /// wraps a configuration store, providing access to the actual configuration values
    /// </summary>
    public interface ITinyConfigStore
    {
        /// <summary>
        /// check if a change has occurred on the configuration store
        /// </summary>
        /// <param name="versionToken"></param>
        /// <returns>true is settings has changed</returns>
        Task<bool> HasChanged(object versionToken = null);

        /// <summary>
        /// get all settings from configuration store
        /// </summary>
        /// <returns>a list containing all configuration key value pairs, with table version token</returns>
        Task<(IEnumerable<ITinySetting>, object)> GetAllWithVersionToken();
    }
}
