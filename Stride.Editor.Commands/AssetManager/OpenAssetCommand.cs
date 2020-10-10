using Stride.Core.Assets;
using Stride.Editor.Design;

namespace Stride.Editor.Commands.AssetManager
{
    /// <summary>
    /// Opens asset via the <see cref="IAssetManager"/>.
    /// </summary>
    public class OpenAssetCommand : ICommand<OpenAssetCommand.Context>
    {
        public struct Context
        {
            public IAssetManager AssetManager { get; set; }
            public AssetItem AssetItem { get; set; }
        }

        public void Execute(Context context)
        {
            context.AssetManager.OpenAsset(context.AssetItem);
        }
    }
}
