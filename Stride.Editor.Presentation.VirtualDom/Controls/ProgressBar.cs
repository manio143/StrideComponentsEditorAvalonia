using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class ProgressBar : ViewBuilder<Avalonia.Controls.ProgressBar>
    {
        public bool IsIndeterminate
        {
            set { Property(Avalonia.Controls.ProgressBar.IsIndeterminateProperty, value); }
        }

        public double Minimum
        {
            set { Property(Avalonia.Controls.ProgressBar.MinimumProperty, value); }
        }

        public double Maximum
        {
            set { Property(Avalonia.Controls.ProgressBar.MaximumProperty, value); }
        }

        public double Value
        {
            set { Property(Avalonia.Controls.ProgressBar.ValueProperty, value); }
        }
    }
}
