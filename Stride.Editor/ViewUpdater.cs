using Avalonia.Controls;
using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Presentation;
using Stride.Editor.Services;
using System.Threading.Tasks;

namespace Stride.Editor
{
    public class ViewUpdater : IViewUpdater
    {
        public ViewUpdater(IServiceRegistry serviceRegistry, ContentControl container)
        {
            ViewContainer = new ViewContainer(container, ViewBuilder);
            Services = serviceRegistry;
            Session = Services.GetSafeServiceAs<SessionService>();
            ViewRegistry = Services.GetSafeServiceAs<ViewRegistry>();
        }

        public IServiceRegistry Services { get; }

        private SessionService Session { get; }
        private ViewRegistry ViewRegistry { get; }

        private ViewContainer ViewContainer { get; }

        private static IViewBuilder ViewBuilder(object context)
        {
            var viewUpdateContext = (ViewUpdateContext)context;
            return viewUpdateContext.View.CreateView(viewUpdateContext.Editor);
        }

        public async Task UpdateView(ICommandDispatcher dispatcher)
        {
            dispatcher.Enabled = false;

            var editor = Session.EditorViewModel;
            var editorView = ViewRegistry.GetView(editor, Services);

            await ViewContainer.Update(new ViewUpdateContext { Editor = editor, View = editorView });

            dispatcher.Enabled = true;
        }

        private class ViewUpdateContext
        {
            public IViewBase View { get; set; }
            public object Editor { get; set; }
        }
    }
}
