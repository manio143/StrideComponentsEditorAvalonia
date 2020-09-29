using Stride.Editor.Design.SceneEditor;

namespace Stride.Editor.Commands.SceneEditor
{
    public class ExpandHierarchyItemCommand : IEditorCommand<Design.SceneEditor.SceneEditor>
    {
        public ExpandHierarchyItemCommand(HierarchyItemViewModel hierarchyItem, bool expanded)
        {
            HierarchyItem = hierarchyItem;
            Expanded = expanded;
        }

        private HierarchyItemViewModel HierarchyItem { get; }
        private bool Expanded { get; }

        public void Apply(Design.SceneEditor.SceneEditor editor)
        {
            HierarchyItem.IsExpanded = Expanded;
        }
    }
}
