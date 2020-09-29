using Stride.Assets.Entities;
using Stride.Core.Assets;
using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Design.SceneEditor
{
    public class SceneViewModel
    {
        public SceneViewModel(SceneAsset sceneAsset)
        {
            if (sceneAsset == null) throw new ArgumentNullException(nameof(sceneAsset));
            Source = sceneAsset;
            Items = BuildHierarchyTree(Source);
        }

        public SceneAsset Source { get; }

        public List<HierarchyItemViewModel> Items { get; }

        /// <summary>
        /// Find the <see cref="EntityViewModel"/> corresponding to an entity with <paramref name="entityId"/>. 
        /// </summary>
        /// <returns>ViewModel for an entity with this Id, or null if not found.</returns>
        public EntityViewModel FindById(Guid entityId)
        {
            foreach (var item in Items)
            {
                var evm = item.FindById(entityId);
                if (evm != null)
                    return evm;
            }
            return null;
        }

        private static List<HierarchyItemViewModel> BuildHierarchyTree(SceneAsset scene)
        {
            var result = new List<HierarchyItemViewModel>();
            var folders = new List<FolderViewModel>();

            foreach (var entity in scene.Hierarchy.RootParts)
                AddRootEntity(entity, scene.Hierarchy.Parts, folders, result);

            folders.Sort(new HierarchyItemViewModelComparer());
            result.InsertRange(0, folders);

            return result;
        }

        private static void AddRootEntity(Entity entity, AssetPartCollection<EntityDesign, Entity> parts, List<FolderViewModel> folders, List<HierarchyItemViewModel> result)
        {
            var entityDesign = parts[entity.Id];

            var viewModel = new EntityViewModel(entityDesign);
            viewModel.AddChildren(parts);

            if (!String.IsNullOrEmpty(entityDesign.Folder))
            {
                var folder = GetFolder(entityDesign.Folder, folders);
                folder.Children.Add(viewModel);
            }
            else
            {
                result.Add(viewModel);
            }
        }

        /// <summary>
        /// Finds or creates a folder corresponding to <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Path to the folder</param>
        /// <param name="folders">Root folder</param>
        private static FolderViewModel GetFolder(string path, List<FolderViewModel> folders)
        {
            var subfolders = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            FolderViewModel folderViewModel = null;
            foreach (var folder in subfolders)
            {
                if (folderViewModel == null)
                {
                    var found = folders.Find(f => f.Name == folder);
                    if (found == null)
                    {
                        folderViewModel = new FolderViewModel(folder);
                        folders.Add(folderViewModel);
                    }
                    else
                    {
                        folderViewModel = found;
                    }
                }
                else
                {
                    var found = (FolderViewModel)folderViewModel.Children.Find(f => f.Name == folder && f.IsFolder);
                    if (found == null)
                    {
                        var newSubFolder = new FolderViewModel(folder);
                        folderViewModel.Children.Add(newSubFolder);
                        folderViewModel = newSubFolder;
                    }
                    else
                    {
                        folderViewModel = found;
                    }
                }

                // Should we sort folders only at the end? What is faster?
                folderViewModel.Children.Sort(new HierarchyItemViewModelComparer());
            }
            return folderViewModel;
        }
    }
}
