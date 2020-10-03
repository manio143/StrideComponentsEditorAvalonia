using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class ComboBoxItem : ViewBuilder<Avalonia.Controls.ComboBoxItem>
    {
        public object Tag
        {
            set { Property(Avalonia.Controls.ComboBoxItem.TagProperty, value); }
        }

        public new IViewBuilder Content
        {
            set { Content(Avalonia.Controls.ComboBoxItem.ContentProperty, value.Build()); }
        }
    }
}
