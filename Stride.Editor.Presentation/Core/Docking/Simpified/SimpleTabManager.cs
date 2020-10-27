using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Editor.Commands;
using Stride.Editor.Design;
using Stride.Editor.Design.Core;
using Stride.Editor.Design.Core.Docking;
using Stride.Editor.Design.Core.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core.Docking.Simpified
{
    public class SimpleTabManager : ITabManager
    {
        private static LoggingScope Logger = LoggingScope.Global(nameof(SimpleTabManager));

        public SimpleTabManager(IServiceRegistry services)
        {
            Services = services;
            CommandDispatcher = services.GetSafeServiceAs<ICommandDispatcher>();
            Session = services.GetSafeServiceAs<IRootViewModelContainer>();
            ViewRegistry = services.GetSafeServiceAs<ViewRegistry>();
        }

        private IServiceRegistry Services { get; }
        private ICommandDispatcher CommandDispatcher { get; }
        private IRootViewModelContainer Session { get; }
        private ViewRegistry ViewRegistry { get; }

        private List<ITabViewModel> toolTabs = new List<ITabViewModel>();
        private List<ITabViewModel> editorTabs = new List<ITabViewModel>();

        private ITabViewModel selectedToolTab;
        private ITabViewModel selectedEditorTab;

        /// <remarks>
        /// Should be called from the UI thread.
        /// </remarks>
        public IViewBuilder GetControl() => new Virtual.Grid
        {
            ColumnDefinitions = new Avalonia.Controls.ColumnDefinitions("*,2*"),
            RowDefinitions = new Avalonia.Controls.RowDefinitions("*"),
            Children = GetGridChildren(),
        };

        private IViewBuilder[] GetGridChildren()
        {
            var tools = new Virtual.TabControl
            {
                Items = toolTabs.Select(tab => new Virtual.TabItem
                {
                    Content = ViewRegistry.GetView(tab, Services).CreateView(tab),
                    Header = tab.Title,
                    IsSelected = tab == selectedToolTab,
                    OnSelected = async () => await FocusTab(tab),
                }),
            };
            var editors = new Virtual.TabControl
            {
                Items = editorTabs.Select(tab => new Virtual.TabItem
                {
                    Content = ViewRegistry.GetView(tab, Services).CreateView(tab),
                    Header = tab.Title,
                    IsSelected = tab == selectedEditorTab,
                    OnSelected = async () => await FocusTab(tab),
                }),
            };
            tools.Property(Avalonia.Controls.Grid.ColumnProperty, 0);
            editors.Property(Avalonia.Controls.Grid.ColumnProperty, 1);
            return new IViewBuilder[] { tools, editors };
        }

        public async Task CloseTab(ITabViewModel tabViewModel)
        {
            Logger.Debug($"Close tab '{tabViewModel.Id}'.");

            var toolIdx = toolTabs.IndexOf(tabViewModel);
            var editorIdx = editorTabs.IndexOf(tabViewModel);

            if (toolIdx >= 0)
            {
                toolTabs.Remove(tabViewModel);
                if (selectedToolTab == tabViewModel)
                {
                    if (toolTabs.Count > 0)
                    {
                        toolIdx = toolIdx < toolTabs.Count ? toolIdx : toolTabs.Count - 1;
                        selectedToolTab = toolTabs[toolIdx];
                    }
                }
                await FocusTab(selectedToolTab);
            }
            else
            {
                editorTabs.Remove(tabViewModel);
                if (selectedEditorTab == tabViewModel)
                {
                    if (editorTabs.Count > 0)
                    {
                        editorIdx = editorIdx < editorTabs.Count ? editorIdx : editorTabs.Count - 1;
                        selectedEditorTab = editorTabs[editorIdx];
                    }
                }
                await FocusTab(selectedEditorTab);
            }

            Session.RootViewModel.Tabs.Remove(tabViewModel);
        }

        public async Task<ITabViewModel> CreateEditorTab(IAssetEditor editor)
        {
            var tab = await AvaloniaUIThread.InvokeAsync(() => new EditorTabViewModel(editor));
            Logger.Debug($"Create new editor tab '{tab.Id}'.");

            editorTabs.Add(tab);
            selectedEditorTab = tab;

            Session.RootViewModel.Tabs.Add(tab, editor);
            Session.RootViewModel.ActiveTab = tab;
            return tab;
        }

        public async Task<ITabViewModel> CreateToolTab(object viewModel)
        {
            var tab = await AvaloniaUIThread.InvokeAsync(() => new ToolTabViewModel(viewModel));
            Logger.Debug($"Create new tool tab '{tab.Id}'.");

            toolTabs.Add(tab);
            selectedToolTab = tab;

            Session.RootViewModel.Tabs.Add(tab, viewModel);
            Session.RootViewModel.ActiveTab = tab;
            return tab;
        }

        public async Task FocusTab(ITabViewModel tabViewModel)
        {
            Logger.Debug($"Focus tab '{tabViewModel.Id}'.");

            if (tabViewModel is EditorTabViewModel)
                selectedEditorTab = tabViewModel;
            else
                selectedToolTab = tabViewModel;

            Session.RootViewModel.ActiveTab = tabViewModel;
        }
    }
}
