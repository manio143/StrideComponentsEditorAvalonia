using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Commands.Core;
using Stride.Editor.Design.Core;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core.Member
{
    public sealed class StringMemberView : InlineMemberView
    {
        public StringMemberView(IServiceRegistry services) : base(services)
        {
            dispatcher = services.GetSafeServiceAs<ICommandDispatcher>();
        }
        private ICommandDispatcher dispatcher;

        public override bool CanBeApplied(MemberViewModel viewModel)
        {
            return viewModel.TypeDescriptor.Type == typeof(string);
        }

        protected override IViewBuilder CreatePropertyView(MemberViewModel viewModel)
        {
            return new Virtual.TextBox
            {
                Text = (string)viewModel.Value,
                OnText = (value) => dispatcher.DispatchCore(new UpdateMemberValueCommand(viewModel, value)),
            };
        }
    }
}
