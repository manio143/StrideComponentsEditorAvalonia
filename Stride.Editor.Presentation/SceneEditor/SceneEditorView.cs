using Avalonia;
using Avalonia.Controls;
using Stride.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation.SceneEditor
{
    public class SceneEditorView : ViewBase<Design.SceneEditor.SceneEditor>
    {
        public SceneEditorView(IServiceRegistry services) : base(services)
        {
        }

        public override IControl CreateView(Design.SceneEditor.SceneEditor viewModel)
        {
            var sceneHierarchy = new SceneHierarchyView(Services).CreateView(viewModel.Scene);
            sceneHierarchy.SetValue(Grid.ColumnProperty, 0);
            var components = new ComponentPropertiesView(Services).CreateView(viewModel.SelectedEntity);
            components.SetValue(Grid.ColumnProperty, 1);
            return new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("*,*"),
                RowDefinitions = new RowDefinitions(),
                Margin = new Thickness(4), //TODO: this should be in a style file or something
                Children =
                {
                    sceneHierarchy,
                    components,
                }
            };
        }
    }
}
