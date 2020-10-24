using Stride.Core.Annotations;
using System.Collections.Generic;
using System.Windows.Input;

namespace Stride.Editor.Design.Core.Menu
{
    public class MenuItemViewModel
    {
        public string Header { get; set; }
        public bool IsEnabled { get; set; } = true;
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }
        public IList<MenuItemViewModel> Items { get; set; }
    }
}
