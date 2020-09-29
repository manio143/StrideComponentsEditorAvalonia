using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Stride.Editor.Design.SceneEditor
{
    internal class HierarchyItemViewModelComparer : IComparer<HierarchyItemViewModel>
    {
        public int Compare([AllowNull] HierarchyItemViewModel x, [AllowNull] HierarchyItemViewModel y)
        {
            if (x.IsFolder)
            {
                if (y.IsFolder)
                    return x.Name.CompareTo(y.Name); // order folders alphabetically
                return -1; // folders precede entities
            }
            else
            {
                if (y.IsFolder)
                    return 1; // folders precede entities
                return 0; // entity order shouldn't matter
            }
        }
    }
}