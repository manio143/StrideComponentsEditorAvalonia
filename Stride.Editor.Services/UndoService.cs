using Stride.Editor.Commands;
using System;
using System.Collections.Generic;

namespace Stride.Editor.Services
{
    // TODO: add logging
    public class UndoService : IUndoService
    {
        // TODO: implement this as a cyclic buffer of some size to forget commands in the past.
        private Stack<IReversibleCommand> undoStack = new Stack<IReversibleCommand>();
        private Stack<IReversibleCommand> redoStack = new Stack<IReversibleCommand>();

        public event Action StateChanged;
        private void OnStateChange() => StateChanged?.Invoke();

        /// <inheritdoc/>
        public void RegisterCommand(IReversibleCommand command)
        {
            undoStack.Push(command);
            redoStack.Clear();
            OnStateChange();
        }

        /// <inheritdoc/>
        public bool CanUndo => undoStack.Count > 0;

        /// <inheritdoc/>
        public bool CanRedo => redoStack.Count > 0;

        /// <inheritdoc/>
        public void Clear()
        {
            var empty = undoStack.Count == 0 && redoStack.Count == 0;

            undoStack.Clear();
            redoStack.Clear();

            if (empty)
                OnStateChange();
        }

        /// <inheritdoc/>
        public void Clear(object context)
        {
            var totalCountBefore = undoStack.Count + redoStack.Count;

            Clear(ref undoStack, context);
            Clear(ref redoStack, context);

            var totalCountAfter = undoStack.Count + redoStack.Count;
            if (totalCountBefore != totalCountAfter)
                OnStateChange();
        }

        private void Clear(ref Stack<IReversibleCommand> stack, object context)
        {
            var temporaryStack = new Stack<IReversibleCommand>(stack.Count);
            while (stack.TryPop(out var command))
            {
                // move commands that don't match context onto temporary stack
                if (command.Context != context)
                    temporaryStack.Push(command);
            }
            while (temporaryStack.TryPop(out var command))
                stack.Push(command); // move them back in the correct order
        }

        /// <inheritdoc/>
        public void Redo()
        {
            if (!CanRedo)
                throw new InvalidOperationException("Could not Redo.");
            var command = redoStack.Pop();
            command.Apply();
            undoStack.Push(command);
            OnStateChange();
        }

        /// <inheritdoc/>
        public void Undo()
        {
            if (!CanUndo)
                throw new InvalidOperationException("Could not Undo.");
            var command = undoStack.Pop();
            command.Undo();
            redoStack.Push(command);
            OnStateChange();
        }
    }
}
