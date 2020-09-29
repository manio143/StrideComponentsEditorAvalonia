using Avalonia.Controls;
using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Commands.SceneEditor;
using Stride.Editor.Design.SceneEditor;
using System.Linq;

namespace Stride.Editor.Presentation.SceneEditor
{
    public class SceneHierarchyView : ViewBase<SceneViewModel>
    {
        public SceneHierarchyView(IServiceRegistry services) : base(services)
        {
            dispatcher = Services.GetSafeServiceAs<ICommandDispatcher>();
        }

        private ICommandDispatcher dispatcher;

        public override IControl CreateView(SceneViewModel scene)
        {
            var itemView = new HierarchyItemView(Services);
            var tree = new TreeView
            {
                Items = scene.Items.Select(item => itemView.CreateView(item)),
                SelectionMode = SelectionMode.Multiple,
                AutoScrollToSelectedItem = true,
            };
            tree.SelectionChanged += Tree_SelectionChanged;
            return tree;
        }

        private void Tree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                var viewItem = (TreeViewItem)e.AddedItems[0];
                var hierarchyItem = (HierarchyItemViewModel)viewItem.Tag;

                if (hierarchyItem.IsFolder)
                    return;

                var entityViewModel = (EntityViewModel)hierarchyItem;
                dispatcher.Dispatch(new SelectEntityCommand(entityViewModel));
            }
        }
    }
}
