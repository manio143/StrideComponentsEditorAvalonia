using Stride.Core.Assets;
using Stride.Editor.Design.Core.Hierarchy;
using System.Collections.Generic;

namespace Stride.Editor.Design.AssetBrowser
{
    public class AssetBrowserViewModel
    {
        public AssetBrowserViewModel(PackageSession packageSession)
        {
            Source = packageSession;

            var packages = packageSession.LocalPackages;
            foreach (var package in packages)
            {
                var packageVM = new PackageItemViewModel(package);
                this.packages.Add(packageVM);

                foreach (var asset in package.Assets)
                {
                    var url = asset.Location;
                    // note: should this be used for imported projects, we might want to add an additional filter to code assets, or not include them altogether (?)
                    var baseDirectory = asset.Asset is IProjectAsset ? "Code/" : "Assets/";
                    var folderVM = packageVM.GetOrCreateFolder(baseDirectory + (url.GetFullDirectory() ?? ""));
                    var assetVM = new AssetItemViewModel(asset);
                    folderVM.Children.Add(assetVM);
                }
            }
        }

        private List<PackageItemViewModel> packages = new List<PackageItemViewModel>();

        public IEnumerable<HierarchyItemViewModel> Items => packages;

        public PackageSession Source { get; }
    }
}
