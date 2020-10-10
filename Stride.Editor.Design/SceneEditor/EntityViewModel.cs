using Stride.Assets.Entities;
using Stride.Editor.Design.Core.Hierarchy;
using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stride.Editor.Design.SceneEditor
{
    public class EntityViewModel : HierarchyItemViewModel
    {
        public EntityViewModel(EntityDesign entityDesign)
        {
            Name = entityDesign.Entity.Name;
            IsFolder = false;
            Source = entityDesign;
            Components = Source.Entity.Components.Select(comp => new EntityComponentViewModel(comp)).ToList();
        }

        public EntityDesign Source { get; }

        public List<EntityComponentViewModel> Components { get; }

        /// <summary>
        /// Recursively create hierarchy of entities by populating <see cref="HierarchyItemViewModel.Children"/>.
        /// </summary>
        /// <param name="parts">Collection of <see cref="EntityDesign"/> sources.</param>
        public void AddChildren(IDictionary<Guid, EntityDesign> designData)
        {
            foreach (var child in Source.Entity.GetChildren())
            {
                var childVM = new EntityViewModel(designData[child.Id]);
                this.Children.Add(childVM);
                childVM.AddChildren(designData);
            }
        }

        /// <inheritdoc/>
        public override TChild FindById<TChild>(Guid entityId) where TChild : class
        {
            if (Source.Entity.Id == entityId)
                return this as TChild;
            return base.FindById<TChild>(entityId);
        }
    }
}
