using Dock.Model;
using System;

namespace Stride.Editor.Presentation.VirtualDom.Controls.Dock
{
    public class DockControl : ViewBuilder<global::Dock.Avalonia.Controls.DockControl>
    {
        public IDock Layout
        {
            set { Property(global::Dock.Avalonia.Controls.DockControl.LayoutProperty, value); }
        }
    }
}
