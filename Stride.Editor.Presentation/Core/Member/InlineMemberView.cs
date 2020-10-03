using Avalonia.Layout;
using Stride.Core;
using Stride.Editor.Design.Core;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core.Member
{
    public abstract class InlineMemberView : MemberViewBase
    {
        protected InlineMemberView(IServiceRegistry services) : base(services)
        {
        }

        public override IViewBuilder CreateView(MemberViewModel viewModel)
        {
            var valueProperty = CreatePropertyView(viewModel);
            valueProperty.Property(Layoutable.HorizontalAlignmentProperty, HorizontalAlignment.Right);

            return new Virtual.DockPanel
            {
                Children = new IViewBuilder[]
                {
                    new Virtual.TextBlock
                    {
                        Text = viewModel.Name,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                    },
                    valueProperty,
                }
            };
        }

        protected abstract IViewBuilder CreatePropertyView(MemberViewModel viewModel);
    }
}
