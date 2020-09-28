using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Stride.Editor.Avalonia.Controls.Properties
{
    public class BoolPropertyEditor : PropertyEditor
    {
        public BoolPropertyEditor()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
