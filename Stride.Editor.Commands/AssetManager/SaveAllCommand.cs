using Stride.Editor.Design;

namespace Stride.Editor.Commands.AssetManager
{
    public class SaveAllCommand : ICommand<SaveAllCommand.Context>
    {
        public struct Context
        {
            public IAssetManager AssetManager { get; set; }
        }

        public void Execute(Context context)
        {
            context.AssetManager.SaveAll();
        }
    }
}
