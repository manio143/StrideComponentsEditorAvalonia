using Stride.Assets.Entities;
using Stride.Core.Assets;
using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.Editor.Design.SceneEditor
{
    public class SceneViewModel
    {
        public SceneViewModel(SceneAsset sceneAsset)
        {
            if (sceneAsset == null) throw new ArgumentNullException(nameof(sceneAsset));
            AssetSource = sceneAsset;
            Items = BuildHierarchyTree(AssetSource.Hierarchy.RootParts, AssetSource.Hierarchy.Parts);
        }

        public SceneViewModel(Scene scene)
        {
            if (scene == null) throw new ArgumentNullException(nameof(scene));
            GameSource = scene;
            Items = BuildHierarchyTree(GameSource.Entities, MapEntityById(GameSource.Entities));
        }

        public SceneAsset AssetSource { get; }

        public Scene GameSource { get; }

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

        private static List<HierarchyItemViewModel> BuildHierarchyTree(IList<Entity> entities, IDictionary<Guid, EntityDesign> designData)
        {
            var result = new List<HierarchyItemViewModel>();
            var folders = new List<FolderViewModel>();

            foreach (var entity in entities)
                AddRootEntity(entity, designData, folders, result);

            folders.Sort(new HierarchyItemViewModelComparer());
            result.InsertRange(0, folders);

            return result;
        }

        private static void AddRootEntity(Entity entity, IDictionary<Guid, EntityDesign> designData, List<FolderViewModel> folders, List<HierarchyItemViewModel> result)
        {
            var entityDesign = designData[entity.Id];

            var viewModel = new EntityViewModel(entityDesign);
            viewModel.AddChildren(designData);

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

        private static Dictionary<Guid, EntityDesign> MapEntityById(IEnumerable<Entity> entities)
        {
            var dict = new Dictionary<Guid, EntityDesign>();

            static void Traverse(Entity e, Dictionary<Guid, EntityDesign> d)
            {
                d.Add(e.Id, new EntityDesign(e));
                foreach (var child in e.GetChildren())
                    Traverse(child, d);
            }

            foreach (var entity in entities)
                Traverse(entity, dict);
            
            return dict;
        }
    }
}
