using System.Collections.Generic;
using System.Linq;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class TabControl : ViewBuilder<Avalonia.Controls.TabControl>
    {
        public IEnumerable<IViewBuilder> Items
        {
            set { ContentMultiple(Avalonia.Controls.TabControl.ItemsProperty, value.Select(val => val.Build())); }
        }
    }
}
