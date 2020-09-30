using Avalonia.Controls;
using Avalonia.FuncUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class TreeView : ViewBuilder<Avalonia.Controls.TreeView>
    {
        public IEnumerable<IViewBuilder> Items
        {
            set { ContentMultiple(Avalonia.Controls.TreeViewItem.ItemsProperty, value.Select(vb => vb.Build())); }
        }

        public SelectionMode SelectionMode
        {
            set { Property(Avalonia.Controls.TreeView.SelectionModeProperty, value); }
        }

        public bool AutoScrollToSelectedItem
        {
            set { Property(Avalonia.Controls.TreeView.AutoScrollToSelectedItemProperty, value); }
        }

        public Action<IList> OnSelectedItems
        {
            set { Subscribe(Avalonia.Controls.TreeView.SelectedItemsProperty, value); }
        }
    }
}
