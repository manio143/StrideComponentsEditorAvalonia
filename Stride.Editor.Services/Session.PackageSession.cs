using Stride.Core.Assets;
using Stride.Core.Diagnostics;
using Stride.Editor.Design;
using Stride.Editor.Design.AssetBrowser;
using Stride.Editor.Design.Core;
using Stride.Editor.Design.Core.Docking;
using Stride.Editor.Design.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stride.Editor.Services
{
    // See Stride.Core.Assets.Editor.SessionViewModel for reference implementation
    public partial class Session
    {
        private static LoggingScope LoadProjectScope = LoggingScope.Global($"{nameof(Session)}.{nameof(LoadProject)}");
        private static LoggingScope SaveProjectScope = LoggingScope.Global($"{nameof(Session)}.{nameof(SaveProject)}");

        public event Action<PackageSessionResult> ProjectLoaded;
        public event Action<PackageSessionResult> ProjectSaved;

        public async Task<PackageSessionResult> LoadProject(string path)
        {
            var viewUpdater = Services.GetService<IViewUpdater>();

            PackageSessionResult sessionResult = await SetupResultProgress(viewUpdater, LoadProjectScope);

            using (var scope = new TimedScope(LoadProjectScope, TimedScope.Status.Failure))
            {
                // Force PackageSession.Load to be executed on the thread pool
                // otherwise it would block execution and we want this process to be async
                await Task.Run(() =>
                {
                    PackageSession.Load(path, sessionResult);
                }).ConfigureAwait(false);

                PackageSession = sessionResult.Session;

                foreach (var pkg in PackageSession.LocalPackages)
                    pkg.UpdateAssemblyReferences(LoggingScope.Global($"{nameof(Session)}.{nameof(pkg.UpdateAssemblyReferences)}"));

                if (!sessionResult.HasErrors)
                    scope.Result = TimedScope.Status.Success;
            }

            // Create asset browser for package and add it to the viewmodel
            var browser = new AssetBrowserViewModel(PackageSession);
            await Services.GetService<ITabManager>().CreateToolTab(browser);

            EditorViewModel.LoadingStatus = null;
            await viewUpdater.UpdateView();

            ProjectLoaded?.Invoke(sessionResult);

            return sessionResult;
        }

        private async Task<PackageSessionResult> SetupResultProgress(IViewUpdater viewUpdater, LoggingScope scope)
        {
            EditorViewModel.LoadingStatus = new LoadingStatus(LoadingStatus.LoadingMode.Indeterminate);
            await viewUpdater.UpdateView();

            // in this result will be any errors from loading the project
            var sessionResult = new PackageSessionResult();
            sessionResult.MessageLogged += (_, e) => scope.Log(e.Message);
            sessionResult.ProgressChanged += async (_, e) =>
            {
                if (e.HasKnownSteps && e.CurrentStep > 0)
                {
                    var percentage = EditorViewModel.LoadingStatus.PercentCompleted;
                    var newPercentage = 100 * e.CurrentStep / e.StepCount;
                    EditorViewModel.LoadingStatus.Mode = LoadingStatus.LoadingMode.Percentage;
                    EditorViewModel.LoadingStatus.PercentCompleted = newPercentage;
                }
                else
                {
                    EditorViewModel.LoadingStatus.Mode = LoadingStatus.LoadingMode.Indeterminate;
                }
                EditorViewModel.LoadingStatus.Message = e.Message;

                await viewUpdater.UpdateView();
            };
            return sessionResult;
        }

        public async Task<LoggerResult> SaveProject()
        {
            if (PackageSession == null)
                throw new InvalidOperationException("Cannot save the project before it was loaded.");

            // TODO: check asset consistency

            // TODO: read more about Stride.Core.Assets.Editor.PackageViewModel
            //// Prepare packages to be saved by setting their dirty flag correctly
            //foreach (var package in LocalPackages)
            //{
            //    package.PreparePackageForSaving();
            //}

            var viewUpdater = Services.GetService<IViewUpdater>();

            PackageSessionResult sessionResult = await SetupResultProgress(viewUpdater, SaveProjectScope);

            // TODO: display a dialog with save progress

            // Force PackageSession.Save to be executed on the thread pool
            // otherwise it would block execution and we want this process to be async
            using (var scope = new TimedScope(SaveProjectScope, TimedScope.Status.Success))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        var saveParameters = PackageSaveParameters.Default();

                        // TODO: read more about Stride.Core.Assets.Editor.AssetViewModel
                        // AllAssets.ForEach(x => x.PrepareSave(sessionResult));
                        PackageSession.Save(sessionResult, saveParameters);
                    }
                    catch (Exception e)
                    {
                        sessionResult.Error(string.Format("There was a problem saving the solution. {0}", e.Message), e);
                        scope.Result = TimedScope.Status.Failure;
                    }
                });

                if (sessionResult.HasErrors)
                    scope.Result = TimedScope.Status.Failure;
            }

            EditorViewModel.LoadingStatus = null;
            await viewUpdater.UpdateView();

            ProjectSaved?.Invoke(sessionResult);

            return sessionResult;
        }
    }
}
