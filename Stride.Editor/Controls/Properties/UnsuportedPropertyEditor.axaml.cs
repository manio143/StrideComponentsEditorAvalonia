using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Stride.Editor.Design.Core.StringUtils;
using Stride.Engine;

namespace Stride.Editor.Avalonia.Controls.Properties
{
    public class UnsuportedPropertyEditor : PropertyEditor
    {
        public UnsuportedPropertyEditor()
        {
            this.InitializeComponent();
        }

        public string Text { get; set; }

        protected override void InitializeContent(PropertyViewModel property)
        {
            if (property.Value == null)
                Text = "(None)";
            else if (property.Value is Entity e)
                Text = $"Entity: {e.Name}";
            else if (property.Value is EntityComponent ec)
                Text = $"{ec.GetType().Name.CamelcaseToSpaces()} @ {ec.Entity.Name}";
            else Text = property.Value.ToString();

            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
