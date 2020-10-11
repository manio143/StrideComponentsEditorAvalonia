using Stride.Core;

namespace Stride.Editor.Presentation.Core.Docking
{
    public class EditorTabView : ViewBase<EditorTabViewModel>
    {
        public EditorTabView(IServiceRegistry services) : base(services)
        {
            ViewRegistry = Services.GetSafeServiceAs<ViewRegistry>();
        }

        private ViewRegistry ViewRegistry { get; }

        public override IViewBuilder CreateView(EditorTabViewModel viewModel)
        {
            return ViewRegistry.GetView(viewModel.ViewModel, Services).CreateView(viewModel.ViewModel);
        }
    }
}
