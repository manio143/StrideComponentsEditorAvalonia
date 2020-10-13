using Dock.Model;
using Stride.Editor.Design.Core;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Stride.Editor.Presentation.Core.Docking
{
    public static class TabViewModelComparer
    {
        public static bool Equal(IDockable dock1, IDockable dock2)
        {
            if (dock1 == null) return dock2 == null;
            else if (dock2 == null) return false;

            if (dock1.GetType() != dock2.GetType())
                return false;

            if (dock1 is IDock d1 && dock2 is IDock d2)
                return Equal(d1, d2);

            if (dock1.Id != dock2.Id)
                return false;

            // NOTE: With the above IDock check this would create a loop
            //if (!Equal(dock1.Owner, dock2.Owner))
            //    return false;

            if (dock1 is ITabViewModel tab1 && dock2 is ITabViewModel tab2)
            {
                if (tab1.ViewModel != tab2.ViewModel)
                    return false;
            }

            return true;
        }

        public static bool Equal(IDock dock1, IDock dock2)
        {
            if (dock1.GetType() != dock2.GetType())
                return false;
            if (dock1.Id != dock2.Id)
                return false;

            if (!Enumerable.SequenceEqual(dock1.VisibleDockables, dock2.VisibleDockables, dockableComparer))
                return false;
            if (!Enumerable.SequenceEqual(dock1.HiddenDockables, dock2.HiddenDockables, dockableComparer))
                return false;
            if(dock1.PinnedDockables != null && dock2.PinnedDockables != null)
                if (!Enumerable.SequenceEqual(dock1.PinnedDockables, dock2.PinnedDockables, dockableComparer))
                    return false;
            if (!Equal(dock1.ActiveDockable, dock2.ActiveDockable))
                return false;
            if (!Equal(dock1.FocusedDockable, dock2.FocusedDockable))
                return false;
            if (dock1.IsActive != dock2.IsActive)
                return false;

            return true;
        }

        private static DockableComparer dockableComparer = new DockableComparer();

        private class DockableComparer : IEqualityComparer<IDockable>
        {
            public bool Equals([AllowNull] IDockable x, [AllowNull] IDockable y)
            {
                return TabViewModelComparer.Equal(x, y);
            }

            public int GetHashCode([DisallowNull] IDockable obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}
