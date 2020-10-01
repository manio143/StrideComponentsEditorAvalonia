using Stride.Editor.Commands;

namespace Stride.Editor.Services
{
    public interface IUndoService
    {
        /// <summary>
        /// Adds <paramref name="command"/> to te undo stack and clears redo stack.
        /// </summary>
        /// <param name="command">Executed command that can be undone.</param>
        void RegisterCommand(IReversibleCommand command);

        /// <summary>
        /// True if there is an action on the undo stack that can be undone.
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// True if there is an action on the redo stack that can be reapplied.
        /// </summary>
        bool CanRedo { get; }

        /// <summary>
        /// Undo the last executed action.
        /// </summary>
        void Undo();

        /// <summary>
        /// Redo the last undone action.
        /// </summary>
        void Redo();

        /// <summary>
        /// Clears undo/redo stacks.
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears undo/redo stacks of the commands refering to <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        void Clear(object context);
    }
}
