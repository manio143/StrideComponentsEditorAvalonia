using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Commands.Core;
using Stride.Editor.Design.Core;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core.Member
{
    public sealed class BoolMemberView : InlineMemberView
    {
        public BoolMemberView(IServiceRegistry services) : base(services)
        {
            dispatcher = services.GetSafeServiceAs<ICommandDispatcher>();
        }
        private ICommandDispatcher dispatcher;

        public override bool CanBeApplied(MemberViewModel viewModel)
        {
            return viewModel.TypeDescriptor.Type == typeof(bool);
        }

        protected override IViewBuilder CreatePropertyView(MemberViewModel viewModel)
        {
            return new Virtual.CheckBox
            {
                IsChecked = (bool)viewModel.Value,
                OnChecked = (value) => 
                    dispatcher.Dispatch(
                        new UpdateMemberValueCommand(), 
                        new UpdateMemberValueCommand.Context
                        {
                            ViewModel = viewModel,
                            Value = value,
                        }),
            };
        }
    }
}
