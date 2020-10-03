using Stride.Core.Reflection;
using Stride.Editor.Design.Core.StringUtils;

namespace Stride.Editor.Design.Core
{
    public class MemberViewModel
    {
        public MemberViewModel(object owner, IMemberDescriptor memberDescriptor, MemberViewModel parent = null)
        {
            Source = owner;
            MemberDescriptor = memberDescriptor;
            Name = memberDescriptor.Name.CamelcaseToSpaces();
        }

        public object Source { get; }

        /// <summary>
        /// For structured members
        /// </summary>
        public MemberViewModel Parent { get; }

        public IMemberDescriptor MemberDescriptor { get; }

        public ITypeDescriptor TypeDescriptor => MemberDescriptor.TypeDescriptor;

        public string Name { get; }

        public object Value
        {
            get => MemberDescriptor.Get(Source);
            set
            {
                MemberDescriptor.Set(Source, value);
                if (Parent != null)
                {
                    Parent.Value = Source;
                }
            }
        }

        public bool? IsExpanded { get; set; }
    }
}
