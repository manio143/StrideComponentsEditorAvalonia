using Stride.Core.Assets;
using System;

namespace Stride.Editor.Design
{
    public interface IAssetManager
    {
        /// <summary>
        /// Opens a new editor for <paramref name="asset"/> or focuses on an existing one.
        /// </summary>
        /// <param name="editorType">Optional: type of the editor to open the <paramref name="asset"/> with. If null, default editor will be selected.</param>
        public void OpenAsset(AssetItem asset, Type editorType = null);

        /// <summary>
        /// Registers a change to the asset being edited.
        /// </summary>
        public void PushChange(Asset asset, Guid changeId);
        
        /// <summary>
        /// Reverses a change to the asset being edited.
        /// </summary>
        public void PopChange(Asset asset, Guid changeId);
    }
}
