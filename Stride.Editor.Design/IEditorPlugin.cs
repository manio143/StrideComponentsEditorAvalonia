using Stride.Core;
using Stride.Core.Reflection;

namespace Stride.Editor.Design
{
    /// <summary>
    /// Plugin entry point.
    /// </summary>
    [AssemblyScan]
    public interface IEditorPlugin
    {
        /// <summary>
        /// Register the plugin by adding custom services, registering views, etc.
        /// </summary>
        /// <param name="services">Global service registry</param>
        void RegisterPlugin(IServiceRegistry services);

        /// <summary>
        /// Unregister custom services, views, etc.
        /// </summary>
        /// <param name="services">Global service registry</param>
        /// <remarks>Plugins may be reloaded by being unregistered, their assembly reloaded and registered again.</remarks>
        void UnregisterPlugin(IServiceRegistry services);
    }
}
