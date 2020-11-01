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
            var label = new Virtual.TextBlock
            {
                Text = viewModel.Name,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            };

            var valueProperty = CreatePropertyView(viewModel);
            valueProperty.Property(Layoutable.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            valueProperty.Property(Avalonia.Controls.Grid.ColumnProperty, 1);

            return new Virtual.Grid
            {
                ColumnDefinitions = new Avalonia.Controls.ColumnDefinitions("*,*"),
                RowDefinitions = new Avalonia.Controls.RowDefinitions("*"),
                Children = new IViewBuilder[]
                {
                    label,
                    valueProperty,
                }
            };
        }

        protected abstract IViewBuilder CreatePropertyView(MemberViewModel viewModel);
    }
}
