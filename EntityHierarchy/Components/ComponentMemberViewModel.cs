using Stride.Core.Reflection;
using Stride.Editor.Avalonia.Core.StringUtils;
using Stride.Engine;

namespace Stride.Editor.Avalonia.EntityHierarchy.Components
{
    public class ComponentMemberViewModel
    {
        public ComponentMemberViewModel(EntityComponent owner, IMemberDescriptor memberDescriptor)
        {
            OwnerComponent = owner;
            MemberDescriptor = memberDescriptor;
            Name = memberDescriptor.Name.CamelcaseToSpaces();
        }

        public EntityComponent OwnerComponent { get; set; }

        public IMemberDescriptor MemberDescriptor { get; set; }

        public ITypeDescriptor TypeDescriptor => MemberDescriptor.TypeDescriptor;

        public string Name { get; set; }

        public object Value
        {
            get => MemberDescriptor.Get(OwnerComponent);
            set => MemberDescriptor.Set(OwnerComponent, value);
        }
    }
}
