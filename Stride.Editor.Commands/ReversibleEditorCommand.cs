using Stride.Editor.Design;

namespace Stride.Editor.Commands
{
    /// <summary>
    /// Generic wrapper over <see cref="IReversibleEditorCommand{TEditor}"/> to implement the non-generic <see cref="IReversibleCommand"/>.
    /// </summary>
    public sealed class ReversibleEditorCommand<TEditor> : IReversibleCommand where TEditor : class, IAssetEditor
    {
        public ReversibleEditorCommand(ICommandDispatcher dispatcher, IReversibleEditorCommand<TEditor> editorCommand, TEditor editor)
        {
            Dispatcher = dispatcher;
            Command = editorCommand;
            Editor = editor;
        }

        private ICommandDispatcher Dispatcher { get; }
        private IReversibleEditorCommand<TEditor> Command { get; }
        private TEditor Editor { get; }

        public object Context => Editor;

        public void Apply()
        {
            Dispatcher.DispatchDirectApply(Editor, Command);
        }

        public void Undo()
        {
            Dispatcher.DispatchDirectUndo(Editor, Command);
        }
    }
}
