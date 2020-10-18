using Stride.Core;
using Stride.Editor.Design.Core;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core
{
    public class LoadingStatusView : ViewBase<LoadingStatus>
    {
        public LoadingStatusView(IServiceRegistry services) : base(services)
        {
        }

        public override IViewBuilder CreateView(LoadingStatus viewModel)
        {
            return new Virtual.ProgressBar
            {
                IsIndeterminate = viewModel.Mode == LoadingStatus.LoadingMode.Indeterminate,
                Minimum = 0,
                Maximum = 100,
                Value = viewModel.PercentCompleted,
            };
        }
    }
}
