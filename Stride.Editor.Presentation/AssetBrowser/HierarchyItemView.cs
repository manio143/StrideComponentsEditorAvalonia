using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Commands.AssetManager;
using Stride.Editor.Commands.Core.Hierarchy;
using Stride.Editor.Design;
using Stride.Editor.Design.AssetBrowser;
using Stride.Editor.Design.Core.Hierarchy;
using System.Linq;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.AssetBrowser
{
    public class HierarchyItemView : ViewBase<HierarchyItemViewModel>
    {
        public HierarchyItemView(IServiceRegistry services) : base(services)
        {
            dispatcher = Services.GetSafeServiceAs<ICommandDispatcher>();
            assetManager = Services.GetSafeServiceAs<IAssetManager>();
        }

        private ICommandDispatcher dispatcher;
        private IAssetManager assetManager;

        public override IViewBuilder CreateView(HierarchyItemViewModel viewModel)
        {
            var assetItem = viewModel as AssetItemViewModel;
            var tvi = new Virtual.TreeViewItem
            {
                Header = viewModel.Name, // TODO: add project/folder/asset icon
                // ContextMenu = ... TODO: generate asset context menu
                Tag = viewModel,
                Items = viewModel.Children.Select(c => CreateView(c)),
                IsExpanded = viewModel.IsExpanded,
                IsSelected = viewModel.IsSelected,
                OnSelected = selected =>
                    dispatcher.Dispatch(
                        new SelectHierarchyItemCommand(),
                        new SelectHierarchyItemCommand.Context
                        {
                            ViewModel = viewModel,
                            Selected = selected,
                        }),
                OnExpanded = expanded =>
                    dispatcher.Dispatch(
                        new ExpandHierarchyItemCommand(),
                        new ExpandHierarchyItemCommand.Context
                        {
                            ViewModel = viewModel,
                            Expand = expanded,
                        }),
            };

            if (assetItem != null)
                tvi.OnDoubleClick = (args) =>
                    dispatcher.Dispatch(
                        new OpenAssetCommand(),
                        new OpenAssetCommand.Context
                        {
                            AssetItem = assetItem.Source,
                            AssetManager = assetManager,
                        });
            else
                tvi.OnDoubleClick = Ignore;

            return tvi;
        }

        private static void Ignore<T>(T x) { }
    }
}
