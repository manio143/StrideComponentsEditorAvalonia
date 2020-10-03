using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class Slider : ViewBuilder<Avalonia.Controls.Slider>
    {
        public double Value
        {
            set { Property(Avalonia.Controls.Slider.ValueProperty, value); }
        }

        public double Minimum
        {
            set { Property(Avalonia.Controls.Slider.MinimumProperty, value); }
        }

        public double Maximum
        {
            set { Property(Avalonia.Controls.Slider.MaximumProperty, value); }
        }

        public Action<double> OnValue
        {
            set { Subscribe(Avalonia.Controls.Slider.ValueProperty, value); }
        }

        public double SmallChange
        {
            set { Property(Avalonia.Controls.Slider.SmallChangeProperty, value); }
        }

        public double LargeChange
        {
            set { Property(Avalonia.Controls.Slider.LargeChangeProperty, value); }
        }
    }
}
