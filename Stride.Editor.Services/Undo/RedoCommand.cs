using Stride.Editor.Commands;

namespace Stride.Editor.Services.Undo
{
    public class RedoCommand : ICommand<IUndoService>
    {
        public void Execute(IUndoService context)
        {
            context.Redo();
        }
    }
}
