using Avalonia.Layout;
using Stride.Core;
using Stride.Core.Reflection;
using Stride.Editor.Commands;
using Stride.Editor.Commands.Core;
using Stride.Editor.Design.Core;
using Stride.Engine;
using System;
using System.Linq;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core.Member
{
    public class StructuredMemberView : MemberViewBase
    {
        public StructuredMemberView(IServiceRegistry services) : base(services)
        {
            viewProvider = services.GetSafeServiceAs<IMemberViewProvider<IViewBuilder>>();
            dispatcher = services.GetSafeServiceAs<ICommandDispatcher>();
        }

        private IMemberViewProvider<IViewBuilder> viewProvider;
        private ICommandDispatcher dispatcher;

        public override bool CanBeApplied(MemberViewModel viewModel)
        {
            var type = viewModel.TypeDescriptor.Type;
            if (type == typeof(string) || viewModel.Value == null ||
                (!type.IsStruct() && !type.IsClass))
                return false;
            // TODO: how to check if something is a reference?
            // lets's assume the following:
            if (typeof(ComponentBase).IsAssignableFrom(type))
                return false; // this includes Entity references and high level components/objects
            if (typeof(EntityComponent).IsAssignableFrom(type))
                return false; // cannot create subcomponents

            return true;
        }

        public override IViewBuilder CreateView(MemberViewModel viewModel)
        {
            var inlineRight = CreateInlineRHS(viewModel);
            inlineRight.Property(Layoutable.HorizontalAlignmentProperty, HorizontalAlignment.Right);
            var content = CreateContent(viewModel);

            return new Virtual.Expander
            {
                IsExpanded = viewModel.IsExpanded ?? false,
                OnExpanded = (expand) =>
                    dispatcher.Dispatch(
                        new ExpandMemberCommand(),
                        new ExpandMemberCommand.Context
                        {
                            ViewModel = viewModel,
                            Expand = expand,
                        }),
                Header = new Virtual.DockPanel
                {
                    Children = new IViewBuilder[]
                    {
                        new Virtual.TextBlock
                        {
                            Text = viewModel.Name,
                            HorizontalAlignment = HorizontalAlignment.Left,
                        },
                        inlineRight,
                    }
                },
                Content = content,
            };
        }

        protected virtual IViewBuilder CreateInlineRHS(MemberViewModel viewModel)
        {
            return new Virtual.ContentControl();
        }

        protected virtual IViewBuilder CreateContent(MemberViewModel viewModel)
        {
            var children = viewModel.Children;
            return new Virtual.ItemsControl
            {
                Items = children.Select(viewProvider.CreateView),
            };
        }
    }
}
