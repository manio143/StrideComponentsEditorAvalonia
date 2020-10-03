using Avalonia.Layout;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class TextBlock : ViewBuilder<Avalonia.Controls.TextBlock>
    {
        public string Text
        {
            set { Property(Avalonia.Controls.TextBlock.TextProperty, value); }
        }

        public VerticalAlignment VerticalAlignment
        {
            set { Property(Avalonia.Controls.TextBlock.VerticalAlignmentProperty, value); }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            set { Property(Avalonia.Controls.TextBlock.HorizontalAlignmentProperty, value); }
        }

        public bool IsEnabled
        {
            set { Property(Avalonia.Controls.TextBlock.IsEnabledProperty, value); }
        }
    }
}
