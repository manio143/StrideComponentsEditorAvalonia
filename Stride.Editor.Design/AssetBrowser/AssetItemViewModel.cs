using Stride.Core.Assets;
using Stride.Editor.Design.Core.Hierarchy;
using System;

namespace Stride.Editor.Design.AssetBrowser
{
    public class AssetItemViewModel : HierarchyItemViewModel
    {
        public AssetItemViewModel(AssetItem asset)
        {
            Source = asset;
            Name = asset.Location.GetFileNameWithoutExtension();
            IsFolder = false;
        }

        public AssetItem Source { get; }

        public override TChild FindById<TChild>(Guid id)
        {
            if ((Guid)Source.Id == id)
                return this as TChild;
            return base.FindById<TChild>(id);
        }
    }
}