using Stride.Core;
using Stride.Core.Assets;
using Stride.Editor.Commands;
using Stride.Editor.Design.Core;

namespace Stride.Editor.Services
{
    public partial class Session
    {
        public EditorViewModel EditorViewModel { get; set; }

        public PackageSession PackageSession { get; set; }

        public ICommandDispatcher CommandDispatcher { get; set; }

        public IServiceRegistry Services { get; set; }

        public IViewUpdater ViewUpdater { get; set; }

        public IUndoService UndoService { get; set; }
    }
}
