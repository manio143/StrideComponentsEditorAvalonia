using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class CheckBox : ViewBuilder<Avalonia.Controls.CheckBox>
    {
        public bool IsVisible
        {
            set { Property(Avalonia.Controls.CheckBox.IsVisibleProperty, value); }
        }

        public bool IsEnabled
        {
            set { Property(Avalonia.Controls.CheckBox.IsEnabledProperty, value); }
        }

        public Action<bool> OnEnabled
        {
            set { Subscribe(Avalonia.Controls.CheckBox.IsEnabledProperty, value); }
        }
    }
}
