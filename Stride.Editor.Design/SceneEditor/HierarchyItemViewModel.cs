using System;
using System.Collections.Generic;

namespace Stride.Editor.Design.SceneEditor
{
    public abstract class HierarchyItemViewModel
    {
        /// <summary>
        /// Name of the item.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Whether this item represents a folder.
        /// </summary>
        public bool IsFolder { get; protected set; }

        /// <summary>
        /// Child items in the hierarchy.
        /// </summary>
        public List<HierarchyItemViewModel> Children { get; } = new List<HierarchyItemViewModel>();

        /// <summary>
        /// Wether this item has been expanded in the view.
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Wether this item has been expanded in the view.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Find the <see cref="EntityViewModel"/> corresponding to an entity with <paramref name="entityId"/>. 
        /// </summary>
        /// <returns>ViewModel for an entity with this Id, or null if not found.</returns>
        public virtual EntityViewModel FindById(Guid entityId)
        {
            foreach (var child in Children)
            {
                var evm = child.FindById(entityId);
                if (evm != null)
                    return evm;
            }
            return null;
        }
    }
}