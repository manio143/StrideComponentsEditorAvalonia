using Stride.Editor.Design.SceneEditor;

namespace Stride.Editor.Commands.SceneEditor
{
    /// <summary>
    /// Modifies te <see cref="EntityComponentViewModel.IsExpanded"/> property.
    /// </summary>
    public class ExpandEntityComponentCommand : ICommand<ExpandEntityComponentCommand.Context>
    {
        public struct Context
        {
            public EntityComponentViewModel ViewModel { get; set; }
            public bool Expand { get; set; }
        }

        public void Execute(Context context)
        {
            context.ViewModel.IsExpanded = context.Expand;
        }
    }
}
