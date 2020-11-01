using Stride.Core.Assets;
using Stride.Editor.Design;
using Stride.Editor.Design.SceneEditor;
using System;

namespace Stride.Editor.Commands.SceneEditor
{
    /// <summary>
    /// Modifies the <see cref="EntityComponentViewModel.IsEnabled"/> property.
    /// </summary>
    public class EnableEntityComponentCommand : IReversibleCommand<EnableEntityComponentCommand.Context>
    {
        public class Context
        {
            public EntityComponentViewModel ViewModel { get; set; }
            public bool Enable { get; set; }
            public IAssetManager AssetManager { get; set; }
            public Asset Asset { get; set; }
            public Guid ChangeId { get; } = Guid.NewGuid();
        }

        public void Reverse(Context context)
        {
            context.ViewModel.IsEnabled = !context.Enable;
            context.AssetManager.PopChange(context.Asset, context.ChangeId);
        }

        public void Execute(Context context)
        {
            context.ViewModel.IsEnabled = context.Enable;
            context.AssetManager.PushChange(context.Asset, context.ChangeId);
        }
    }
}
