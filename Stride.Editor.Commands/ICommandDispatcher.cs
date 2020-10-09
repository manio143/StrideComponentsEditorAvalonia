using Stride.Editor.Design;

namespace Stride.Editor.Commands
{
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Dispatch <paramref name="command"/> to be executed with <paramref name="context"/>.
        /// </summary>
        void Dispatch(ICommand command, object context);

        /// <summary>
        /// Dispatch <paramref name="command"/> to be executed with <paramref name="context"/>.
        /// </summary>
        void Dispatch<T>(ICommand<T> command, T context);

        /// <summary>
        /// Dispatch <paramref name="command"/> to be executed with <paramref name="context"/> and the active <see cref="IAssetEditor"/>.
        /// </summary>
        void DispatchToActiveEditor<T>(ICommand<ContextWithActiveEditor<T>> command, T context);

        /// <summary>
        /// Gets or sets the enabled property of the dispatcher.
        /// </summary>
        /// <remarks>If false dispatched commands will not be executed.</remarks>
        bool Enabled { get; set; }
    }
}
