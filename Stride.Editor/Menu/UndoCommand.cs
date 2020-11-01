using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Services;
using System.Threading.Tasks;

namespace Stride.Editor.Menu
{
    public class UndoCommand : MenuCommandBase
    {
        public UndoCommand(IServiceRegistry services) : base(services)
        {
            CommandDispatcher = services.GetSafeServiceAs<ICommandDispatcher>();
            UndoService = services.GetSafeServiceAs<IUndoService>();
            UndoService.StateChanged += UndoService_StateChanged;
        }

        public override bool CanExecute(object parameter) => CanUndo;

        private bool CanUndo { get; set; }

        private IUndoService UndoService { get; }
        private ICommandDispatcher CommandDispatcher { get; }

        protected override Task ExecuteAsync(object parameter)
        {
            CommandDispatcher.Dispatch(new Services.Undo.UndoCommand(), UndoService);
            return Task.FromResult(false);
        }

        private void UndoService_StateChanged()
        {
            var canUndo = UndoService.CanUndo;
            if (this.CanUndo != canUndo)
            {
                this.CanUndo = canUndo;
                OnCanExecuteChanged();
            }
        }
    }
}
