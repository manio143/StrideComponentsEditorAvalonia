using Stride.Core;

namespace Stride.Editor.Design
{
    /// <summary>
    /// Plugin entry point.
    /// </summary>
    public interface IEditorPlugin
    {
        /// <summary>
        /// Register the plugin by adding custom services, registering views, etc.
        /// </summary>
        /// <param name="services">Global service registry</param>
        void RegisterPlugin(IServiceRegistry services);
    }
}
