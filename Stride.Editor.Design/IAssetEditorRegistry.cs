using Stride.Core.Assets;

namespace Stride.Editor.Design
{
    /// <summary>
    /// Registry of asset editors.
    /// </summary>
    public interface IAssetEditorRegistry
    {
        /// <summary>
        /// Registers the editor for the specified asset. The editor has to have a constructor <typeparamref name="TAssetEditor"/>(<typeparamref name="TAsset"/> asset, <see cref="Stride.Core.IServiceRegistry"/> services).
        /// </summary>
        /// <remarks>First editor registered for <typeparamref name="TAsset"/> is the default editor.</remarks>
        /// <exception cref="System.InvalidOperationException">Already registered.</exception>
        void RegisterAssetEditor<TAsset, TAssetEditor>()
            where TAsset : Asset
            where TAssetEditor : IAssetEditor;

        /// <summary>
        /// Unregisters the editor.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Has not been registered.</exception>
        void UnregisterAssetEditor<TAssetEditor>()
            where TAssetEditor : IAssetEditor;
    }
}
