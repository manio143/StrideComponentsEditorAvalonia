using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class ItemsControl : ViewBuilder<Avalonia.Controls.ItemsControl>
    {
        public IEnumerable<IViewBuilder> Items
        {
            set { ContentMultiple(Avalonia.Controls.ItemsControl.ItemsProperty, value.Select(v => v.Build())); }
        }
    }
}
