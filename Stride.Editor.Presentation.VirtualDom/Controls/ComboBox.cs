using Avalonia.FuncUI.DSL;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class ComboBox : ViewBuilder<Avalonia.Controls.ComboBox>
    {
        public IEnumerable<IViewBuilder> Items
        {
            set { ContentMultiple(Avalonia.Controls.ComboBox.ItemsProperty, value.Select(v => v.Build())); }
        }

        public int SelectedIndex
        {
            set { Property(Avalonia.Controls.ComboBox.SelectedIndexProperty, value); }
        }

        public Action<int> OnSelected
        {
            set { Subscribe(Avalonia.Controls.ComboBox.SelectedIndexProperty, value, SubPatchOptions.Always); }
        }
    }
}
