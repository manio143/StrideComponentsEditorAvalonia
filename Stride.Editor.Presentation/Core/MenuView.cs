using Stride.Core;
using Stride.Editor.Design.Core.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core
{
    public class MenuView : ViewBase<IEnumerable<MenuItemViewModel>>
    {
        public MenuView(IServiceRegistry services) : base(services)
        {
        }

        public override IViewBuilder CreateView(IEnumerable<MenuItemViewModel> viewModel)
        {
            return new Virtual.Menu
            {
                Items = viewModel.Select(CreateMenuItem)
            };
        }

        private IViewBuilder CreateMenuItem(MenuItemViewModel viewModel)
        {
            var subItems = viewModel.Items ?? Enumerable.Empty<MenuItemViewModel>();
            return new Virtual.MenuItem
            {
                Items = subItems.Select(CreateMenuItem),
                Header = viewModel.Header,
                IsEnabled = viewModel.IsEnabled,
                Command = viewModel.Command,
                CommandParameter = viewModel.CommandParameter,
            };
        }
    }
}
