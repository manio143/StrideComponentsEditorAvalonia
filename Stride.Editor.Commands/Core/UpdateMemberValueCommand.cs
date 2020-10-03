using Stride.Editor.Design.Core;

namespace Stride.Editor.Commands.Core
{
    public class UpdateMemberValueCommand : IReversibleCommand
    {
        public UpdateMemberValueCommand(MemberViewModel viewModel, object value)
        {
            ViewModel = viewModel;
            Value = value;
        }

        private MemberViewModel ViewModel { get; }
        private object Value { get; }
        private object OldValue { get; set; }

        public object Context { get; set; }

        public void Apply()
        {
            OldValue = ViewModel.Value;
            ViewModel.Value = Value;
        }

        public void Undo()
        {
            ViewModel.Value = OldValue;
        }
    }
}
