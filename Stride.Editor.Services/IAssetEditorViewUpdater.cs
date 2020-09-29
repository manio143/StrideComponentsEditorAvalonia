namespace Stride.Editor.Services
{
    public interface IAssetEditorViewUpdater
    {
        /// <summary>
        /// Invoke a UI update of the specified <paramref name="editor"/>.
        /// </summary>
        void UpdateAssetEditorView<TEditor>(TEditor editor);
    }
}
