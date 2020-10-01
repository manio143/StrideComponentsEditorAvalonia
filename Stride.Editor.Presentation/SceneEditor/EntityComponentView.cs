using Avalonia.Layout;
using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Commands.SceneEditor;
using Stride.Editor.Design.SceneEditor;
using Stride.Editor.Presentation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.SceneEditor
{
    public class EntityComponentView : ViewBase<EntityComponentViewModel>
    {
        public EntityComponentView(IServiceRegistry services) : base(services)
        {
            memberView = new MemberView(services);
            dispatcher = services.GetSafeServiceAs<ICommandDispatcher>();
        }

        private MemberView memberView;
        private ICommandDispatcher dispatcher;

        public override IViewBuilder CreateView(EntityComponentViewModel viewModel)
        {
            return new Virtual.Expander
            {
                Header = CreateHeader(viewModel),
                IsExpanded = viewModel.IsExpanded,
                Content = new Virtual.ItemsControl
                {
                    Items = viewModel.ComponentMembers.Select(cm => memberView.CreateView(cm)),
                }
            };
        }

        private IViewBuilder CreateHeader(EntityComponentViewModel viewModel)
        {
            return new Virtual.StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children = new List<IViewBuilder>
                {
                    new Virtual.CheckBox
                    {
                        IsVisible = viewModel.IsEnablable,
                        IsChecked = viewModel.IsEnabled,
                        OnChecked = (check) =>
                        {
                            if (viewModel.IsEnablable)
                                dispatcher.Dispatch(new EnableEntityComponentCommand(viewModel, check ?? false));
                        },
                    },
                    new Virtual.TextBlock
                    {
                        Text = viewModel.Name,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    }
                }
            };
        }
    }
}
