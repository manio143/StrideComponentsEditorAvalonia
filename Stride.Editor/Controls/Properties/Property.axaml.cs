using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Stride.Core.Reflection;
using System.Windows.Input;

namespace Stride.Editor.Avalonia.Controls.Properties
{
    public class Property : UserControl
    {
        public Property()
        {
            this.InitializeComponent();
            this.DataContextChanged += (sender, args) => InitializeContent();
        }

        private void InitializeContent()
        {
            var vm = (PropertyViewModel)DataContext;
            var editor = PropertyEditor.GetEditor(vm.Type.Type);
            editor.Initialize(vm);
            this.FindControl<ContentControl>("PropertyEditor").Content = editor;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
