using Stride.Core.Reflection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Avalonia.Controls.Properties
{
    public class PropertyViewModel
    {
        private object _value;

        public event Action<object> ValueChanged;
        public string Label { get; set; }
        public object Value
        {
            get => _value;
            set { _value = value; ValueChanged?.Invoke(value); }
        }
        public ITypeDescriptor Type { get; set; }
    }
}
