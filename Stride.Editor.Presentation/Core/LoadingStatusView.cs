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
            var bar = new Virtual.ProgressBar
            {
                IsIndeterminate = viewModel.Mode == LoadingStatus.LoadingMode.Indeterminate,
                Minimum = 0,
                Maximum = 100,
                Value = viewModel.PercentCompleted,
            };
            var container = new Virtual.ContentControl
            {
                Width = 100, // TODO: this might need to be configurable if LoadingStatus is to be reused outside of the status bar
                Margin = new Avalonia.Thickness(4),
                Content = bar,
            };
            var message = new Virtual.TextBlock
            {
                Text = viewModel.Message,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            };
            return new Virtual.StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Children = new IViewBuilder[]
                {
                    container,
                    message,
                }
            };
        }
    }
}
