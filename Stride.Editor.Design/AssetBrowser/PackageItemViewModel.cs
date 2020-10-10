using Stride.Core.Assets;
using Stride.Core.IO;
using Stride.Editor.Design.Core.Hierarchy;
using System;
using System.Linq;

namespace Stride.Editor.Design.AssetBrowser
{
    public class PackageItemViewModel : HierarchyItemViewModel
    {
        public PackageItemViewModel(Package package)
        {
            Source = package;
            Name = package.Meta.Name;
            IsFolder = true;
        }

        public Package Source { get; }

        public AssetFolderViewModel GetOrCreateFolder(string path)
        {
            var directories = path.Split("/", StringSplitOptions.RemoveEmptyEntries);
            HierarchyItemViewModel hierarchyItem = this;
            foreach (var dir in directories)
            {
                var folder = hierarchyItem.Children.FirstOrDefault(i => i.Name == dir);
                if (folder == null)
                {
                    folder = new AssetFolderViewModel(dir);
                    hierarchyItem.Children.Add(folder);
                }
                hierarchyItem = folder;
            }
            return (AssetFolderViewModel)hierarchyItem;
        }
    }
}