using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class NumericUpDown : ViewBuilder<Avalonia.Controls.NumericUpDown>
    {
        public double Value
        {
            set { Property(Avalonia.Controls.NumericUpDown.ValueProperty, value); }
        }

        public double Minimum
        {
            set { Property(Avalonia.Controls.NumericUpDown.MinimumProperty, value); }
        }

        public double Maximum
        {
            set { Property(Avalonia.Controls.NumericUpDown.MaximumProperty, value); }
        }

        public Action<Avalonia.Controls.NumericUpDownValueChangedEventArgs> OnValue
        {
            set { Subscribe(Avalonia.Controls.NumericUpDown.ValueChangedEvent, value); }
        }

        public string FormatString
        {
            set { Property(Avalonia.Controls.NumericUpDown.FormatStringProperty, value); }
        }
    }
}
