using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using Stride.Editor.Avalonia.Controls.Properties;

namespace Stride.Editor.Avalonia.EntityHierarchy.Components.Views
{
    public class ComponentMemberView : UserControl
    {
        public ComponentMemberView()
        {
            this.InitializeComponent();
            this.DataContextChanged += ComponentMemberView_DataContextChanged;
        }

        private void ComponentMemberView_DataContextChanged(object sender, EventArgs e)
        {
            var vm = (ComponentMemberViewModel)DataContext;
            var propertyVM = new PropertyViewModel
            {
                Label = vm.Name,
                Type = vm.TypeDescriptor,
                Value = vm.Value,
            };
            propertyVM.ValueChanged += (val) => vm.Value = val;
            Content = new Property() { DataContext = propertyVM };
        }

        public ComponentMemberView(ComponentMemberViewModel vm)
            : this()
        {
            DataContext = vm;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}