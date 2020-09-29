using Avalonia.Controls;
using Stride.Core;
using Stride.Editor.Design.SceneEditor;

namespace Stride.Editor.Presentation.SceneEditor
{
    public class ComponentPropertiesView : ViewBase<EntityViewModel>
    {
        public ComponentPropertiesView(IServiceRegistry services) : base(services)
        {
        }

        public override IControl CreateView(EntityViewModel viewModel)
        {
            return new ContentControl(); //TODO: implement view
        }
    }
}