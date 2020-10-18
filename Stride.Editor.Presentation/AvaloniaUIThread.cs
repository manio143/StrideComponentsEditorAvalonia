using Avalonia.Threading;
using System;
using System.Threading.Tasks;

namespace Stride.Editor.Presentation
{
    public static class AvaloniaUIThread
    {
        public static async Task InvokeAsync(Action action)
        {
            if (Dispatcher.UIThread.CheckAccess())
                action();
            else
                await Dispatcher.UIThread.InvokeAsync(action);
        }

        public static async Task<T> InvokeAsync<T>(Func<T> action)
        {
            if (Dispatcher.UIThread.CheckAccess())
                return action();
            else
                return await Dispatcher.UIThread.InvokeAsync(action);
        }
    }
}
