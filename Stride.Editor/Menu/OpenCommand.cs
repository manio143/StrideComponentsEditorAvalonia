using Stride.Core;
using Stride.Editor.Design;
using Stride.Editor.Design.AssetBrowser;
using Stride.Editor.Design.Core;
using Stride.Editor.Design.Core.Dialogs;
using Stride.Editor.Presentation.Core.Docking;
using Stride.Editor.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stride.Editor.Menu
{
    public class OpenCommand : MenuCommandBase
    {
        public OpenCommand(IServiceRegistry services) : base(services)
        {
            Session = services.GetSafeServiceAs<Session>();
        }

        private Session Session { get; }

        protected override async Task ExecuteAsync(object parameter)
        {
            var viewUpdater = Services.GetService<IViewUpdater>();
            var dialogService = Services.GetService<IDialogService>();

            var dialogSettings = new OpenFileDialogViewModel
            {
                Title = "Open project...",
                Directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filters = new[]
                {
                    new FileDialogFilter
                    {
                        Name = "Solution file",
                        AllowedExtensions = new[] { "sln" },
                    }
                },
                AllowMultipleItems = false,
            };

            var path = (await dialogService.OpenFileDialog(dialogSettings)).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(path))
                return;

            Session.EditorViewModel.LoadingStatus = new LoadingStatus(LoadingStatus.LoadingMode.Indeterminate);
            await viewUpdater.UpdateView();

            await Session.LoadProject(path);

            Session.EditorViewModel.LoadingStatus = null;
            var browser = new AssetBrowserViewModel(Session.PackageSession);
            await Services.GetService<TabManager>().CreateToolTab(browser);
            await viewUpdater.UpdateView();
        }
    }
}
