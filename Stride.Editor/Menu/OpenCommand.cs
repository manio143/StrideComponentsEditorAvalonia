using Stride.Core;
using Stride.Editor.Design.Core.Dialogs;
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

            await Session.LoadProject(path);
        }
    }
}
