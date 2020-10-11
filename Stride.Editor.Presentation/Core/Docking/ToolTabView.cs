using Stride.Core;

namespace Stride.Editor.Presentation.Core.Docking
{
    public class ToolTabView : ViewBase<ToolTabViewModel>
    {
        public ToolTabView(IServiceRegistry services) : base(services)
        {
            ViewRegistry = Services.GetSafeServiceAs<ViewRegistry>();
        }

        private ViewRegistry ViewRegistry { get; }

        public override IViewBuilder CreateView(ToolTabViewModel viewModel)
        {
            return ViewRegistry.GetView(viewModel.ViewModel, Services).CreateView(viewModel.ViewModel);
        }
    }
}
