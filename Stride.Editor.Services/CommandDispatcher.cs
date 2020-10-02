using Stride.Core;
using Stride.Core.Collections;
using Stride.Editor.Commands;
using Stride.Editor.Design;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stride.Editor.Services
{
    public class CommandDispatcher : ICommandDispatcher
    {
        public CommandDispatcher(IServiceRegistry serviceRegistry)
        {
            Services = serviceRegistry;
            AssetEditorViewUpdater = serviceRegistry.GetSafeServiceAs<IAssetEditorViewUpdater>();
            UndoService = serviceRegistry.GetSafeServiceAs<IUndoService>();
            Ticker = Task.Run(Tick);
        }

        private readonly Task Ticker;

        public IServiceRegistry Services { get; }

        public IAssetEditorViewUpdater AssetEditorViewUpdater { get; }

        public IUndoService UndoService { get; }

        private bool ignoreCommands;
        private object ignoreCommandsLock = new object();
        private bool IgnoreCommands
        {
            get { lock (ignoreCommandsLock) return ignoreCommands; }
            set { lock (ignoreCommandsLock) ignoreCommands = value; }
        }

        private HashSet<IAssetEditor> UpdateView = new HashSet<IAssetEditor>();

        /// <inheritdoc/>
        public void Dispatch<TEditor>(IEditorCommand<TEditor> editorCommand) where TEditor : class, IAssetEditor
        {
            if (IgnoreCommands)
                return;

            TEditor found = null;
            foreach (var active in activeEditors)
                if (active is TEditor activeEditor)
                {
                    editorCommand.Apply(activeEditor);
                    found = activeEditor;

                    if (editorCommand is IReversibleEditorCommand<TEditor> reversible)
                        UndoService.RegisterCommand(new ReversibleEditorCommand<TEditor>(this, reversible, activeEditor));
                    
                    break;
                }

            if (found != null)
                lock (UpdateView)
                    UpdateView.Add(found);
        }

        /// <inheritdoc/>
        public void DispatchDirectApply<TEditor>(TEditor editor, IEditorCommand<TEditor> editorCommand) where TEditor : class, IAssetEditor
        {
            editorCommand.Apply(editor);
            if (activeEditors.Contains(editor))
                lock (UpdateView)
                    UpdateView.Add(editor);
        }

        /// <inheritdoc/>
        public void DispatchDirectUndo<TEditor>(TEditor editor, IReversibleEditorCommand<TEditor> editorCommand) where TEditor : class, IAssetEditor
        {
            editorCommand.Undo(editor);
            if (activeEditors.Contains(editor))
                lock (UpdateView)
                    UpdateView.Add(editor);
        }

        private FastCollection<IAssetEditor> activeEditors = new FastCollection<IAssetEditor>();

        /// <summary>
        /// Set active editor for type <typeparamref name="TEditor"/>.
        /// </summary>
        /// <returns>True if another editor of the same type was active, false otherwise.</returns>
        public bool SetActiveEditor<TEditor>(TEditor editor) where TEditor : class, IAssetEditor
        {
            TEditor existing = null;

            foreach (var active in activeEditors)
                if (active is TEditor activeEditor)
                {
                    existing = activeEditor;
                    break;
                }

            if (existing != null)
                activeEditors.Remove(existing);

            activeEditors.Add(editor);

            lock (UpdateView)
                UpdateView.Add(editor);

            return existing != null;
        }

        public async Task Tick()
        {
            while (true)
            {
                await Task.Delay(16); // 60 fps refresh rate
                FastList<IAssetEditor> local;
                lock (UpdateView)
                {
                    if (UpdateView.Count == 0)
                        continue;

                    local = new FastList<IAssetEditor>(UpdateView);
                    UpdateView.Clear();
                }

                IgnoreCommands = true; // ignore commands during view update
                // this is because FuncUI executes subscriptions on creation
                
                foreach (var editor in local)
                    await AssetEditorViewUpdater.UpdateAssetEditorView(editor);

                IgnoreCommands = false;
            }
        }
    }
}
