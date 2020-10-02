using Avalonia.Controls;
using Avalonia.Threading;
using Stride.Editor.Presentation.VirtualDom;
using System;
using System.Threading.Tasks;

namespace Stride.Editor.Presentation
{
    public class ViewContainer
    {
        public ViewContainer(ContentControl container, Func<object, IViewBuilder> viewFactory)
        {
            Container = container;
            ViewFactory = viewFactory;
        }

        private ContentControl Container { get; }
        private Func<object, IViewBuilder> ViewFactory { get; }
        private IViewBuilder LastView { get; set; }

        public async Task Update(object context)
        {
            // creating view has to happen on the UI thread because some data structures
            // e.g. Grid.ColumnsDefinitions require that
            if (Dispatcher.UIThread.CheckAccess())
                CreateAndUpdateView(context);
            else
                await Dispatcher.UIThread.InvokeAsync(() => CreateAndUpdateView(context));
        }

        private void CreateAndUpdateView(object context)
        {
            var newView = ViewFactory(context);
            newView.UpdateRoot(Container, LastView);

            LastView = newView;
        }
    }
}
