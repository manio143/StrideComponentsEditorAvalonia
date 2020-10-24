using System.Threading.Tasks;

namespace Stride.Editor.Design.Core.Docking
{
    /// <summary>
    /// Manager of tabs/windows in the editor.
    /// </summary>
    /// <note>The ITabManager encapsulates mutable state of the docking system.</note>
    public interface ITabManager
    {
        /// <summary>
        /// Creates a new 'Editor tab' with the specified <paramref name="viewModel"/> and adds it to the <see cref="EditorViewModel"/>.
        /// </summary>
        Task<ITabViewModel> CreateEditorTab(IAssetEditor editor);

        /// <summary>
        /// Creates a new 'Tool tab' with the specified <paramref name="viewModel"/> and adds it to the <see cref="EditorViewModel"/>.
        /// </summary>
        Task<ITabViewModel> CreateToolTab(object viewModel);

        /// <summary>
        /// Sets focus to the specified <paramref name="tabViewModel"/>.
        /// </summary>
        Task FocusTab(ITabViewModel tabViewModel);

        /// <summary>
        /// Closes the specified tab and removes it from the <see cref="EditorViewModel"/>.
        /// </summary>
        Task CloseTab(ITabViewModel tabViewModel);
    }
}
