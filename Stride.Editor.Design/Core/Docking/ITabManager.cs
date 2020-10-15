using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Design.Core.Docking
{
    /// <summary>
    /// Manager of tabs/windows in the editor.
    /// </summary>
    /// <note>The ITabManager encapsulates mutable state of the docking system.</note>
    public interface ITabManager
    {
        ITabViewModel CreateEditorTab(IAssetEditor editor);

        ITabViewModel CreateToolTab(object viewModel);

        void FocusTab(ITabViewModel tabViewModel);

        void CloseTab(ITabViewModel tabViewModel);
    }
}
