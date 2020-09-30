using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class Grid : PanelViewBuilder<Avalonia.Controls.Grid>
    {
        public IEnumerable<IViewBuilder> Children
        {
            set { ContentMultiple(nameof(Children), value.Select(v => v.Build())); }
        }

        public Thickness Margin
        {
            set { Property(Avalonia.Controls.Grid.MarginProperty, value); }
        }

        public RowDefinitions RowDefinitions
        {
            set
            {
                static RowDefinitions Getter(Avalonia.Controls.Grid grid) => grid.RowDefinitions;
                static void Setter(Avalonia.Controls.Grid grid, RowDefinitions rows) => grid.RowDefinitions = rows;
                static bool Comparer(RowDefinitions r1, RowDefinitions r2) =>
                    Enumerable.SequenceEqual(r1, r2, new RowDefinitionComparer());
                Property(nameof(RowDefinitions), value, Getter, Setter, Comparer);
            }
        }

        public ColumnDefinitions ColumnDefinitions
        {
            set
            {
                static ColumnDefinitions Getter(Avalonia.Controls.Grid grid) => grid.ColumnDefinitions;
                static void Setter(Avalonia.Controls.Grid grid, ColumnDefinitions rows) => grid.ColumnDefinitions = rows;
                static bool Comparer(ColumnDefinitions r1, ColumnDefinitions r2) =>
                    Enumerable.SequenceEqual(r1, r2, new ColumnDefinitionComparer());
                Property(nameof(ColumnDefinitions), value, Getter, Setter, Comparer);
            }
        }

        private class RowDefinitionComparer : IEqualityComparer<RowDefinition>
        {
            public bool Equals([AllowNull] RowDefinition a, [AllowNull] RowDefinition b)
            {
                return a.Height == b.Height &&
                    a.MinHeight == b.MinHeight &&
                    a.MaxHeight == b.MaxHeight &&
                    a.SharedSizeGroup == b.SharedSizeGroup;
            }

            public int GetHashCode([DisallowNull] RowDefinition a)
            {
                return (a.Height, a.MinHeight, a.MaxHeight, a.SharedSizeGroup).GetHashCode();
            }
        }
        private class ColumnDefinitionComparer : IEqualityComparer<ColumnDefinition>
        {
            public bool Equals([AllowNull] ColumnDefinition a, [AllowNull] ColumnDefinition b)
            {
                return a.Width == b.Width &&
                    a.MinWidth == b.MinWidth &&
                    a.MaxWidth == b.MaxWidth &&
                    a.SharedSizeGroup == b.SharedSizeGroup;
            }

            public int GetHashCode([DisallowNull] ColumnDefinition a)
            {
                return (a.Width, a.MinWidth, a.MaxWidth, a.SharedSizeGroup).GetHashCode();
            }
        }
    }
}
