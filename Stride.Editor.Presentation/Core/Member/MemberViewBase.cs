using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Commands.AssetManager;
using Stride.Editor.Commands.Core;
using Stride.Editor.Design;
using Stride.Editor.Design.Core;
using System;

namespace Stride.Editor.Presentation.Core.Member
{
    public abstract class MemberViewBase : ViewBase<MemberViewModel>
    {
        protected MemberViewBase(IServiceRegistry services) : base(services)
        {
            dispatcher = services.GetSafeServiceAs<ICommandDispatcher>();
        }

        private ICommandDispatcher dispatcher;

        public abstract bool CanBeApplied(MemberViewModel viewModel);

        protected (ICommand, object) CreateUpdateCommand(MemberViewModel viewModel, object value)
        {
            if (viewModel.Context is IAssetEditor editor)
            {
                return (new UpdateAssetMemberValueCommand(),
                    new UpdateAssetMemberValueCommand.Context
                    {
                        AssetEditor = editor,
                        AssetManager = Services.GetSafeServiceAs<IAssetManager>(),
                        ViewModel = viewModel,
                        Value = value,
                    });
            }
            else
            {
                return (new UpdateMemberValueCommand(),
                    new UpdateMemberValueCommand.Context
                    {
                        ViewModel = viewModel,
                        Value = value,
                    });
            }
        }

        protected Action<T> CreateUpdate<T>(MemberViewModel viewModel, Func<T, object> valueTransform = null)
        {
            return (value) =>
            {
                var transformedValue = valueTransform?.Invoke(value) ?? value;
                (var command, var context) = CreateUpdateCommand(viewModel, transformedValue);
                dispatcher.Dispatch(command, context);
            };
        }
    }
}
