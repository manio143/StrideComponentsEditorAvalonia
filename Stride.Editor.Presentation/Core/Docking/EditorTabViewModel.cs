using Dock.Model.Controls;
using Stride.Editor.Design;
using Stride.Editor.Design.Core;
using Stride.Editor.Design.Core.StringUtils;

namespace Stride.Editor.Presentation.Core.Docking
{
    public class EditorTabViewModel : Document, ITabViewModel
    {
        public EditorTabViewModel(IAssetEditor editor)
        {
            Id = $"{editor.GetType().FullName}:{editor.Asset.Id}";
            Title = $"{editor.AssetName} - {editor.GetType().Name.CamelcaseToSpaces()}";
            ViewModel = editor;
        }

        /// <inheritdoc/>
        public object ViewModel { get; }
    }
}
