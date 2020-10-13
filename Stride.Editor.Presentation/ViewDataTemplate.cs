using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Dock.Model;
using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Editor.Commands;
using Stride.Editor.Design;
using Stride.Editor.Design.Core;
using Stride.Editor.Design.Core.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stride.Editor.Presentation
{
    public class ViewDataTemplate : IDataTemplate, IViewUpdater
    {
        public ViewDataTemplate(IServiceRegistry services)
        {
            Services = services;
            ViewRegistry = services.GetSafeServiceAs<ViewRegistry>();
            RootVMContainer = services.GetSafeServiceAs<IRootViewModelContainer>();
        }

        private static LoggingScope Logger = new LoggingScope(GlobalLogger.GetLogger(nameof(ViewDataTemplate)));

        static ViewDataTemplate() { Logger.ActivateLog(LogMessageType.Debug); }

        private ViewRegistry ViewRegistry { get; }

        private IServiceRegistry Services { get; }

        private IRootViewModelContainer RootVMContainer { get; }

        public bool SupportsRecycling => false; // we do our own recycling

        private readonly Dictionary<string, ViewContainer> views = new Dictionary<string, ViewContainer>();

        public IControl Build(object viewModel)
        {
            return BuildAsync(viewModel).Result;
        }

        public async Task<IControl> BuildAsync(object viewModel)
        {
            string id = null;
            if (viewModel is IDockable dock)
                id = dock.Id;
            else if (viewModel is EditorViewModel)
                id = "root";
            else throw new NotSupportedException();

            Logger.Debug($"Building view for object '{id}'.");

            ViewContainer container;
            if (!views.TryGetValue(id, out container))
            {
                container = new ViewContainer(new ContentControl(), CreateView, Cleanup);
                views.Add(id, container);
            }

            var context = new ViewUpdateContext
            {
                CommandDispatcher = Services.GetSafeServiceAs<ICommandDispatcher>(),
                ViewModel = viewModel,
                View = ViewRegistry.GetView(viewModel, Services),
            };

            await container.Update(context);
            return container.Container;
        }

        public bool Match(object data)
        {
            return data is IDockable || data is EditorViewModel;
        }

        private static IViewBuilder CreateView(object context)
        {
            var viewUpdateContext = (ViewUpdateContext)context;

            Logger.Debug($"Start creating view for '{viewUpdateContext.ViewModel.GetType()}'.");

            // We need to disable the dispatcher as events may fire during FuncUI subscription update
            viewUpdateContext.CommandDispatcher.Enabled = false;

            return viewUpdateContext.View.CreateView(viewUpdateContext.ViewModel);
        }

        private static void Cleanup(object context)
        {
            var viewUpdateContext = (ViewUpdateContext)context;

            viewUpdateContext.CommandDispatcher.Enabled = true;

            Logger.Debug($"Finish creating view for '{viewUpdateContext.ViewModel.GetType()}'.");
        }

        /// <summary>
        /// Disables the <paramref name="dispatcher"/> for the UI update,
        /// creates a view for the <see cref="RootVMContainer"/>'s <see cref="Design.Core.EditorViewModel"/>,
        /// updates the window content.
        /// </summary>
        public async Task UpdateView()
        {
            using (var scope = new TimedScope(UpdateViewScope))
            {
                try
                {
                    var editorVM = RootVMContainer.RootViewModel;
                    await BuildAsync(editorVM);

                    foreach (var tab in editorVM.Tabs.Keys)
                        await BuildAsync(tab);
                }
                catch(Exception e)
                {
                    scope.Result = TimedScope.Status.Failure;
                    scope.Error(e.Message, e);
                    throw;
                }
            }
        }

        private static LoggingScope UpdateViewScope = new LoggingScope(GlobalLogger.GetLogger($"{nameof(ViewDataTemplate)}.{nameof(UpdateView)}"));

        /// <summary>
        /// Context passed to the view factory for <see cref="ViewContainer"/>.
        /// </summary>
        private class ViewUpdateContext
        {
            public IViewBase View { get; set; }
            public object ViewModel { get; set; }
            public ICommandDispatcher CommandDispatcher { get; set; }
        }
    }
}
