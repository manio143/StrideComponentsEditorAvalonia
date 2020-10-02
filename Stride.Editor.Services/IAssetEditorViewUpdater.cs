using Stride.Editor.Design;
using System.Threading.Tasks;

namespace Stride.Editor.Services
{
    public interface IAssetEditorViewUpdater
    {
        /// <summary>
        /// Invoke a UI update of the specified <paramref name="editor"/>.
        /// </summary>
        Task UpdateAssetEditorView(IAssetEditor editor);
    }
}
