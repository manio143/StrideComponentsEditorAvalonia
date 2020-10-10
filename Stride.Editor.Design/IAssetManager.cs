using Stride.Core.Assets;

namespace Stride.Editor.Design
{
    public interface IAssetManager
    {
        /// <summary>
        /// Opens a new editor for <paramref name="asset"/> or focuses on an existing one.
        /// </summary>
        public void OpenAsset(AssetItem asset);
    }
}
