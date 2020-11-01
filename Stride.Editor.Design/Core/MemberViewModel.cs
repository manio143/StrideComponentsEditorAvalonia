using Stride.Core.Reflection;
using Stride.Editor.Design.Core.StringUtils;
using System.Collections.Generic;
using System.Linq;

namespace Stride.Editor.Design.Core
{
    public class MemberViewModel
    {
        public MemberViewModel(object owner, IMemberDescriptor memberDescriptor, MemberViewModel parent = null, object context = null)
        {
            Source = owner;
            MemberDescriptor = memberDescriptor;
            Parent = parent;
            Name = memberDescriptor.Name.CamelcaseToSpaces();
            Context = context;
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

        // lazily computed - only if used by the structured member view
        private List<MemberViewModel> children;
        public List<MemberViewModel> Children
        {
            get
            {
                if (children == null)
                    children = new List<MemberViewModel>(TypeDescriptor.Members.Select(m => new MemberViewModel(Value, m, this)));
                return children;
            }
        }

        /// <summary>
        /// Additional context for the view.
        /// </summary>
        public object Context { get; set; }
    }
}
