using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.FuncUI;
using Avalonia.FuncUI.DSL;
using Avalonia.Interactivity;
using Microsoft.FSharp.Core;

namespace Stride.Editor.Presentation.VirtualDom
{
    public class PanelViewBuilder<TView> : ViewBuilder<TView> where TView : Panel
    {
        public IViewBuilder ContentMultiple(string property, IEnumerable<Types.IView> content)
        {
            var getter = FSharpValueOption<FSharpFunc<TView, object>>.Some(FuncConvert.FromFunc<TView, object>(control => control.Children));
            var setter = FSharpValueOption<FSharpFunc<Tuple<TView, object>,Unit>>.None;
            var list = Microsoft.FSharp.Collections.ListModule.OfSeq(content);
            Attrs.Add(Avalonia.FuncUI.Builder.AttrBuilder<TView>.CreateContentMultiple(property, getter, setter, list));
            return this;
        }
    }
}
