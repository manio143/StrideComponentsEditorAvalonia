using Stride.Core;
using Stride.Core.Assets;
using Stride.Editor.Commands;
using Stride.Editor.Design.Core;

namespace Stride.Editor.Services
{
    public partial class Session
    {
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
