using Stride.Core;
using Stride.Core.Reflection;
using Stride.Editor.Avalonia.Core.StringUtils;
using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stride.Editor.Avalonia.EntityHierarchy.Components
{
    public class ComponentViewModel
    {
        public ComponentViewModel(EntityComponent component)
        {
            Component = component;
            TypeDescriptor = TypeDescriptorFactory.Default.Find(component.GetType());
            Name = ComponentName();
            IsEnablable = HasEnabledProperty();
        }

        public EntityComponent Component { get; set; }

        public ITypeDescriptor TypeDescriptor { get; set; }

        public string Name { get; set; }

        public bool IsEnablable { get; }

        public bool IsEnabled
        {
            get
            {
                if (!IsEnablable)
                    return false; // don't throw
                return (bool)EnabledMember.Get(Component);
            }
        }

        public IEnumerable<ComponentMemberViewModel> ComponentMembers
            => TypeDescriptor.Members
                .Select(member => new ComponentMemberViewModel(Component, member));

        #region Commands

        /// <summary>
        /// Toggles the Enabled property/field value of the <see cref="Component"/>.
        /// </summary>
        public void ToggleEnabled()
        {
            if (!IsEnablable)
                throw new InvalidOperationException("This component cannot be enabled/disabled.");
            var value = (bool)EnabledMember.Get(Component);
            EnabledMember.Set(Component, !value);
        }

        #endregion Commands

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
                if (attr is DisplayAttribute display)
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
