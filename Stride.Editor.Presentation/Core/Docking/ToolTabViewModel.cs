using Avalonia.LogicalTree;
using Dock.Model.Controls;
using Stride.Editor.Design.Core;
using Stride.Editor.Design.Core.StringUtils;

namespace Stride.Editor.Presentation.Core.Docking
{
    public class ToolTabViewModel : Tool, ITabViewModel
    {
        public ToolTabViewModel(object tool)
        {
            Id = $"{tool.GetType().FullName}";
            Title = $"{tool.GetType().Name.CamelcaseToSpaces()}";
            ViewModel = tool;
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
