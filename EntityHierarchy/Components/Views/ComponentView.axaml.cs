using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using System;

namespace Stride.Editor.Avalonia.EntityHierarchy.Components.Views
{
    public class ComponentView : UserControl
    {
        public ComponentView()
        {
            this.InitializeComponent();
        }

        public ComponentView(ComponentViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
