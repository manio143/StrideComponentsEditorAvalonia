using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Build.Locator;
using ReactiveUI;
using Stride.Assets.Entities;
using Stride.Core;
using Stride.Core.Assets;
using Stride.Editor.Commands;
using Stride.Editor.Design.Core;
using Stride.Editor.Design.Core.Menu;
using Stride.Editor.Design.SceneEditor;
using Stride.Editor.Presentation;
using Stride.Editor.Presentation.Core;
using Stride.Editor.Presentation.SceneEditor;
using Stride.Editor.Presentation.VirtualDom;
using Stride.Editor.Services;
using Stride.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stride.Editor.Avalonia
{
    public class MainWindow : Window
    {
        private ServiceRegistry Services = new ServiceRegistry();
        private AssetEditorViewUpdater viewUpdater;
        private CommandDispatcher commandDispatcher;
        private UndoService undoService;
        private ViewContainer menuViewContainer;
        public bool IsLoading { get; set; }
        public MainWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            var menuContainer = this.FindControl<ContentControl>("MenuContainer");
            menuViewContainer = new ViewContainer(menuContainer, _ => new MenuView(Services).CreateView(BuildMenu()));
            menuViewContainer.Update(null).Wait();

            DataContext = this;

            viewUpdater = new AssetEditorViewUpdater(Services, this.FindControl<ContentControl>("AssetEditorContainer"));
            Services.AddService<IAssetEditorViewUpdater>(viewUpdater);

            undoService = new UndoService();
            undoService.StateChanged += () =>
            {
                menuViewContainer.Update(null);
            }; // refresh menu
            Services.AddService<IUndoService>(undoService);

            commandDispatcher = new CommandDispatcher(Services);
            Services.AddService<ICommandDispatcher>(commandDispatcher);

            Services.AddService<IMemberViewProvider<IViewBuilder>>(new MemberViewProvider(Services));
        }

        public async void OpenScene()
        {
            // show "Loading..."
            TextBlock loadingBlock = null;
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                loadingBlock = new TextBlock { Text = "Loading..." };
                this.FindControl<DockPanel>("DockPanel").Children.Add(loadingBlock);
            }).ConfigureAwait(false);

            MSBuildLocator.RegisterDefaults(); // see https://github.com/microsoft/MSBuildLocator/issues/64

            // in this result will be any errors from loading the project
            var sessionResult = new PackageSessionResult();

            await Task.Run(async () =>
            {
                // TODO: obtain the path with a file dialog
                PackageSession.Load(@"D:\Documents\Stride Projects\MinimalTestProject\MinimalTestProject.sln", sessionResult);

                sessionResult.Session.LoadMissingReferences(sessionResult);

                foreach (var pack in sessionResult.Session.Packages)
                {
                    foreach (var asset in pack.Assets)
                    {
                        // TODO: how to find the default scene?
                        if (asset.Id == new AssetId("ac69f056-347d-4abd-8c64-b56bd8c2004a"))
                        {
                            var scene = (SceneAsset)asset.Asset;

                            await Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                // remove "Loading..."
                                this.FindControl<DockPanel>("DockPanel").Children.Remove(loadingBlock);

                                var sceneEditor = new SceneEditor(scene);
                                commandDispatcher.SetActiveEditor(sceneEditor);
                            });
                        }
                    }
                }
            }).ConfigureAwait(false);
        }

        public bool CanUndo { get; set; }
        public bool CanRedo { get; set; }
        public void Undo() => undoService.Undo();
        public void Redo() => undoService.Redo();

        private IEnumerable<MenuItemViewModel> BuildMenu() => new[]
        {
            new MenuItemViewModel
            {
                Header = "_File",
                Items = new[]
                {
                    new MenuItemViewModel { Header = "_Open", Command = ReactiveCommand.Create(OpenScene) },
                    new MenuItemViewModel { Header = "E_xit", Command = ReactiveCommand.Create(Close) },
                }
            },
            new MenuItemViewModel
            {
                Header = "_Edit",
                Items = new[]
                {
                    new MenuItemViewModel
                    {
                        Header = "_Undo",
                        IsEnabled = undoService?.CanUndo ?? false,
                        Command = ReactiveCommand.Create(Undo),
                    },
                    new MenuItemViewModel
                    {
                        Header = "_Redo",
                        IsEnabled = undoService?.CanRedo ?? false,
                        Command = ReactiveCommand.Create(Redo),
                    },
                }
            },
        };


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
