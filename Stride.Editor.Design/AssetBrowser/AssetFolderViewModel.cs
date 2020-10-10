using Stride.Editor.Design.Core.Hierarchy;

namespace Stride.Editor.Design.AssetBrowser
{
    public class AssetFolderViewModel : HierarchyItemViewModel
    {
        public AssetFolderViewModel(string name)
        {
            Name = name;
            IsFolder = true;
        }
    }
}