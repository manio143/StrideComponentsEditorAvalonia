using Stride.Editor.Design.SceneEditor;

namespace Stride.Editor.Commands.SceneEditor
{
    public class ExpandEntityComponentCommand : IEditorCommand<Design.SceneEditor.SceneEditor>
    {
        public ExpandEntityComponentCommand(EntityComponentViewModel viewModel, bool expand)
        {
            ViewModel = viewModel;
            Expand = expand;
        }

        private EntityComponentViewModel ViewModel { get; }
        private bool Expand { get; }

        public void Apply(Design.SceneEditor.SceneEditor editor)
        {
            ViewModel.IsExpanded = Expand;
        }
    }
}
