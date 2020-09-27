using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Stride.Editor.Avalonia.Controls.Properties
{
    public class EnumPropertyEditor : PropertyEditor
    {
        public EnumPropertyEditor()
        {
            this.InitializeComponent();
        }

        protected override void InitializeContent(PropertyViewModel property)
        {
            var values = Enum.GetValues(property.Type.Type);
            var names = Enum.GetNames(property.Type.Type);
            var flags = property.Type.Type.GetCustomAttribute<FlagsAttribute>();
            var comboBox = (ComboBox)Content;
            if (flags == null)
            {
                // TODO: implement sane and working combobox
                comboBox.Items = values.Cast<object>().Select((v, i)
                    => new ComboBoxItem { Tag = v, Content = new TextBlock { Text = names[i] } });
                comboBox.SelectedIndex = Array.IndexOf(values, property.Value);
                comboBox.SelectionChanged += (sender, args) =>
                    property.Value = comboBox.SelectedItem;
            }
            else
            {
                //TODO: implement multichoice combobox
                Content = "Flag enums are unsupported.";
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
