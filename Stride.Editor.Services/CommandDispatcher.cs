using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Editor.Commands;
using Stride.Editor.Design;
using Stride.Editor.Design.Core.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stride.Editor.Services
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private static LoggingScope Logger = LoggingScope.Global(nameof(CommandDispatcher));

        public CommandDispatcher(IServiceRegistry serviceRegistry)
        {
            Services = serviceRegistry;
            ViewUpdater = serviceRegistry.GetSafeServiceAs<IViewUpdater>();
            UndoService = serviceRegistry.GetSafeServiceAs<IUndoService>();
            Session = serviceRegistry.GetSafeServiceAs<Session>();
        }

        private readonly Queue<(ICommand, object)> executionQueue = new Queue<(ICommand, object)>();

        public IServiceRegistry Services { get; }

        private IViewUpdater ViewUpdater { get; }
        private IUndoService UndoService { get; }
        private Session Session { get; }

        private bool ignoreCommands;
        private object ignoreCommandsLock = new object();

        /// <inheritdoc/>
        public bool Enabled
        {
            get { lock (ignoreCommandsLock) return !ignoreCommands; }
            set 
            {
                Logger.Debug($"Enabled set to {value}.");
                lock (ignoreCommandsLock) ignoreCommands = !value;
            }
        }

        /// <inheritdoc/>
        public void Dispatch(ICommand command, object context)
        {
            if (!Enabled)
                return;

            Logger.Debug($"Dispatching command {command.GetType()}.");
            lock (executionQueue)
                executionQueue.Enqueue((command, context));
        }

        /// <inheritdoc/>
        public void Dispatch<T>(ICommand<T> command, T context) => Dispatch((ICommand)command, (object)context);

        /// <inheritdoc/>
        public void DispatchToActiveEditor<T>(ICommand<ContextWithActiveEditor<T>> command, T context)
        {
            var activeEditor = Session.EditorViewModel.ActiveEditor;
            if (activeEditor == null)
                throw new System.InvalidOperationException("Cannot dispatch to active editor when it is null.");

            Dispatch(command, new ContextWithActiveEditor<T>
            {
                ActiveEditor = activeEditor,
                Context = context,
            });
        }

        /// <summary>
        /// Processes commands that have been enqued during the last UI input processing frame.
        /// </summary>
        /// <remarks>This method should not be called outside of UI loop synchronization point.</remarks>
        public async Task ProcessDispatchedCommands()
        {
            bool stateHasBeenModified = false;
            for (;;)
            {
                (ICommand cmd, object ctx) item;

                lock (executionQueue)
                {
                    if (executionQueue.Count == 0)
                        break;

                    item = executionQueue.Dequeue();
                }

                item.cmd.Execute(item.ctx);
                stateHasBeenModified = true;

                if (item.cmd is IReversibleCommand rev && !(item.cmd is ICommand<UndoService>))
                    UndoService.RegisterCommand(rev, item.ctx);
            }

            if (stateHasBeenModified)
                await ViewUpdater.UpdateView();
        }
    }
}
