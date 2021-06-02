
namespace TinyConfig.Abstractions
{
    /// <summary>
    /// settings model
    /// </summary>
    public interface ITinySetting
    {
        /// <summary>
        /// settings id (key)
        /// </summary>
        string Id { get; }

        /// <summary>
        /// settings value
        /// </summary>
        string Value { get; }

        /// <summary>
        /// settings version
        /// </summary>
        byte[] Version { get; }
    }
}
