using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Stride.Editor.Avalonia.Controls.Properties
{
    public class CharPropertyEditor : PropertyEditor
    {
        public CharPropertyEditor()
        {
            this.InitializeComponent();
        }

        public string Text
        {
            get => new string((char)((PropertyViewModel)DataContext).Value, 1);
            set => ((PropertyViewModel)DataContext).Value = string.IsNullOrEmpty(value) ? (char)0 : value[0];
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
