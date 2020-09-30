using Avalonia.FuncUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class TreeViewItem : ViewBuilder<Avalonia.Controls.TreeViewItem>
    {
        public IEnumerable<IViewBuilder> Items
        {
            set { ContentMultiple(Avalonia.Controls.TreeViewItem.ItemsProperty, value.Select(vb => vb.Build())); }
        }

        public Action<bool> OnSelected
        {
            set { Subscribe(Avalonia.Controls.TreeViewItem.IsSelectedProperty, value); }
        }

        public Action<bool> OnExpanded
        {
            set { Subscribe(Avalonia.Controls.TreeViewItem.IsExpandedProperty, value); }
        }

        public bool IsSelected
        {
            set { Property(Avalonia.Controls.TreeViewItem.IsSelectedProperty, value); }
        }

        public bool IsExpanded
        {
            set { Property(Avalonia.Controls.TreeViewItem.IsExpandedProperty, value); }
        }

        public object Tag
        {
            set { Property(Avalonia.Controls.TreeViewItem.TagProperty, value); }
        }

        public object Header
        {
            set { Property(Avalonia.Controls.TreeViewItem.HeaderProperty, value); }
        }
    }
}
