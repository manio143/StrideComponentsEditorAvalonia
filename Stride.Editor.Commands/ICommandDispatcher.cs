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

        /// <summary>
        /// Dispatch a command to be applied to <paramref name="editor"/>.
        /// </summary>
        /// <remarks>
        /// Direct dispatch will not register the command in undo/redo service, as it should've already been registered.
        /// </remarks>
        void DispatchDirectApply<TEditor>(TEditor editor, IEditorCommand<TEditor> editorCommand)
            where TEditor : class, IAssetEditor;

        /// <summary>
        /// Dispatch a command to be undone to <paramref name="editor"/>.
        /// </summary>
        /// <remarks>
        /// Direct dispatch will not register the command in undo/redo service, as it should've already been registered.
        /// </remarks>
        void DispatchDirectUndo<TEditor>(TEditor editor, IReversibleEditorCommand<TEditor> editorCommand)
            where TEditor : class, IAssetEditor;
    }
}
