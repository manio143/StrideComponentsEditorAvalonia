namespace Stride.Editor.Design.Core
{
    public class TabViewModel<TViewModel> : ITabViewModel
    {
        /// <summary>
        /// ViewModel associated with the contents of this tab.
        /// </summary>
        public TViewModel ViewModel { get; }
        object ITabViewModel.ViewModel => this.ViewModel;

        public TabViewModel(TViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        /// <inheritdoc/>
        public object ViewLayout { get; set; }
    }
}