using Avalonia.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class StackPanel : PanelViewBuilder<Avalonia.Controls.StackPanel>
    {
        public IEnumerable<IViewBuilder> Children
        {
            set { ContentMultiple(nameof(Children), value.Select(v => v.Build())); }
        }

        public Orientation Orientation
        {
            set { Property(Avalonia.Controls.StackPanel.OrientationProperty, value); }
        }
    }
}
