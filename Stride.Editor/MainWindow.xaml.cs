using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Design;
using Stride.Editor.Design.AssetBrowser;
using Stride.Editor.Design.Core;
using Stride.Editor.Design.Core.Docking;
using Stride.Editor.Design.Core.Logging;
using Stride.Editor.Design.Core.Menu;
using Stride.Editor.Presentation;
using Stride.Editor.Presentation.Core;
using Stride.Editor.Presentation.Core.Docking;
using Stride.Editor.Services;
using System;
using System.Collections.Generic;

namespace Stride.Editor
{
    public class MainWindow : Window
    {
        private ServiceRegistry Services = new ServiceRegistry();
        private Session Session;
        public MainWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            using var initScope = new TimedScope(LoggingScope.Global($"{nameof(MainWindow)}.InitializeServices"));

            Session = new Session();

            Session.EditorViewModel = new EditorViewModel();
            // TODO: move onto menuprovider
            Session.EditorViewModel.Menu = new MenuViewModel
            {
                Items = new List<MenuItemViewModel>
                {
                    new MenuItemViewModel
                    {
                        Header = "_File",
                        Items = new List<MenuItemViewModel>
                        {
                            new MenuItemViewModel
                            {
                                Header = "Open (const)",
                                Command = ReactiveUI.ReactiveCommand.Create(async () =>
                                {
                                    await Session.LoadProject(@"D:\Documents\Stride Projects\MinimalTestProject\MinimalTestProject.sln");
                                    var browser = new AssetBrowserViewModel(Session.PackageSession);
                                    Session.EditorViewModel.Tabs.Add(Services.GetService<TabManager>().CreateToolTab(browser), browser);
                                    await Services.GetService<IViewUpdater>().UpdateView();
                                }),
                            }
                        }
                    }
                }
            };
            Session.Services = Services;
            
            Services.AddService<Session>(Session);
            Services.AddService<IRootViewModelContainer>(Session);
            Services.AddService<ViewRegistry>(new ViewRegistry());

            var viewDataTemplate = new ViewDataTemplate(Services);
            Services.AddService<IViewUpdater>(viewDataTemplate);
            DataTemplates.Add(viewDataTemplate);

            Services.AddService<IUndoService>(new UndoService());
            Services.AddService<ICommandDispatcher>(new CommandDispatcher(Services));
            Services.AddService<IMemberViewProvider<IViewBuilder>>(new MemberViewProvider(Services));

            var tabManager = new TabManager(Services);
            Services.AddService<ITabManager>(tabManager);
            Services.AddService<TabManager>(tabManager);

            var assetManager = new AssetManager(Services);
            Services.AddService<IAssetManager>(assetManager);
            Services.AddService<IAssetEditorRegistry>(assetManager);

            var pluginRegistry = new PluginRegistry(Services);
            pluginRegistry.RefreshAvailablePlugins();
            
            Services.AddService<PluginRegistry>(pluginRegistry);
            
            foreach (var initialPlugin in pluginRegistry.AvailablePlugins)
                pluginRegistry.Register(initialPlugin);

            // It will be rendered by the DataTemplate
            DataContext = Session.EditorViewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
