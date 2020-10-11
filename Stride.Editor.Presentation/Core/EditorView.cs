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
        }

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

        // TODO: move this to a separate class
        private IViewBuilder CreateDocking(EditorViewModel viewModel)
        {
            var factory = new DockFactory(viewModel);
            var layout = factory.CreateLayout();
            factory.InitLayout(layout);

            return new Virtual.Dock.DockControl(TabViewModelComparer.Equal)
            {
                Layout = layout,
            };
        }

        private class DockFactory : Factory
        {
            public DockFactory(EditorViewModel viewModel)
                => this.viewModel = viewModel;
            private readonly EditorViewModel viewModel;
            public override IDock CreateLayout()
            {
                return new RootDock
                {
                    // We delegate the rendering of tabs to the ViewDataTemplate
                    VisibleDockables = new AvaloniaList<IDockable>
                    {
                        new ToolDock
                        {
                            VisibleDockables = new AvaloniaList<IDockable>(viewModel.Tabs.Keys.Cast<IDockable>()),
                        },
                    }
                };
            }
            public override void InitLayout(IDockable layout)
            {
                this.ContextLocator = new Dictionary<string, Func<object>>
                {
                    [nameof(IRootDock)] = () => viewModel,
                    [nameof(IProportionalDock)] = () => viewModel,
                    [nameof(IDocumentDock)] = () => viewModel,
                    [nameof(IToolDock)] = () => viewModel,
                    [nameof(ISplitterDock)] = () => viewModel,
                    [nameof(IDockWindow)] = () => viewModel,
                    [nameof(IDocument)] = () => viewModel,
                    [nameof(ITool)] = () => viewModel,
                };
                this.HostWindowLocator = new Dictionary<string, Func<IHostWindow>>
                {
                    [nameof(IDockWindow)] = () =>
                    {
                        var hostWindow = new HostWindow()
                        {
                            [!HostWindow.TitleProperty] = new Binding("ActiveDockable.Title")
                        };
                        return hostWindow;
                    }
                };

                base.InitLayout(layout);
            }
        }
    }
}
