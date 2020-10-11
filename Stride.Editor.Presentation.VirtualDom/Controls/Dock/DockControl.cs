using Dock.Model;
using System;

namespace Stride.Editor.Presentation.VirtualDom.Controls.Dock
{
    public class DockControl : ViewBuilder<global::Dock.Avalonia.Controls.DockControl>
    {
        public DockControl(Func<IDock, IDock, bool> dockComparer)
        {
            DockComparer = dockComparer;
        }

        public IDock Layout
        {
            set { Property(global::Dock.Avalonia.Controls.DockControl.LayoutProperty, value, DockComparer); }
        }

        public Func<IDock, IDock, bool> DockComparer;
    }
}
