using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Commands.Core;
using Stride.Editor.Design.Core;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core.Member
{
    public sealed class CharMemberView : InlineMemberView
    {
        public CharMemberView(IServiceRegistry services) : base(services)
        {
            dispatcher = services.GetSafeServiceAs<ICommandDispatcher>();
        }
        private ICommandDispatcher dispatcher;

        public override bool CanBeApplied(MemberViewModel viewModel)
        {
            return viewModel.TypeDescriptor.Type == typeof(char);
        }

        protected override IViewBuilder CreatePropertyView(MemberViewModel viewModel)
        {
            return new Virtual.TextBox
            {
                Text = new string((char)viewModel.Value, 1),
                OnText = (value) => dispatcher.DispatchCore(new UpdateMemberValueCommand(viewModel, value[0])),
                MaxLength = 1,
            };
        }
    }
}
