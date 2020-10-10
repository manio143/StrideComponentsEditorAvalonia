using Avalonia.Controls;
using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Presentation;
using Stride.Editor.Services;
using System.Threading.Tasks;

namespace Stride.Editor
{
    /// <summary>
    /// Updater service responsible for updating the main window UI with the view of the <see cref="Design.Core.EditorViewModel"/>.
    /// </summary>
    public class ViewUpdater : IViewUpdater
    {
        /// <param name="container">Root control of the <see cref="MainWindow"/></param>
        public ViewUpdater(IServiceRegistry serviceRegistry, ContentControl container)
        {
            ViewContainer = new ViewContainer(container, ViewBuilder);
            Services = serviceRegistry;
            Session = Services.GetSafeServiceAs<Session>();
            ViewRegistry = Services.GetSafeServiceAs<ViewRegistry>();
        }

        public IServiceRegistry Services { get; }

        private Session Session { get; }

        private ViewRegistry ViewRegistry { get; }

        private ViewContainer ViewContainer { get; }

        private static IViewBuilder ViewBuilder(object context)
        {
            var viewUpdateContext = (ViewUpdateContext)context;

            viewUpdateContext.CommandDispatcher.Enabled = false;

            return viewUpdateContext.View.CreateView(viewUpdateContext.Editor);
        }

        /// <summary>
        /// Disables the <paramref name="dispatcher"/> for the UI update,
        /// creates a view for the <see cref="Session"/>'s <see cref="Design.Core.EditorViewModel"/>,
        /// updates the window content.
        /// </summary>
        public async Task UpdateView(ICommandDispatcher dispatcher)
        {
            // disabling the dispatcher has been moved to ViewBuilder method which is executed
            // on the UI thread - this prevents premature disabling of dispatcher while
            // the UI thread might still be processing input events
            
            var editor = Session.EditorViewModel;
            var editorView = ViewRegistry.GetView(editor, Services);

            await ViewContainer.Update(new ViewUpdateContext
            {
                Editor = editor,
                View = editorView,
                CommandDispatcher = dispatcher,
            });

            dispatcher.Enabled = true;
        }

        /// <summary>
        /// Context passed to the view factory for <see cref="ViewContainer"/>.
        /// </summary>
        private class ViewUpdateContext
        {
            public IViewBase View { get; set; }
            public object Editor { get; set; }
            public ICommandDispatcher CommandDispatcher { get; set; }
        }
    }
}
