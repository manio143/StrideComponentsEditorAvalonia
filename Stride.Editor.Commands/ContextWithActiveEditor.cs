using Stride.Editor.Design;

namespace Stride.Editor.Commands
{
    public class ContextWithActiveEditor<TContext>
    {
        public IAssetEditor ActiveEditor { get; set; }
        public TContext Context { get; set; }
    }
}
