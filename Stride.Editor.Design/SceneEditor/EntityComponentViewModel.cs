using Stride.Core;
using Stride.Core.Reflection;
using Stride.Editor.Design.Core;
using Stride.Editor.Design.Core.StringUtils;
using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stride.Editor.Design.SceneEditor
{
    public class EntityComponentViewModel
    {
        public EntityComponentViewModel(EntityComponent component, IAssetEditor editor)
        {
            Source = component;
            Editor = editor;
            TypeDescriptor = TypeDescriptorFactory.Default.Find(component.GetType());
            Name = ComponentName();
            IsEnablable = HasEnabledProperty();
        }

        public EntityComponent Source { get; }

        public IAssetEditor Editor { get; }

        public string Name { get; }

        public ITypeDescriptor TypeDescriptor { get; }

        public bool IsExpanded { get; set; }

        public bool IsEnablable { get; }

        public bool IsEnabled
        {
            get
            {
                if (!IsEnablable)
                    return false; // don't throw
                return (bool)EnabledMember.Get(Source);
            }
            set
            {
                if (!IsEnablable)
                    throw new InvalidOperationException("Cannot set IsEnabled for this component");
                EnabledMember.Set(Source, value); // we modify the component in the asset in memory
            }
        }

        public IEnumerable<MemberViewModel> ComponentMembers
            => TypeDescriptor.Members
                .Select(member => new MemberViewModel(Source, member, context: Editor));

        private IMemberDescriptor enabledMember;
        private IMemberDescriptor EnabledMember
        {
            get
            {
                if (enabledMember == null)
                    enabledMember = TypeDescriptor.Members.FirstOrDefault(member => member.Name == "Enabled");
                return enabledMember;
            }
        }

        /// <summary>
        /// Computes a human friendly name of the component.
        /// </summary>
        private string ComponentName()
        {
            // If there's a [Display] return its value
            foreach (var attr in TypeDescriptor.Attributes)
                if (attr is DisplayAttribute display 
                    && !string.IsNullOrWhiteSpace(display.Name))
                    return display.Name;

            // Otherwise insert spaces before uppper case letters to improve readability
            var name = TypeDescriptor.Type.Name;
            return name.CamelcaseToSpaces();
        }

        /// <summary>
        /// Checks if there's a bool Enabled property/field on the component.
        /// </summary>
        private bool HasEnabledProperty()
        {
            return EnabledMember != null && EnabledMember.Type == typeof(bool);
        }
    }
}