namespace Stride.Editor.Commands
{
    public interface IReversibleCommand
    {
        /// <summary>
        /// Executes command.
        /// </summary>
        void Apply();

        /// <summary>
        /// Reverses the command.
        /// </summary>
        void Undo();

        /// <summary>
        /// The context object this command operates on.
        /// </summary>
        object Context { get; }
    }
}
