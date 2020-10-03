using Stride.Core;
using Stride.Editor.Design.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation.Core.Member
{
    public abstract class MemberViewBase : ViewBase<MemberViewModel>
    {
        protected MemberViewBase(IServiceRegistry services) : base(services)
        {
        }

        public abstract bool CanBeApplied(MemberViewModel viewModel);
    }
}
