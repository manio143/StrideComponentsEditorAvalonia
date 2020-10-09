using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Commands.SceneEditor;
using Stride.Editor.Design.SceneEditor;
using System.Linq;
using Avalonia.Controls;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.SceneEditor
{
    public class SceneHierarchyView : ViewBase<SceneViewModel>
    {
        public SceneHierarchyView(IServiceRegistry services) : base(services)
        {
            dispatcher = Services.GetSafeServiceAs<ICommandDispatcher>();
        }

        private ICommandDispatcher dispatcher;

        public override IViewBuilder CreateView(SceneViewModel scene)
        {
            var itemView = new HierarchyItemView(Services);

            var tree = new Virtual.TreeView
            {
                Items = scene.Items.Select(item => itemView.CreateView(item)),
                SelectionMode = SelectionMode.Multiple,
                AutoScrollToSelectedItem = true,
                OnSelectedItems = TreeView_SelectedItemChanged
            };
            return tree;
        }

        private void TreeView_SelectedItemChanged(object item)
        {
            if (item is TreeViewItem viewItem)
            {
                var hierarchyItem = (HierarchyItemViewModel)viewItem.Tag;

                if (hierarchyItem.IsFolder)
                    return;

                var entityViewModel = (EntityViewModel)hierarchyItem;
                dispatcher.DispatchToActiveEditor(new SelectEntityCommand(), entityViewModel);
            }
        }
    }
}
