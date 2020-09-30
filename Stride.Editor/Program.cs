using Avalonia;
using System;
using System.Threading;

namespace Stride.Editor.Avalonia
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
            // run app with more stack space (8MB)
            //var t = new Thread(new ThreadStart(
            //    () => BuildAvaloniaApp()
            //        .StartWithClassicDesktopLifetime(args)),
            //    8 * 1024 * 1024);
            //t.Start();
            //t.Join();
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect();
    }
}
