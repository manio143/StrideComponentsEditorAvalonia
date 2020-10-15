using Avalonia.Collections;
using Avalonia.Data;
using Dock.Avalonia.Controls;
using Dock.Model;
using Dock.Model.Controls;
using Stride.Core;
using Stride.Editor.Design.Core;
using Stride.Editor.Presentation.Core.Docking;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var dockingSystem = CreateDocking(viewModel);

            return new Virtual.DockPanel
            {
                Children = new IViewBuilder[]
                {
                    menu,
                    dockingSystem,
                }
            };
        }

        private IViewBuilder CreateDocking(EditorViewModel viewModel)
        {
            return new Virtual.Dock.DockControl(TabViewModelComparer.Equal)
            {
                Layout = TabManager.Layout,
            };
        }
    }
}
