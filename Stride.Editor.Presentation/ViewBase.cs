using Stride.Core;
using Stride.Editor.Design;

namespace Stride.Editor.Presentation
{
    public abstract class ViewBase<TViewModel> : IView<TViewModel, IViewBuilder>
    {
        public ViewBase(IServiceRegistry services)
        {
            Services = services;
        }

        public IServiceRegistry Services { get; }

        /// <inheritdoc/>
        public abstract IViewBuilder CreateView(TViewModel viewModel);
    }
}
