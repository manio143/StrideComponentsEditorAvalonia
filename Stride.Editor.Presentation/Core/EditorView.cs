using Stride.Core;
using Stride.Editor.Design.Core;
using Stride.Editor.Presentation.Core.Docking;
using System.Collections.Generic;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core
{
    public class EditorView : ViewBase<EditorViewModel>
    {
        public EditorView(IServiceRegistry services) : base(services)
        {
            TabManager = services.GetSafeServiceAs<TabManager>();
        }

        private TabManager TabManager { get; }

        public override IViewBuilder CreateView(EditorViewModel viewModel)
        {
            var menu = new MenuView(Services).CreateView(viewModel.Menu);
            menu.Property(Avalonia.Controls.DockPanel.DockProperty, Avalonia.Controls.Dock.Top);

            var statusBar = CreateStatusBar(viewModel);
            statusBar.Property(Avalonia.Controls.DockPanel.DockProperty, Avalonia.Controls.Dock.Bottom);

            var dockingSystem = TabManager.GetControl();

            return new Virtual.DockPanel
            {
                Children = new IViewBuilder[]
                {
                    menu,
                    statusBar,
                    // top/bottom children have to go before left/right children
                    dockingSystem,
                }
            };
        }

        private IViewBuilder CreateStatusBar(EditorViewModel viewModel)
        {
            var items = new List<IViewBuilder>();

            var statusText = new Virtual.TextBlock
            {
                Text = "Status",
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Width = 50,
            };
            statusText.Property(Avalonia.Layout.Layoutable.MarginProperty, new Avalonia.Thickness(8, 0));
            items.Add(statusText);

            if (viewModel.LoadingStatus != null)
            {
                var progressBar = new LoadingStatusView(Services).CreateView(viewModel.LoadingStatus);
                items.Add(progressBar);
            }

            return new Virtual.StackPanel
            {
                MinHeight = 20,
                Children = items,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
            };
        }
    }
}
