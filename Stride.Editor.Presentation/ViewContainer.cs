using Avalonia.Controls;
using Avalonia.Threading;
using Stride.Core.Diagnostics;
using Stride.Editor.Design.Core.Logging;
using Stride.Editor.Presentation.VirtualDom;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Stride.Editor.Presentation
{
    /// <summary>
    /// View container which performs the UI control update from the virtual view.
    /// </summary>
    public class ViewContainer
    {
        public ViewContainer(ContentControl container, Func<object, IViewBuilder> viewFactory, Action<object> cleanup)
        {
            Container = container;
            ViewFactory = viewFactory;
            Cleanup = cleanup;
        }

        public ContentControl Container { get; }
        private Func<object, IViewBuilder> ViewFactory { get; }
        private Action<object> Cleanup { get; }
        private IViewBuilder LastView { get; set; }

        private static LoggingScope Logger = LoggingScope.Global(nameof(ViewContainer));

        /// <summary>
        /// On the UI thread, creates a view from the <see cref="ViewFactory"/>, updates UI, calls <see cref="Cleanup"/>.
        /// </summary>
        /// <param name="context">Context passed to user methods</param>
        public async Task Update(object context)
        {
            // creating view has to happen on the UI thread because some data structures
            // e.g. Grid.ColumnsDefinitions require that
            await AvaloniaUIThread.InvokeAsync(() => CreateAndUpdateView(context));
        }

        private void CreateAndUpdateView(object context)
        {
            var newView = ViewFactory(context);

            Logger.Debug("Updating UI container...");
            DebugCheckIfEqual(LastView, newView);
            newView.UpdateRoot(Container, LastView);

            LastView = newView;
            Logger.Debug("UI updated. Last virtual view saved.");

            Cleanup(context);
        }

        [Conditional("DEBUG")]
        private void DebugCheckIfEqual(IViewBuilder last, IViewBuilder @new)
        {
            if (@new.Build().Equals(last?.Build()))
                Logger.Debug("The views are equal.");
            else
                Logger.Debug("The views differ.");
        }
    }
}
