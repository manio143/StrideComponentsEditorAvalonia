using Microsoft.Build.Locator;
using Stride.Core;
using Stride.Core.Assets;
using Stride.Editor.Design.Core;

namespace Stride.Editor.Services
{
    public partial class Session
    {
        public Session()
        {
            // Register access to MSBuild assembly - required to load project dependencies
            // Must be called before we touch any type from referenced MSBuild assembly
            // either via reflection on runtime's TypeLoader.
            // see https://github.com/microsoft/MSBuildLocator/issues/64
            if (!MSBuildLocator.IsRegistered)
                MSBuildLocator.RegisterDefaults();
        }

        /// <summary>
        /// View model for the whole editor application.
        /// </summary>
        public EditorViewModel EditorViewModel { get; set; }

        /// <summary>
        /// Loaded game project session.
        /// </summary>
        public PackageSession PackageSession { get; set; }

        /// <summary>
        /// Main service registry used across the application.
        /// </summary>
        public IServiceRegistry Services { get; set; }
    }
}
