using Avalonia.Controls;
using Avalonia.FuncUI;
using Microsoft.FSharp.Core;
using System.Diagnostics;

namespace Stride.Editor.Presentation.VirtualDom
{
    public static class ViewBuilderExtensions
    {
        public static IControl Create(this IViewBuilder viewBuilder)
        {
            return Avalonia.FuncUI.VirtualDom.VirtualDom.create(viewBuilder.Build());
        }

        public static void Update(this IViewBuilder viewBuilder, IControl control, IViewBuilder prevViewBuilder)
        {
            Avalonia.FuncUI.VirtualDom.VirtualDom.update(control, prevViewBuilder.Build(), viewBuilder.Build());
        }

        public static void UpdateRoot(this IViewBuilder viewBuilder, IContentControl control, IViewBuilder prevViewBuilder)
        {
            Debug.Assert(!object.ReferenceEquals(viewBuilder, prevViewBuilder));
            var last = prevViewBuilder == null
                ? FSharpOption<Types.IView>.None
                : FSharpOption<Types.IView>.Some(prevViewBuilder.Build());
            var next = viewBuilder == null
                ? FSharpOption<Types.IView>.None
                : FSharpOption<Types.IView>.Some(viewBuilder.Build());
            Avalonia.FuncUI.VirtualDom.VirtualDom.updateRoot(control, last, next);
        }
    }
}
