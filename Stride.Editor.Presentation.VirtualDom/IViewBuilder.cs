using Avalonia;
using Avalonia.FuncUI;
using Avalonia.FuncUI.DSL;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;

namespace Stride.Editor.Presentation
{
    public interface IViewBuilder
    {
        Types.IView Build();
        IViewBuilder Content(AvaloniaProperty property, Types.IView content);
        IViewBuilder ContentMultiple(AvaloniaProperty property, IEnumerable<Types.IView> content);
        IViewBuilder Property<TValue>(AvaloniaProperty property, TValue value);
        IViewBuilder Property<TValue>(AvaloniaProperty property, TValue value, Func<TValue, TValue, bool> comparer);
        IViewBuilder Subscribe<TValue>(AvaloniaProperty<TValue> property, Action<TValue> action, SubPatchOptions? patchOptions = null);
        IViewBuilder Subscribe<TValue>(RoutedEvent<TValue> @event, Action<TValue> action, SubPatchOptions? patchOptions = null) where TValue : RoutedEventArgs;
    }
}