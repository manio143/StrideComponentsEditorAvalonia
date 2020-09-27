using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Stride.Core.Reflection;
using System.Linq;

namespace Stride.Editor.Avalonia.Controls.Properties
{
    public class StructuredTypePropertyEditor : PropertyEditor
    {
        public StructuredTypePropertyEditor()
        {
            this.InitializeComponent();
        }

        public class Member
        {
            public string Name { get; set; }
            public IMemberDescriptor MemberDescriptor { get; set; }
            public ITypeDescriptor TypeDescriptor => MemberDescriptor.TypeDescriptor;
            public object Owner { get; set; }
            public object Value
            {
                get => MemberDescriptor.Get(Owner);
                set => MemberDescriptor.Set(Owner, value);
            }
        }

        protected override void InitializeContent(PropertyViewModel property)
        {
            var itemsControl = (ItemsControl)Content;
            itemsControl.Items = property.Type.Members.Select(m
                => {
                    var vm = new PropertyViewModel()
                    {
                        Label = m.Name,
                        Type = m.TypeDescriptor,
                        Value = m.Get(property.Value),
                    };
                    vm.ValueChanged += (val) => m.Set(property.Value, val);
                    return new Property() { DataContext = vm };
                });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
