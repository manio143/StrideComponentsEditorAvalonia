namespace Stride.Editor.Design
{
    /// <summary>
    /// A class capable of producing a View (in the context of <typeparamref name="TViewObjectBase"/>) for the <typeparamref name="TViewModel"/>.
    /// </summary>
    public interface IView<TViewModel, TViewObjectBase>
    {
        /// <summary>
        /// Creates a new View from the ViewModel.
        /// </summary>
        TViewObjectBase CreateView(TViewModel viewModel);
    }
}
