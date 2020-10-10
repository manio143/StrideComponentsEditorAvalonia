using Stride.Core;
using System.Linq;
using Avalonia.Controls;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;
using Stride.Editor.Design.AssetBrowser;

namespace Stride.Editor.Presentation.AssetBrowser
{
    public class AssetBrowserView : ViewBase<AssetBrowserViewModel>
    {
        public AssetBrowserView(IServiceRegistry services) : base(services)
        {
        }

        public override IViewBuilder CreateView(AssetBrowserViewModel scene)
        {
            var itemView = new HierarchyItemView(Services);

            var tree = new Virtual.TreeView
            {
                Items = scene.Items.Select(item => itemView.CreateView(item)),
                SelectionMode = SelectionMode.Multiple,
                AutoScrollToSelectedItem = true,
            };
            return tree;
        }
    }
}
