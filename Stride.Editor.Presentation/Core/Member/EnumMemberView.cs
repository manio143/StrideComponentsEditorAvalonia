using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Commands.Core;
using Stride.Editor.Design.Core;
using System;
using System.Linq;
using System.Reflection;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core.Member
{
    public sealed class EnumMemberView : InlineMemberView
    {
        public EnumMemberView(IServiceRegistry services) : base(services)
        {
            dispatcher = services.GetSafeServiceAs<ICommandDispatcher>();
        }
        private ICommandDispatcher dispatcher;

        public override bool CanBeApplied(MemberViewModel viewModel)
        {
            return viewModel.TypeDescriptor.Type.IsEnum;
        }

        protected override IViewBuilder CreatePropertyView(MemberViewModel viewModel)
        {
            var type = viewModel.TypeDescriptor.Type;
            var values = Enum.GetValues(type).Cast<object>().ToArray();
            var names = Enum.GetNames(type);
            var flags = type.GetCustomAttribute<FlagsAttribute>();
            if (flags == null)
            {
                return new Virtual.ComboBox
                {
                    Items = values.Select((v, i)
                        => new Virtual.ComboBoxItem
                        {
                            Tag = v,
                            Content = new Virtual.TextBlock { Text = names[i] }
                        }),
                    SelectedIndex = Array.IndexOf(values, viewModel.Value),
                    OnSelected = (index) => dispatcher.DispatchCore(new UpdateMemberValueCommand(viewModel, values[index])),
                };
            }
            else
            {
                //TODO: implement multichoice combobox
                return new Virtual.TextBlock { Text = "Flag enums are not yet supported." };
            }
        }
    }
}
