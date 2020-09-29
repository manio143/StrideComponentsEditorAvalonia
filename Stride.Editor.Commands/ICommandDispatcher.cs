using Stride.Editor.Design;

namespace Stride.Editor.Commands
{
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Dispatch a command to be applied to the active editor of type <typeparamref name="TEditor"/>.
        /// </summary>
        void Dispatch<TEditor>(IEditorCommand<TEditor> editorCommand)
            where TEditor : class, IAssetEditor;
    }
}
