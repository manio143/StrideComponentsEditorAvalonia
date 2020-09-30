using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Commands.SceneEditor;
using Stride.Editor.Design.SceneEditor;
using System.Linq;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.SceneEditor
{
    public class HierarchyItemView : ViewBase<HierarchyItemViewModel>
    {
        public HierarchyItemView(IServiceRegistry services) : base(services)
        {
            dispatcher = Services.GetSafeServiceAs<ICommandDispatcher>();
        }
        private ICommandDispatcher dispatcher;


        public override IViewBuilder CreateView(HierarchyItemViewModel viewModel)
        {
            var tvi = new Virtual.TreeViewItem
            {
                Header = viewModel.Name, // TODO: add folder/entity icon
                // ContextMenu = ... TODO: generate entity context menu
                Tag = viewModel,
                Items = viewModel.Children.Select(c => CreateView(c)),
                IsExpanded = viewModel.IsExpanded,
                IsSelected = viewModel.IsSelected,
                OnSelected = selected => dispatcher.Dispatch(new SelectHierarchyItemCommand(viewModel, selected)),
                OnExpanded = expanded => dispatcher.Dispatch(new ExpandHierarchyItemCommand(viewModel, expanded)),
            };
            return tvi;
        }
    }
}
