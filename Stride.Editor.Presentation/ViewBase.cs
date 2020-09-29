using Avalonia.Controls;
using Stride.Core;
using Stride.Editor.Design;

namespace Stride.Editor.Presentation
{
    public abstract class ViewBase<TViewModel> : IView<TViewModel, IControl>
    {
        public ViewBase(IServiceRegistry services)
        {
            Services = services;
        }

        public IServiceRegistry Services { get; }

        /// <inheritdoc/>
        public abstract IControl CreateView(TViewModel viewModel);
    }
}
