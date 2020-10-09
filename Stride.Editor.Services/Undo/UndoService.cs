using Stride.Editor.Commands;
using System;
using System.Collections.Generic;

namespace Stride.Editor.Services
{
    // TODO: add logging
    public class UndoService : IUndoService
    {
        private struct StackEntry
        {
            public IReversibleCommand Command { get; set; }
            public object Context { get; set; }
        }

        // TODO: implement this as a cyclic buffer of some size to forget commands in the past.
        private Stack<StackEntry> undoStack = new Stack<StackEntry>();
        private Stack<StackEntry> redoStack = new Stack<StackEntry>();

        public event Action StateChanged;
        private void OnStateChange() => StateChanged?.Invoke();

        /// <inheritdoc/>
        public void RegisterCommand(IReversibleCommand command, object context)
        {
            undoStack.Push(new StackEntry { Command = command, Context = context });
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

        private void Clear(ref Stack<StackEntry> stack, object context)
        {
            var temporaryStack = new Stack<StackEntry>(stack.Count);
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
            var entry = redoStack.Pop();
            
            entry.Command.Execute(entry.Context);
            
            undoStack.Push(entry);
            OnStateChange();
        }

        /// <inheritdoc/>
        public void Undo()
        {
            if (!CanUndo)
                throw new InvalidOperationException("Could not Undo.");
            var entry = undoStack.Pop();
            
            entry.Command.Reverse(entry.Context);
            
            redoStack.Push(entry);
            OnStateChange();
        }
    }
}
