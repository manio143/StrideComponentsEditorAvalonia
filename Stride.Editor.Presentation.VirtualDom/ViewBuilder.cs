using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.FuncUI;
using Avalonia.FuncUI.DSL;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Microsoft.FSharp.Core;

namespace Stride.Editor.Presentation
{
    public class ViewBuilder<TView> : IViewBuilder, IEquatable<ViewBuilder<TView>>
    {
        protected List<Types.IAttr<TView>> Attrs = new List<Types.IAttr<TView>>();

        public ViewBuilder()
        {
        }

        public IViewBuilder Content(AvaloniaProperty property, Types.IView content)
        {
            var opt = content == null ? FSharpOption<Types.IView>.None
                : FSharpOption<Types.IView>.Some(content);
            Attrs.Add(Avalonia.FuncUI.Builder.AttrBuilder<TView>.CreateContentSingle(property, opt));
            return this;
        }

        public IViewBuilder ContentMultiple(AvaloniaProperty property, IEnumerable<Types.IView> content)
        {
            content = content ?? throw new ArgumentNullException(nameof(content));
            var list = Microsoft.FSharp.Collections.ListModule.OfSeq(content);
            Attrs.Add(Avalonia.FuncUI.Builder.AttrBuilder<TView>.CreateContentMultiple(property, list));
            return this;
        }

        public IViewBuilder Property<TValue>(AvaloniaProperty property, TValue value)
        {
            Attrs.Add(Avalonia.FuncUI.Builder.AttrBuilder<TView>.CreateProperty(property, value, FSharpValueOption<FSharpFunc<Tuple<object, object>, bool>>.None));
            return this;
        }

        public IViewBuilder Property<TValue>(AvaloniaProperty property, TValue value, Func<TValue, TValue, bool> comparer)
        {
            Attrs.Add(Avalonia.FuncUI.Builder.AttrBuilder<TView>.CreateProperty(property, value, FSharpValueOption<FSharpFunc<Tuple<object, object>, bool>>.Some(FuncConvert.FromFunc<Tuple<object, object>, bool>(t => comparer((TValue)t.Item1, (TValue)t.Item2)))));
            return this;
        }

        public IViewBuilder Property<TValue>(string property, TValue value, Func<TView, TValue> getter, Action<TView, TValue> setter, Func<TValue, TValue, bool> comparer)
        {
            var cmp = comparer == null
                ? FSharpValueOption<FSharpFunc<Tuple<object, object>, bool>>.None
                : FSharpValueOption<FSharpFunc<Tuple<object, object>, bool>>.Some(FuncConvert.FromFunc<Tuple<object, object>, bool>(t => comparer((TValue)t.Item1, (TValue)t.Item2)));
            var get = getter == null
                ? FSharpValueOption<FSharpFunc<TView, TValue>>.None
                : FSharpValueOption<FSharpFunc<TView, TValue>>.Some(FuncConvert.FromFunc(getter));
            var set = setter == null
                ? FSharpValueOption<FSharpFunc<Tuple<TView, TValue>, Unit>>.None
                : FSharpValueOption<FSharpFunc<Tuple<TView, TValue>, Unit>>.Some(FuncConvert.FromAction<Tuple<TView, TValue>>(tup => setter(tup.Item1, tup.Item2)));
            Attrs.Add(Avalonia.FuncUI.Builder.AttrBuilder<TView>.CreateProperty(property, value, get, set, cmp));
            return this;
        }

        public IViewBuilder Subscribe<TValue>(AvaloniaProperty<TValue> property, Action<TValue> action, SubPatchOptions? patchOptions = null)
        {
            var option = patchOptions.HasValue ? patchOptions.Value : SubPatchOptions.Never;
            Attrs.Add(Avalonia.FuncUI.Builder.AttrBuilder<TView>.CreateSubscription<TValue>(property, FuncConvert.FromAction(action), FSharpOption<SubPatchOptions>.Some(option)));
            return this;
        }

        public IViewBuilder Subscribe<TValue>(RoutedEvent<TValue> @event, Action<TValue> action, SubPatchOptions? patchOptions = null)
            where TValue : RoutedEventArgs
        {
            var option = patchOptions.HasValue ? patchOptions.Value : SubPatchOptions.Never;
            Attrs.Add(Avalonia.FuncUI.Builder.AttrBuilder<TView>.CreateSubscription<TValue>(@event, FuncConvert.FromAction(action), FSharpOption<SubPatchOptions>.Some(option)));
            return this;
        }

        public Types.IView Build()
        {
            return Avalonia.FuncUI.Builder.ViewBuilder.Create(Microsoft.FSharp.Collections.ListModule.OfSeq(Attrs));
        }

        public bool Equals(ViewBuilder<TView> other)
        {
            if (other == null)
                return false;
            if (Dispatcher.UIThread.CheckAccess())
                return Enumerable.SequenceEqual(Attrs, other.Attrs);
            // If we have any elements that could VerifyAccess() this would get killed.
            // Example: ColumnDefinition.Width checks thread in its getter
            else return Dispatcher.UIThread.InvokeAsync(() => this.Equals(other)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public override bool Equals(object obj)
            => obj != null && obj.GetType() == this.GetType() && Equals((ViewBuilder<TView>)obj);

        public override int GetHashCode() => Attrs.GetHashCode();
    }
}
