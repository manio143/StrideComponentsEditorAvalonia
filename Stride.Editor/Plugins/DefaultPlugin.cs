using Stride.Core;
using Stride.Editor.Design;
using Stride.Editor.Design.AssetBrowser;
using Stride.Editor.Design.Core;
using Stride.Editor.Design.Core.Menu;
using Stride.Editor.Menu;
using Stride.Editor.Presentation;
using Stride.Editor.Presentation.AssetBrowser;
using Stride.Editor.Presentation.Core;
using Stride.Editor.Presentation.Core.Docking;
using Stride.Editor.Presentation.Core.Member;
using System;

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
            viewRegistry.RegisterView<AssetBrowserViewModel, AssetBrowserView>();
            viewRegistry.RegisterView<EditorTabViewModel, EditorTabView>();
            viewRegistry.RegisterView<ToolTabViewModel, ToolTabView>();

            DefaultMemberViews.Register(services);

            RegisterDefaultMenu(services);
        }

        private void RegisterDefaultMenu(IServiceRegistry services)
        {
            var menuProvider = services.GetSafeServiceAs<IMenuProvider>();
            menuProvider.RegisterMenuItem("/", new MenuItemViewModel { Header = "_File" });
            menuProvider.RegisterMenuItem("/", new MenuItemViewModel { Header = "_Edit" });

            menuProvider.RegisterMenuItem("/File", new MenuItemViewModel
            {
                Header = "_Open...",
                Command = new OpenCommand(services),
            });

            menuProvider.RegisterMenuItem("/Edit", new MenuItemViewModel
            {
                Header = "_Undo",
                Command = new UndoCommand(services),
            });
            menuProvider.RegisterMenuItem("/Edit", new MenuItemViewModel
            {
                Header = "_Redo",
                Command = new RedoCommand(services),
            });
        }

        public void UnregisterPlugin(IServiceRegistry services)
        {
            throw new InvalidOperationException("Default views should never be unregistered.");
        }
    }
}
