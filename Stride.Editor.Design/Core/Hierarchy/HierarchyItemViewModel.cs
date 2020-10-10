using System;
using System.Collections.Generic;

namespace Stride.Editor.Design.Core.Hierarchy
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
        /// Find the child <typeparamref name="TChild"/> corresponding to an entity with <paramref name="id"/>. 
        /// </summary>
        /// <returns>ViewModel for an child with this Id, or null if not found.</returns>
        public virtual TChild FindById<TChild>(Guid id) where TChild : class
        {
            foreach (var child in Children)
            {
                var evm = child.FindById<TChild>(id);
                if (evm != null)
                    return evm;
            }
            return null;
        }
    }
}