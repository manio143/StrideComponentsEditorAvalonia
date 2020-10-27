using Avalonia.FuncUI.DSL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class TabItem : ViewBuilder<Avalonia.Controls.TabItem>
    {
        public new IViewBuilder Content
        {
            set { Content(Avalonia.Controls.TabItem.ContentProperty, value.Build()); }
        }

        public string Header
        {
            set { Property(Avalonia.Controls.TabItem.HeaderProperty, value); }
        }

        public bool IsSelected
        {
            set { Property(Avalonia.Controls.TabItem.IsSelectedProperty, value); }
        }

        public Action OnSelected
        {
            set { Subscribe(Avalonia.Controls.TabItem.IsSelectedProperty, (v) => { if (v) value(); }, SubPatchOptions.Always); }
        }
    }
}
