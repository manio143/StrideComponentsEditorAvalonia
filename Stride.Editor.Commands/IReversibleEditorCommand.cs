using Stride.Editor.Design;

namespace Stride.Editor.Commands
{
    public interface IReversibleEditorCommand<TEditor> : IEditorCommand<TEditor> where TEditor : IAssetEditor
    {
        /// <summary>
        /// Reverses the effect of <see cref="IEditorCommand{TEditor}.Apply"/> on the <paramref name="editor"/> state.
        /// </summary>
        void Undo(TEditor editor);
    }
}
