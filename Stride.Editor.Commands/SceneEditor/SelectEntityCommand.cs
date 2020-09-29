using Stride.Editor.Design.SceneEditor;

namespace Stride.Editor.Commands.SceneEditor
{
    public class SelectEntityCommand : IEditorCommand<Design.SceneEditor.SceneEditor>
    {
        public EntityViewModel EntityViewModel { get; }

        public SelectEntityCommand(EntityViewModel entityViewModel)
        {
            EntityViewModel = entityViewModel;
        }

        public void Apply(Design.SceneEditor.SceneEditor editor)
        {
            editor.SelectedEntity = EntityViewModel;
        }
    }
}
