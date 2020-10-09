using Stride.Editor.Design.SceneEditor;

namespace Stride.Editor.Commands.SceneEditor
{
    /// <summary>
    /// Modifies the <see cref="HierarchyItemViewModel.IsSelected"/> property.
    /// </summary>
    public class SelectHierarchyItemCommand : ICommand<SelectHierarchyItemCommand.Context>
    {
        public struct Context
        {
            public HierarchyItemViewModel ViewModel { get; set; }
            public bool Selected { get; set; }
        }

        public void Execute(Context context)
        {
            context.ViewModel.IsSelected = context.Selected;
        }
    }
}
