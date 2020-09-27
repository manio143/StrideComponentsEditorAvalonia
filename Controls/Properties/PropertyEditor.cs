using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Avalonia.Controls.Properties
{
    public abstract class PropertyEditor : UserControl
    {
        private static readonly Dictionary<Type, Type> CustomEditors = new Dictionary<Type, Type>();

        /// <summary>
        /// Registers a custom editor <typeparamref name="TEditor"/> that handles values of type <paramref name="valueType"/>.
        /// </summary>
        public static void RegisterEditor<TEditor>(Type valueType) where TEditor : PropertyEditor, new()
        {
            CustomEditors.Add(valueType, typeof(TEditor));
        }

        /// <summary>
        /// Get a new instance of an editor for the <paramref name="valueType"/>.
        /// </summary>
        /// <returns>User control that can edit this type of value.</returns>
        public static PropertyEditor GetEditor(Type valueType)
        {
            // check predefined types
            if (valueType.IsValueType)
            {
                if (valueType.IsEnum)
                    return new EnumPropertyEditor();
                if (valueType == typeof(bool))
                    return new BoolPropertyEditor();
                if (valueType == typeof(char))
                    return new CharPropertyEditor();
                if (valueType.IsPrimitive)
                    return new NumberPropertyEditor();
            }
            else
            {
                if (valueType == typeof(string))
                    return new StringPropertyEditor();
            }

            foreach (var kvp in CustomEditors)
            {
                // allows to edit subclasses, but do we want that?
                if (kvp.Key.IsAssignableFrom(valueType))
                    return (PropertyEditor)Activator.CreateInstance(kvp.Value);
            }

            // TODO: the has to be some more complex process of deciding when to show references etc.
            // Otherwise we can get a circular dependency and StackOverflow.
            return new UnsuportedPropertyEditor();
        }

        /// <summary>
        /// Sets <see cref="this.DataContext"/> to <paramref name="property"/> and calls subclass specific initialization.
        /// </summary>
        public void Initialize(PropertyViewModel property)
        {
            this.DataContext = property;
            InitializeContent(property);
        }

        /// <summary>
        /// Process assigned data context.
        /// </summary>
        protected virtual void InitializeContent(PropertyViewModel property) { }
    }
}
