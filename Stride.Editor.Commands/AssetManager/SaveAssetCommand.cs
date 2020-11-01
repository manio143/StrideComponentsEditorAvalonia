using Stride.Core.Assets;
using Stride.Editor.Design;

namespace Stride.Editor.Commands.AssetManager
{
    public class SaveAssetCommand : ICommand<SaveAssetCommand.Context>
    {
        public struct Context
        {
            public IAssetManager AssetManager { get; set; }
            public Asset Asset { get; set; }
        }

        public void Execute(Context context)
        {
            context.AssetManager.SaveAsset(context.Asset);
        }
    }
}
