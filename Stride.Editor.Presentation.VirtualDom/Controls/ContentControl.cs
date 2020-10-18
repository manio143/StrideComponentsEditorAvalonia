using Avalonia;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class ContentControl : ViewBuilder<Avalonia.Controls.ContentControl>
    {
        public new IViewBuilder Content
        {
            set { Content(Avalonia.Controls.ContentControl.ContentProperty, value.Build()); }
        }

        public int Width
        {
            set { Property(Avalonia.Controls.ContentControl.WidthProperty, value); }
        }

        public Thickness Margin
        {
            set { Property(Avalonia.Controls.ContentControl.MarginProperty, value); }
        }
    }
}
