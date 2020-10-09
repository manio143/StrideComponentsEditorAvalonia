using Stride.Editor.Design.SceneEditor;

namespace Stride.Editor.Commands.SceneEditor
{
    /// <summary>
    /// Modifies the <see cref="EntityComponentViewModel.IsEnabled"/> property.
    /// </summary>
    public class EnableEntityComponentCommand : IReversibleCommand<EnableEntityComponentCommand.Context>
    {
        public struct Context
        {
            public EntityComponentViewModel ViewModel { get; set; }
            public bool Enable { get; set; }
        }

        public void Reverse(Context context)
        {
            context.ViewModel.IsEnabled = !context.Enable;
        }

        public void Execute(Context context)
        {
            context.ViewModel.IsEnabled = context.Enable;
        }
    }
}
