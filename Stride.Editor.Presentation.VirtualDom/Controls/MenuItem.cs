using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class MenuItem : ViewBuilder<Avalonia.Controls.MenuItem>
    {
        public string Header
        {
            set { Property(Avalonia.Controls.MenuItem.HeaderProperty, value); }
        }

        public IEnumerable<IViewBuilder> Items
        {
            set { ContentMultiple(Avalonia.Controls.MenuItem.ItemsProperty, value.Select(v => v.Build())); }
        }

        public bool IsEnabled
        {
            set { Property(Avalonia.Controls.MenuItem.IsEnabledProperty, value); }
        }

        public ICommand Command
        {
            set { Property(Avalonia.Controls.MenuItem.CommandProperty, value); }
        }

        public object CommandParameter
        {
            set { Property(Avalonia.Controls.MenuItem.CommandParameterProperty, value); }
        }

        public IViewBuilder Icon
        {
            set { Content(Avalonia.Controls.MenuItem.IconProperty, value.Build()); }
        }
    }
}
