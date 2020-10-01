using Stride.Editor.Design;

namespace Stride.Editor.Commands
{
    public interface IEditorCommand<TEditor> where TEditor : IAssetEditor
    {
        /// <summary>
        /// Applies command to the <paramref name="editor"/> state.
        /// </summary>
        void Apply(TEditor editor);
    }
}
