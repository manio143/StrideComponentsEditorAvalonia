namespace Stride.Editor.Commands
{
    public interface IReversibleCommand : ICommand
    {
        /// <summary>
        /// Executes a reversal of the command under provided <paramref name="context"/>.
        /// </summary>
        void Reverse(object context);
    }

    public interface IReversibleCommand<T> : IReversibleCommand, ICommand<T>
    {
        /// <summary>
        /// Executes a reversal of the command under provided <paramref name="context"/>.
        /// </summary>
        void Reverse(T context);
        void IReversibleCommand.Reverse(object context) => Reverse((T)context);
    }
}
