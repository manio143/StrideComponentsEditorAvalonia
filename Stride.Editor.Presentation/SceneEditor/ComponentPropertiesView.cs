using Stride.Core;
using Stride.Core.Annotations;
using Stride.Editor.Design.SceneEditor;
using System.Linq;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.SceneEditor
{
    public class ComponentPropertiesView : ViewBase<EntityViewModel>
    {
        public ComponentPropertiesView(IServiceRegistry services) : base(services)
        {
            componentView = new EntityComponentView(services);
        }

        private EntityComponentView componentView;

        public override IViewBuilder CreateView([CanBeNull] EntityViewModel viewModel)
        {
            return new Virtual.ScrollViewer
            {
                Content = viewModel == null ? null : new Virtual.StackPanel
                {
                    Children = viewModel.Components.Select(c => componentView.CreateView(c)),
                }
            };
        }
    }
}