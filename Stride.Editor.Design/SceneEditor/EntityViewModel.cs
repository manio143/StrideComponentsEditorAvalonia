using Stride.Assets.Entities;
using Stride.Core.Assets;
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
        }

        public EntityDesign Source { get; }

        public IEnumerable<EntityComponentViewModel> Components
            => Source.Entity.Components.Select(comp => new EntityComponentViewModel(comp));

        /// <summary>
        /// Recursively create hierarchy of entities by populating <see cref="HierarchyItemViewModel.Children"/>.
        /// </summary>
        /// <param name="parts">Collection of <see cref="EntityDesign"/> sources.</param>
        public void AddChildren(AssetPartCollection<EntityDesign, Entity> parts)
        {
            foreach (var child in Source.Entity.GetChildren())
            {
                var childVM = new EntityViewModel(parts[child.Id]);
                this.Children.Add(childVM);
                childVM.AddChildren(parts);
            }
        }

        /// <inheritdoc/>
        public override EntityViewModel FindById(Guid entityId)
        {
            if (Source.Entity.Id == entityId)
                return this;
            return base.FindById(entityId);
        }
    }
}
