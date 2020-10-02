using System.Collections.Generic;
using System.Linq;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class Menu : ViewBuilder<Avalonia.Controls.Menu>
    {
        public IEnumerable<IViewBuilder> Items
        {
            set { ContentMultiple(Avalonia.Controls.Menu.ItemsProperty, value.Select(v => v.Build())); }
        }
    }
}
