using Avalonia.Controls;
using Stride.Core;
using Stride.Editor.Design.SceneEditor;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.SceneEditor
{
    public class ComponentPropertiesView : ViewBase<EntityViewModel>
    {
        public ComponentPropertiesView(IServiceRegistry services) : base(services)
        {
        }

        public override IViewBuilder CreateView(EntityViewModel viewModel)
        {
            return new Virtual.ContentControl(); //TODO: implement view
        }
    }
}