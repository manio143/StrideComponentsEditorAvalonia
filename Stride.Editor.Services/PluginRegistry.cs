using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Editor.Design;
using Stride.Editor.Design.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stride.Editor.Services
{
    /// <summary>
    /// Manages adding/removing plugins from the system.
    /// </summary>
    public class PluginRegistry
    {
        public PluginRegistry(IServiceRegistry services)
        {
            Services = services;
        }

        private static LoggingScope Logger = LoggingScope.Global(nameof(PluginRegistry));

        private IServiceRegistry Services { get; }

        private readonly List<IEditorPlugin> availablePlugins = new List<IEditorPlugin>();

        private readonly Dictionary<Type, IEditorPlugin> activePlugins = new Dictionary<Type, IEditorPlugin>();

        public IEnumerable<IEditorPlugin> AvailablePlugins => availablePlugins;

        /// <summary>
        /// Registers a plugin, calling <see cref="IEditorPlugin.RegisterPlugin(IServiceRegistry)"/>.
        /// </summary>
        /// <param name="plugin"></param>
        public void Register(IEditorPlugin plugin)
        {
            if (activePlugins.ContainsKey(plugin.GetType()))
                throw new InvalidOperationException($"Plugin of type '{plugin.GetType()}' is already registered.");

            Logger.Debug($"Registering plugin '{plugin.GetType().FullName}'...");
            
            plugin.RegisterPlugin(Services);
            activePlugins.Add(plugin.GetType(), plugin);
            
            Logger.Info($"Registered plugin '{plugin.GetType().FullName}'.");
        }

        public void Unregister(IEditorPlugin plugin)
        {
            if (!activePlugins.ContainsKey(plugin.GetType()))
                throw new InvalidOperationException($"Plugin of type '{plugin.GetType()}' has not been registered.");
            
            Logger.Debug($"Unregistering plugin '{plugin.GetType().FullName}'...");

            var rplugin = activePlugins[plugin.GetType()];
            
            if (plugin != rplugin)
                Logger.Warning($"A different instance of plugin '{plugin.GetType()}' has been registered and unregistered. Make sure the plugin has no state.");
            
            plugin.UnregisterPlugin(Services);
            activePlugins.Remove(plugin.GetType());

            Logger.Info($"Unregistered plugin '{plugin.GetType().FullName}'.");
        }

        /// <summary>
        /// Searches through the assemblies loaded in the current AppDomain
        /// looking for implementations of <see cref="IEditorPlugin"/> with empty constructor.
        /// Populates <see cref="AvailablePlugins"/>.
        /// </summary>
        public void RefreshAvailablePlugins()
        {
            Logger.Debug($"Refreshing available plugins...");
            
            availablePlugins.Clear();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var plugins = assembly.GetTypes().Where(t => typeof(IEditorPlugin).IsAssignableFrom(t) && t.GetConstructor(new Type[0]) != null);
                foreach (var plugin in plugins)
                {
                    Logger.Debug($"Found plugin {plugin.FullName}");
                    availablePlugins.Add((IEditorPlugin)Activator.CreateInstance(plugin));
                }
            }
        }
    }
}
