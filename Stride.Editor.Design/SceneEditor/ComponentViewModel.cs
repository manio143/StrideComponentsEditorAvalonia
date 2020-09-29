using Stride.Engine;

namespace Stride.Editor.Design.SceneEditor
{
    public class ComponentViewModel
    {
        public ComponentViewModel(EntityComponent component)
        {
            Source = component;
        }

        public EntityComponent Source { get; }
    }
}