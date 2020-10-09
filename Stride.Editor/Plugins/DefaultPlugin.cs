using Stride.Core;
using Stride.Editor.Design;
using Stride.Editor.Design.Core;
using Stride.Editor.Design.Core.Menu;
using Stride.Editor.Presentation;
using Stride.Editor.Presentation.Core;
using Stride.Editor.Presentation.Core.Member;

namespace Stride.Editor.Plugins
{
    /// <summary>
    /// Registers core components.
    /// </summary>
    public class DefaultPlugin : IEditorPlugin
    {
        public void RegisterPlugin(IServiceRegistry services)
        {
            var viewRegistry = services.GetSafeServiceAs<ViewRegistry>();
            viewRegistry.RegisterView<MenuViewModel, MenuView>();
            viewRegistry.RegisterView<EditorViewModel, EditorView>();

            DefaultMemberViews.Register(services);

            // TODO: register standard menu
        }
    }
}
