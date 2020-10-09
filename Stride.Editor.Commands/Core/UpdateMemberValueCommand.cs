using Stride.Editor.Design.Core;

namespace Stride.Editor.Commands.Core
{
    /// <summary>
    /// Modifies the <see cref="MemberViewModel.Value"/> property and allows reserving the change.
    /// </summary>
    public class UpdateMemberValueCommand : IReversibleCommand<UpdateMemberValueCommand.Context>
    {
        /// <summary>
        /// Stateful context that allows swapping <see cref="ViewModel"/>'s value with a new one.
        /// </summary>
        public class Context
        {
            public MemberViewModel ViewModel { get; set; }
            public object Value { get; set; }
            public object OldValue { get; set; }
        }

        public void Reverse(Context context)
        {
            context.ViewModel.Value = context.OldValue;
        }

        public void Execute(Context context)
        {
            context.OldValue = context.ViewModel.Value;
            context.ViewModel.Value = context.Value;
        }
    }
}
