using Stride.Editor.Design.SceneEditor;

namespace Stride.Editor.Commands.SceneEditor
{
    /// <summary>
    /// Modifies the <see cref="HierarchyItemViewModel.IsExpanded"/> property.
    /// </summary>
    public class ExpandHierarchyItemCommand : ICommand<ExpandHierarchyItemCommand.Context>
    {
        public struct Context
        {
            public HierarchyItemViewModel ViewModel { get; set; }
            public bool Expand { get; set; }
        }

        public void Execute(Context context)
        {
            context.ViewModel.IsExpanded = context.Expand;
        }
    }
}
