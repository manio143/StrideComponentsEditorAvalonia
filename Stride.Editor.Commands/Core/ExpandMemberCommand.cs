using Stride.Editor.Design.Core;

namespace Stride.Editor.Commands.Core
{
    /// <summary>
    /// Modifies the <see cref="MemberViewModel.IsExpanded"/> property.
    /// </summary>
    public class ExpandMemberCommand : ICommand<ExpandMemberCommand.Context>
    {
        public struct Context
        {
            public MemberViewModel ViewModel { get; set; }
            public bool Expand { get; set; }
        }

        public void Execute(Context context)
        {
            context.ViewModel.IsExpanded = context.Expand;
        }
    }
}
