using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stride.Editor.Design.Core.Dialogs
{
    public interface IDialogService
    {
        /// <summary>
        /// Opens a model dialog that allows for file selection.
        /// </summary>
        /// <param name="viewModel">Dialog window settings.</param>
        /// <param name="window">Optional: window object which should be the parent of this dialog.</param>
        /// <returns>Paths to selected file</returns>
        Task<string[]> OpenFileDialog(OpenFileDialogViewModel viewModel, object window = null);
    }
}
