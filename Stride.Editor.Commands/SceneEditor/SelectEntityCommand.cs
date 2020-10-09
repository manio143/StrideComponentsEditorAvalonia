using Stride.Editor.Design.SceneEditor;

namespace Stride.Editor.Commands.SceneEditor
{
    /// <summary>
    /// Modifies the <see cref="Design.SceneEditor.SceneEditor.SelectedEntity"/> property.
    /// </summary>
    public class SelectEntityCommand : ICommand<ContextWithActiveEditor<EntityViewModel>>
    {
        public void Execute(ContextWithActiveEditor<EntityViewModel> context)
        {
            (context.ActiveEditor as Design.SceneEditor.SceneEditor)
                .SelectedEntity = context.Context;
        }
    }
}
