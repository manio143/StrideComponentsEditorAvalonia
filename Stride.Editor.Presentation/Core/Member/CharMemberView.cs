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
        }

        public override bool CanBeApplied(MemberViewModel viewModel)
        {
            return viewModel.TypeDescriptor.Type == typeof(char);
        }

        protected override IViewBuilder CreatePropertyView(MemberViewModel viewModel)
        {
            return new Virtual.TextBox
            {
                Text = new string((char)viewModel.Value, 1),
                OnText = CreateUpdate<string>(viewModel, v => v[0]),
                MaxLength = 1,
            };
        }
    }
}
