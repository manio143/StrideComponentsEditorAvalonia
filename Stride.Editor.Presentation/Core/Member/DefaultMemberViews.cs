using Stride.Core;
using Stride.Editor.Design.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation.Core.Member
{
    public static class DefaultMemberViews
    {
        public static void Register(IServiceRegistry services)
        {
            var provider = services.GetSafeServiceAs<IMemberViewProvider<IViewBuilder>>();
            Register(provider, new BoolMemberView(services));
            Register(provider, new NumberMemberView(services));
            Register(provider, new StringMemberView(services));
            Register(provider, new EnumMemberView(services));
        }
        private static void Register(IMemberViewProvider<IViewBuilder> provider, MemberViewBase memberView)
        {
            provider.RegisterMemberView(memberView.CanBeApplied, memberView);
        }
    }
}
