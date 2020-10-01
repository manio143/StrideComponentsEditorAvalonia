using Stride.Editor.Design.SceneEditor;

namespace Stride.Editor.Commands.SceneEditor
{
    public class EnableEntityComponentCommand : IReversibleEditorCommand<Design.SceneEditor.SceneEditor>
    {
        public EnableEntityComponentCommand(EntityComponentViewModel viewModel, bool enable)
        {
            ViewModel = viewModel;
            Enable = enable;
        }

        private EntityComponentViewModel ViewModel { get; }
        private bool Enable { get; }

        public void Apply(Design.SceneEditor.SceneEditor editor)
        {
            ViewModel.IsEnabled = Enable;
        }

        public void Undo(Design.SceneEditor.SceneEditor editor)
        {
            ViewModel.IsEnabled = !Enable;
        }
    }
}
