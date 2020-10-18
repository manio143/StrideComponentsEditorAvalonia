using Avalonia.LogicalTree;
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

        // TODO: dispatch a command and return false
        public override bool OnClose()
        {
            return base.OnClose();
        }

        /// <inheritdoc/>
        public object ViewModel { get; }

        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromLogicalTree(e);
            RequiresViewRefresh = true;
        }

        public bool RequiresViewRefresh { get; set; }
    }
}
