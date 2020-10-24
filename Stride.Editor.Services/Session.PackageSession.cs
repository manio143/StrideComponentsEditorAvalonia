using Stride.Core.Assets;
using Stride.Core.Diagnostics;
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
        public async Task<PackageSessionResult> LoadProject(string path)
        {
            // TODO: display a dialog box with load progress

            // in this result will be any errors from loading the project
            var sessionResult = new PackageSessionResult();
            sessionResult.MessageLogged += (_, e) => LoadProjectScope.Log(e.Message);

            using (var scope = new TimedScope(LoadProjectScope, TimedScope.Status.Failure))
            {
                // Force PackageSession.Load to be executed on the thread pool
                // otherwise it would block execution and we want this process to be async
                await Task.Run(() =>
                {
                    PackageSession.Load(path, sessionResult);

                    sessionResult.Session.LoadMissingReferences(sessionResult);
                }).ConfigureAwait(false);

                PackageSession = sessionResult.Session;

                // TODO: Load user assemblies into AppDomain
                foreach (var pkg in PackageSession.LocalPackages)
                    pkg.UpdateAssemblyReferences(LoggingScope.Global($"{nameof(Session)}.{nameof(pkg.UpdateAssemblyReferences)}"));

                if (!sessionResult.HasErrors)
                    scope.Result = TimedScope.Status.Success;
            }

            // TODO: populate EditorViewModel with loaded Assets

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

            var sessionResult = new LoggerResult();

            // TODO: display a dialog with save progress

            // Force PackageSession.Save to be executed on the thread pool
            // otherwise it would block execution and we want this process to be async
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
                }
            });

            return sessionResult;
        }
    }
}
