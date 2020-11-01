using Stride.Core;
using Stride.Editor.Design.Core;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core.Member
{
    public sealed class BoolMemberView : InlineMemberView
    {
        public BoolMemberView(IServiceRegistry services) : base(services)
        {
        }

        public override bool CanBeApplied(MemberViewModel viewModel)
        {
            return viewModel.TypeDescriptor.Type == typeof(bool);
        }

        protected override IViewBuilder CreatePropertyView(MemberViewModel viewModel)
        {
            return new Virtual.CheckBox
            {
                IsChecked = (bool)viewModel.Value,
                OnChecked = CreateUpdate<bool?>(viewModel, v => v ?? false),
            };
        }
    }
}
