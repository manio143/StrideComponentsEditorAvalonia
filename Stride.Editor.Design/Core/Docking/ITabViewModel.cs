namespace Stride.Editor.Design.Core
{
    public interface ITabViewModel
    {
        /// <summary>
        /// ViewModel associated with the contents of this tab.
        /// </summary>
        object ViewModel { get; }
    }
}