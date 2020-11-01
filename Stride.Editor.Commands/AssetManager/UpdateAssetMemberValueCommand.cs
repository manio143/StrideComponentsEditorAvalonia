using Stride.Editor.Design;
using Stride.Editor.Design.Core;
using System;

namespace Stride.Editor.Commands.AssetManager
{
    /// <summary>
    /// Modifies the <see cref="MemberViewModel.Value"/> property and allows reserving the change. A specialized version for AssetEditors that allows tracking asset changes.
    /// </summary>
    public class UpdateAssetMemberValueCommand : IReversibleCommand<UpdateAssetMemberValueCommand.Context>
    {
        /// <summary>
        /// Stateful context that allows swapping <see cref="ViewModel"/>'s value with a new one.
        /// </summary>
        public class Context
        {
            public IAssetManager AssetManager { get; set; }
            public IAssetEditor AssetEditor { get; set; }
            public MemberViewModel ViewModel { get; set; }
            public object Value { get; set; }
            public object OldValue { get; set; }
            public Guid ChangeId { get; } = Guid.NewGuid();
        }

        public void Reverse(Context context)
        {
            context.ViewModel.Value = context.OldValue;
            context.AssetManager.PopChange(context.AssetEditor.Asset, context.ChangeId);
        }

        public void Execute(Context context)
        {
            context.OldValue = context.ViewModel.Value;
            context.ViewModel.Value = context.Value;
            context.AssetManager.PushChange(context.AssetEditor.Asset, context.ChangeId);
        }
    }
}
