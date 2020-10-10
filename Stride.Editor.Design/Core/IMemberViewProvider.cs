using System;

namespace Stride.Editor.Design.Core
{
    public interface IMemberViewProvider<TViewObject> : IView<MemberViewModel, TViewObject>
    {
        void RegisterMemberView(Func<MemberViewModel, bool> canBeApplied, IView<MemberViewModel, TViewObject> view);

        void UnregisterMemberView<TView>() where TView : IView<MemberViewModel, TViewObject>;
    }
}
