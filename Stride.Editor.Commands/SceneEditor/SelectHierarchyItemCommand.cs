using Stride.Editor.Design.SceneEditor;

namespace Stride.Editor.Commands.SceneEditor
{
    public class SelectHierarchyItemCommand : IEditorCommand<Design.SceneEditor.SceneEditor>
    {
        public SelectHierarchyItemCommand(HierarchyItemViewModel hierarchyItem, bool selected)
        {
            HierarchyItem = hierarchyItem;
            Selected = selected;
        }

        private HierarchyItemViewModel HierarchyItem { get; }
        private bool Selected { get; }

        public void Apply(Design.SceneEditor.SceneEditor editor)
        {
            HierarchyItem.IsSelected = Selected;
        }
    }
}
