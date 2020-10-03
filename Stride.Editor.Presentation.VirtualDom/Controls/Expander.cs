using Avalonia.FuncUI.DSL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class Expander : ViewBuilder<Avalonia.Controls.Expander>
    {
        public IViewBuilder Header
        {
            set { Content(Avalonia.Controls.Expander.HeaderProperty, value.Build()); }
        }

        public new IViewBuilder Content
        {
            set { Content(Avalonia.Controls.Expander.ContentProperty, value.Build()); }
        }

        public bool IsExpanded
        {
            set { Property(Avalonia.Controls.Expander.IsExpandedProperty, value); }
        }

        public Action<bool> OnExpanded
        {
            set { Subscribe(Avalonia.Controls.Expander.IsExpandedProperty, value, SubPatchOptions.Always); }
        }
    }
}
