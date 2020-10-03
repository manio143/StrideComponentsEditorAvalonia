using Stride.Editor.Design.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Stride.Editor.Commands.Core
{
    // TODO: While ICommand seems like a decent interface (and potentially reusable across different places) I think it makes more sense to create a custom one during dispatcher rewrite
    public class ExpandMemberCommand : ICommand
    {
        public ExpandMemberCommand(MemberViewModel viewModel, bool expand)
        {
            ViewModel = viewModel;
            Expand = expand;
        }

        private MemberViewModel ViewModel { get; }
        private bool Expand { get; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            ViewModel.IsExpanded = Expand;
        }
    }
}
