using Stride.Core.Reflection;
using Stride.Editor.Design.Core.StringUtils;

namespace Stride.Editor.Design.Core
{
    public class MemberViewModel
    {
        public MemberViewModel(object owner, IMemberDescriptor memberDescriptor)
        {
            Source = owner;
            MemberDescriptor = memberDescriptor;
            Name = memberDescriptor.Name.CamelcaseToSpaces();
        }

        public object Source { get; }

        public IMemberDescriptor MemberDescriptor { get; }

        public ITypeDescriptor TypeDescriptor => MemberDescriptor.TypeDescriptor;

        public string Name { get; set; }

        public object Value
        {
            get => MemberDescriptor.Get(Source);
        }
    }
}
