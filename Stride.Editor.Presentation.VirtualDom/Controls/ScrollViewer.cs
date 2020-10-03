using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class ScrollViewer : ViewBuilder<Avalonia.Controls.ScrollViewer>
    {
        public new IViewBuilder Content
        {
            set { Content(Avalonia.Controls.ScrollViewer.ContentProperty, value?.Build()); }
        }
    }
}
