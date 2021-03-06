﻿using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Commands.Core.Hierarchy;
using Stride.Editor.Design.Core.Hierarchy;
using System.Linq;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.SceneEditor
{
    public class HierarchyItemView : ViewBase<HierarchyItemViewModel>
    {
        public HierarchyItemView(IServiceRegistry services) : base(services)
        {
            dispatcher = Services.GetSafeServiceAs<ICommandDispatcher>();
        }
        private ICommandDispatcher dispatcher;


        public override IViewBuilder CreateView(HierarchyItemViewModel viewModel)
        {
            var tvi = new Virtual.TreeViewItem
            {
                Header = viewModel.Name, // TODO: add folder/entity icon
                // ContextMenu = ... TODO: generate entity context menu
                Tag = viewModel,
                Items = viewModel.Children.Select(c => CreateView(c)),
                IsExpanded = viewModel.IsExpanded,
                IsSelected = viewModel.IsSelected,
                OnSelected = selected =>
                    dispatcher.Dispatch(
                        new SelectHierarchyItemCommand(),
                        new SelectHierarchyItemCommand.Context
                        {
                            ViewModel = viewModel,
                            Selected = selected,
                        }),
                OnExpanded = expanded =>
                    dispatcher.Dispatch(
                        new ExpandHierarchyItemCommand(),
                        new ExpandHierarchyItemCommand.Context
                        {
                            ViewModel = viewModel,
                            Expand = expanded,
                        }),
            };
            return tvi;
        }
    }
}
