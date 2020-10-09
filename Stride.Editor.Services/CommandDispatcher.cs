using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Editor.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stride.Editor.Services
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private static Logger Logger = GlobalLogger.GetLogger(nameof(CommandDispatcher));

        public CommandDispatcher(IServiceRegistry serviceRegistry)
        {
            Services = serviceRegistry;
            ViewUpdater = serviceRegistry.GetSafeServiceAs<IViewUpdater>();
            UndoService = serviceRegistry.GetSafeServiceAs<IUndoService>();
            Session = serviceRegistry.GetSafeServiceAs<Session>();
        }

        private Task Worker;
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
            set { lock (ignoreCommandsLock) ignoreCommands = !value; }
        }

        /// <inheritdoc/>
        public void Dispatch(ICommand command, object context)
        {
            if (!Enabled)
                return;

            lock (executionQueue)
                executionQueue.Enqueue((command, context));

            if (Worker.IsFaulted)
            {
                Logger.Error("Command dispatcher's worker has faulter.", Worker.Exception);
                Worker = null;
            }

            if (Worker == null)
                Worker = Task.Run(ProcessCommands);
        }

        /// <inheritdoc/>
        public void Dispatch<T>(ICommand<T> command, T context) => Dispatch(command, context);

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

        private async Task ProcessCommands()
        {
            await Task.Delay(2); // wait for all commands from a single UI action to get added to the queue

            for(;;)
            {
                (ICommand cmd, object ctx) item;

                lock(executionQueue)
                {
                    if (executionQueue.Count == 0)
                        break;

                    item = executionQueue.Dequeue();
                }

                item.cmd.Execute(item.ctx);

                if (item.cmd is IReversibleCommand rev && !(item.cmd is ICommand<UndoService>))
                    UndoService.RegisterCommand(rev, item.ctx);
            }

            await ViewUpdater.UpdateView(this);
        }
    }
}
