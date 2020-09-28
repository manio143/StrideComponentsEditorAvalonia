using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace Stride.Editor.Avalonia.Controls.Properties
{
    public class NumberPropertyEditor : PropertyEditor
    {
        public NumberPropertyEditor()
        {
            this.InitializeComponent();
        }

        protected override void InitializeContent(PropertyViewModel property)
        {
            var input = (NumericUpDown)Content;
            input.Value = Convert.ToDouble(property.Value);
            input.ValueChanged += (sender, args) =>
            {
                if (property.Type.Type == typeof(int))
                    property.Value = (int)input.Value;
                else if (property.Type.Type == typeof(uint))
                    property.Value = (uint)input.Value;
                else if (property.Type.Type == typeof(float))
                    property.Value = (float)input.Value;
                else if (property.Type.Type == typeof(long))
                    property.Value = (long)input.Value;
                else if (property.Type.Type == typeof(short))
                    property.Value = (short)input.Value;
                else if (property.Type.Type == typeof(ulong))
                    property.Value = (ulong)input.Value;
                else if (property.Type.Type == typeof(ushort))
                    property.Value = (ushort)input.Value;
                else if (property.Type.Type == typeof(byte))
                    property.Value = (byte)input.Value;
                else if (property.Type.Type == typeof(sbyte))
                    property.Value = (sbyte)input.Value;
                else property.Value = input.Value; //double
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
