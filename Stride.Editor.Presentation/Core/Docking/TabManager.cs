using Avalonia.Collections;
using Avalonia.Threading;
using Dock.Model;
using Dock.Model.Controls;
using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Editor.Commands;
using Stride.Editor.Design;
using Stride.Editor.Design.Core;
using Stride.Editor.Design.Core.Docking;
using Stride.Editor.Design.Core.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core.Docking
{
    public class TabManager : ITabManager
    {
        private static LoggingScope Logger = LoggingScope.Global(nameof(TabManager));

        public TabManager(IServiceRegistry services)
        {
            CommandDispatcher = services.GetSafeServiceAs<ICommandDispatcher>();
            Session = services.GetSafeServiceAs<IRootViewModelContainer>();
            InitializeDockLayout();
        }

        private ICommandDispatcher CommandDispatcher { get; }
        private IRootViewModelContainer Session { get; }
        private HashSet<ITabViewModel> tabs = new HashSet<ITabViewModel>();
        private DockFactory factory = new DockFactory();

        public IDock Layout { get; private set; }
        
        public IViewBuilder GetControl() => new Virtual.Dock.DockControl()
        {
            Layout = this.Layout,
        };

        public async Task CloseTab(ITabViewModel tabViewModel)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Logger.Debug($"Close tab '{tabViewModel.Id}'.");
                factory.RemoveDockable(tabViewModel as IDockable, collapse: false);
                Session.RootViewModel.Tabs.Remove(tabViewModel);
            });
        }

        public async Task<ITabViewModel> CreateEditorTab(IAssetEditor editor)
        {
            return await Dispatcher.UIThread.InvokeAsync(() =>
            {
                var tab = new EditorTabViewModel(editor);
                Logger.Debug($"Create new editor tab '{tab.Id}'.");
                factory.AddDockable(factory.Documents, tab);
                tabs.Add(tab);
                Session.RootViewModel.Tabs.Add(tab, editor);
                Session.RootViewModel.ActiveTab = tab;
                return tab;
            });
        }

        public async Task<ITabViewModel> CreateToolTab(object viewModel)
        {
            return await Dispatcher.UIThread.InvokeAsync(() =>
            {
                var tab = new ToolTabViewModel(viewModel);
                Logger.Debug($"Create new tool tab '{tab.Id}'.");
                factory.AddDockable(factory.Tools, tab);
                tabs.Add(tab);
                Session.RootViewModel.Tabs.Add(tab, viewModel);
                Session.RootViewModel.ActiveTab = tab;
                return tab;
            });
        }

        public async Task FocusTab(ITabViewModel tabViewModel)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Logger.Debug($"Focus tab '{tabViewModel.Id}'.");
                factory.SetActiveDockable(tabViewModel as IDockable);
                Session.RootViewModel.ActiveTab = tabViewModel;
            });
        }

        private void InitializeDockLayout()
        {
            Logger.Debug($"Initialize dock layout.");
            Layout = factory.CreateLayout();
            factory.InitLayout(Layout);
        }

        private class DockFactory : Factory
        {
            public ToolDock Tools { get; private set; }
            public DocumentDock Documents { get; private set; }
            public override IDock CreateLayout()
            {
                Tools = new ToolDock();
                Documents = new DocumentDock();
                return new RootDock
                {
                    // We delegate the rendering of tabs to the ViewDataTemplate
                    VisibleDockables = new AvaloniaList<IDockable>
                    {
                        new ProportionalDock
                        {
                            VisibleDockables = new AvaloniaList<IDockable>
                            {
                                Tools,
                                new SplitterDock(),
                                Documents,
                            }
                        }
                    }
                };
            }
        }
    }
}
