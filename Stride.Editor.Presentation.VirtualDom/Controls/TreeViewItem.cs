using Avalonia.FuncUI;
using Avalonia.FuncUI.DSL;
using Avalonia.Interactivity;
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
            set { Subscribe(Avalonia.Controls.TreeViewItem.IsSelectedProperty, value, SubPatchOptions.Always); }
        }

        public Action<bool> OnExpanded
        {
            set { Subscribe(Avalonia.Controls.TreeViewItem.IsExpandedProperty, value, SubPatchOptions.Always); }
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

        public Action<RoutedEventArgs> OnDoubleClick
        {
            set { Subscribe(Avalonia.Controls.TreeViewItem.DoubleTappedEvent, value, SubPatchOptions.Always); }
        }
    }
}
