using System.Collections.Generic;
using System.Linq;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class DockPanel : PanelViewBuilder<Avalonia.Controls.DockPanel>
    {
        public IEnumerable<IViewBuilder> Children
        {
            set { ContentMultiple(nameof(Children), value.Select(v => v.Build())); }
        }

        public int MinHeight
        {
            set { Property(Avalonia.Controls.DockPanel.MinHeightProperty, value); }
        }
    }
}
