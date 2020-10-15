using System.Threading.Tasks;

namespace Stride.Editor.Design.Core.Docking
{
    /// <summary>
    /// Manager of tabs/windows in the editor.
    /// </summary>
    /// <note>The ITabManager encapsulates mutable state of the docking system.</note>
    public interface ITabManager
    {
        Task<ITabViewModel> CreateEditorTab(IAssetEditor editor);

        Task<ITabViewModel> CreateToolTab(object viewModel);

        Task FocusTab(ITabViewModel tabViewModel);

        Task CloseTab(ITabViewModel tabViewModel);
    }
}
