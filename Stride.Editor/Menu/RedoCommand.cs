using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Services;
using System.Threading.Tasks;

namespace Stride.Editor.Menu
{
    public class RedoCommand : MenuCommandBase
    {
        public RedoCommand(IServiceRegistry services) : base(services)
        {
            CommandDispatcher = services.GetSafeServiceAs<ICommandDispatcher>();
            UndoService = services.GetSafeServiceAs<IUndoService>();
            UndoService.StateChanged += UndoService_StateChanged;
        }

        public override bool CanExecute(object parameter) => CanRedo;

        private bool CanRedo { get; set; }

        private IUndoService UndoService { get; }
        private ICommandDispatcher CommandDispatcher { get; }

        protected override Task ExecuteAsync(object parameter)
        {
            CommandDispatcher.Dispatch(new Services.Undo.RedoCommand(), UndoService);
            return Task.FromResult(false);
        }

        private void UndoService_StateChanged()
        {
            var canRedo = UndoService.CanRedo;
            if (this.CanRedo != canRedo)
            {
                this.CanRedo = canRedo;
                OnCanExecuteChanged();
            }
        }
    }
}
