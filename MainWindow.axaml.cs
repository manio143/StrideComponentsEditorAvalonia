using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Build.Locator;
using ReactiveUI;
using Stride.Assets.Entities;
using Stride.Core.Assets;
using Stride.Editor.Avalonia.EntityHierarchy.Components;
using Stride.Editor.Avalonia.EntityHierarchy.Components.Views;
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
        public bool IsLoading { get; set; }
        public MainWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            // We're binding menu to some methods on this class
            DataContext = this;
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
                PackageSession.Load(@"D:\Documents\Stride Projects\RiseOfTheUndeaf\RiseOfTheUndeaf.sln", sessionResult);

                sessionResult.Session.LoadMissingReferences(sessionResult);

                foreach (var pack in sessionResult.Session.Packages)
                {
                    foreach (var asset in pack.Assets)
                    {
                        // TODO: how to find the default scene?
                        if (asset.Id == new AssetId("5844eac5-b9de-4cbf-8145-b653b75caf98"))
                        {
                            var scene = (SceneAsset)asset.Asset;

                            await Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                // remove "Loading..."
                                this.FindControl<DockPanel>("DockPanel").Children.Remove(loadingBlock);

                                // create a tree of entities (and folders) that can be interacted with
                                var treeView = BuildTreeView(scene);
                                this.FindControl<ContentControl>("Hierarchy").Content = treeView;
                            });
                        }
                    }
                }
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Visit all entities and construct a <see cref="TreeView"/> of them.
        /// </summary>
        private TreeView BuildTreeView(SceneAsset scene)
        {
            var entityMapping = new Dictionary<Entity, TreeViewItem>();
            var rootTreeViewItems = new List<TreeViewItem>();
            var folders = new Dictionary<string, List<TreeViewItem>>();

            foreach (var part in scene.Hierarchy.Parts)
            {
                var entityDesign = part.Value;
                if (entityMapping.ContainsKey(entityDesign.Entity))
                    continue; // it has been processed recursively by its children
                ProcessEntityPart(entityMapping, rootTreeViewItems, folders, entityDesign, scene.Hierarchy.Parts);
            }

            foreach (var fol in folders)
            {
                rootTreeViewItems.Add(new TreeViewItem { Items = fol.Value, Header = fol.Key, Tag = null });
            }

            var tree = new TreeView { Items = rootTreeViewItems };
            tree.SelectionChanged += Tree_SelectionChanged;
            return tree;
        }

        private static void ProcessEntityPart(Dictionary<Entity, TreeViewItem> entityMapping, List<TreeViewItem> rootTreeViewItems, Dictionary<string, List<TreeViewItem>> folders, EntityDesign entityDesign, AssetPartCollection<EntityDesign, Entity> parts)
        {
            if (!String.IsNullOrEmpty(entityDesign.Folder))
            {
                // for now we'll have flat folders but later this needs to be ammended
                if (!folders.ContainsKey(entityDesign.Folder))
                    folders[entityDesign.Folder] = new List<TreeViewItem>();

                var tvi = new TreeViewItem { Tag = entityDesign, Header = entityDesign.Entity.Name };
                
                folders[entityDesign.Folder].Add(tvi);
                entityMapping[entityDesign.Entity] = tvi;
            }
            else
            {
                var parent = entityDesign.Entity.GetParent();
                if (parent != null)
                {
                    // first make sure the parent has been processed
                    if (!entityMapping.ContainsKey(parent))
                        ProcessEntityPart(entityMapping, rootTreeViewItems, folders, parts[parent.Id], parts);

                    // then insert a new subitem
                    var items = (entityMapping[parent].Items as IList);
                    if (items == null)
                        entityMapping[parent].Items = items = new List<TreeViewItem>();
                    
                    var tvi = new TreeViewItem { Tag = entityDesign, Header = entityDesign.Entity.Name };
                    
                    items.Add(tvi);
                    entityMapping[entityDesign.Entity] = tvi;
                }
                else
                {
                    var tvi = new TreeViewItem { Tag = entityDesign, Header = entityDesign.Entity.Name };
                    
                    rootTreeViewItems.Add(tvi);
                    entityMapping[entityDesign.Entity] = tvi;
                }
            }
        }

        private void Tree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                var tvi = (TreeViewItem)e.AddedItems[0];

                if (tvi.Tag == null)
                    return; // folder has been selected, don't do anything

                var entityDesign = tvi.Tag as EntityDesign;

                this.FindControl<ContentControl>("Components").Content = new ScrollViewer
                {
                    Content = new ListBox
                    {
                        Items = entityDesign.Entity.Components.Select(c
                            => new ComponentView(new ComponentViewModel(c))),
                    }
                };
            }
            else
            {
                this.FindControl<ContentControl>("Components").Content = null;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
