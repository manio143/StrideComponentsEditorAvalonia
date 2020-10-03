using Avalonia.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class DockPanel : PanelViewBuilder<Avalonia.Controls.DockPanel>
    {
        public IEnumerable<IViewBuilder> Children
        {
            set { ContentMultiple(nameof(Children), value.Select(v => v.Build())); }
        }
    }
}
