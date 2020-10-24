using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
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

        private static LoggingScope Logger = LoggingScope.Global(nameof(ViewDataTemplate));

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
            if (viewModel is ITabViewModel dock)
            {
                id = dock.Id;
                if (dock.RequiresViewRefresh)
                {
                    Logger.Verbose($"Tab requires refresh '{id}'");
                    views.Remove(id);
                    dock.RequiresViewRefresh = false;
                }
            }
            else if (viewModel is EditorViewModel)
                id = "root";
            else throw new NotSupportedException();

            Logger.Debug($"Building view for object '{id}'.");

            ViewContainer container;
            if (!views.TryGetValue(id, out container))
            {
                await AvaloniaUIThread.InvokeAsync(() =>
                {
                    container = new ViewContainer(new ContentControl(), CreateView, Cleanup);
                });
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
            return data is ITabViewModel || data is EditorViewModel;
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
            // do not allow running more than one update at a time
            // under "normal" conditions this will never happen
            // but if it does, we're assuming changes don't introduce breaking inconsistency
            if (UpdateViewTask?.IsCompleted != false)
                UpdateViewTask = UpdateViewInternal();

            await UpdateViewTask;
        }

        private Task UpdateViewTask;

        private async Task UpdateViewInternal()
        {
            using var scope = new TimedScope(UpdateViewScope);
            try
            {
                var editorVM = RootVMContainer.RootViewModel;
                await BuildAsync(editorVM);

                foreach (var tab in editorVM.Tabs.Keys)
                    await BuildAsync(tab);
            }
            catch (Exception e)
            {
                scope.Result = TimedScope.Status.Failure;
                scope.Error(e.Message, e);
                throw;
            }
        }

        private static LoggingScope UpdateViewScope = LoggingScope.Global($"{nameof(ViewDataTemplate)}.{nameof(UpdateView)}");

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
