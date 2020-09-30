using Stride.Core;
using Stride.Editor.Design.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core
{
    public class MemberView : ViewBase<MemberViewModel>
    {
        public MemberView(IServiceRegistry services) : base(services)
        {
        }

        public override IViewBuilder CreateView(MemberViewModel viewModel)
        {
            return new Virtual.ContentControl(); //TODO: Implement view
        }
    }
}
