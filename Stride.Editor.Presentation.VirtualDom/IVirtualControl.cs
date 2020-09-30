using Avalonia;
using Avalonia.Controls;

namespace Stride.Editor.Presentation.VirtualDom
{
    public interface IVirtualControl
    {
    }

    public interface IVirtualControl<TControl> where TControl : AvaloniaObject, IControl, new()
    {
    }
}
