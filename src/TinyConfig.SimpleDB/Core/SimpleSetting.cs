using TinyConfig.Abstractions;

namespace TinyConfig.SimpleDB.Core
{
    /// <summary>
    /// simple setting model
    /// </summary>
    public class SimpleSetting : ITinySetting
    {
        /// <inheritdoc />
        public string Id  { get; set; }

        /// <inheritdoc />
        public string Value { get; set; }

        /// <inheritdoc />
        public byte[] Version { get; set; }

        /// <summary>
        /// indicate if field is a secret
        /// </summary>
        public bool IsSecret { get; set; }
    }
}
