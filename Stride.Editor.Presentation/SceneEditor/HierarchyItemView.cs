using Avalonia.Controls;
using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Commands.SceneEditor;
using Stride.Editor.Design.SceneEditor;
using System.Linq;

namespace Stride.Editor.Presentation.SceneEditor
{
    public class HierarchyItemView : ViewBase<HierarchyItemViewModel>
    {
        public HierarchyItemView(IServiceRegistry services) : base(services)
        {
            dispatcher = Services.GetSafeServiceAs<ICommandDispatcher>();
        }
        private ICommandDispatcher dispatcher;


        public override IControl CreateView(HierarchyItemViewModel viewModel)
        {
            var tvi = new TreeViewItem
            {
                Header = viewModel.Name, // TODO: add folder/entity icon
                // ContextMenu = ... TODO: generate entity context menu
                Tag = viewModel,
                Items = viewModel.Children.Select(c => CreateView(c)),
                IsExpanded = viewModel.IsExpanded,
                IsSelected = viewModel.IsSelected,
            };
            tvi.PropertyChanged += Tvi_PropertyChanged;
            return tvi;
        }

        private void Tvi_PropertyChanged(object sender, Avalonia.AvaloniaPropertyChangedEventArgs e)
        {
            var tvi = (TreeViewItem)sender;
            if (e.Property == TreeViewItem.IsExpandedProperty)
                dispatcher.Dispatch(new ExpandHierarchyItemCommand(tvi.Tag as HierarchyItemViewModel, (bool)e.NewValue));
            else if (e.Property == TreeViewItem.IsSelectedProperty)
                dispatcher.Dispatch(new SelectHierarchyItemCommand(tvi.Tag as HierarchyItemViewModel, (bool)e.NewValue));
        }
    }
}
