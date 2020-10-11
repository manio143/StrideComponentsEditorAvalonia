using Stride.Editor.Commands;

namespace Stride.Editor.Services.Undo
{
    public class UndoCommand : ICommand<IUndoService>
    {
        public void Execute(IUndoService context)
        {
            context.Undo();
        }
    }
}
