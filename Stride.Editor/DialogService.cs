using Avalonia.Controls;
using Stride.Core;
using Stride.Editor.Design;
using Stride.Editor.Design.Core.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stride.Editor
{
    public class DialogService : IDialogService
    {
        public DialogService(IServiceRegistry services)
        {
            Services = services;
            MainWindow = services.GetSafeServiceAs<Window>();
        }

        private IServiceRegistry Services { get; }
        private Window MainWindow { get; }

        public Task<string[]> OpenFileDialog(OpenFileDialogViewModel viewModel, object window = null)
        {
            return new OpenFileDialog
            {
                AllowMultiple = viewModel.AllowMultipleItems,
                Directory = viewModel.Directory,
                Filters = viewModel.Filters.Select(filter => new Avalonia.Controls.FileDialogFilter
                    {
                        Name = filter.Name,
                        Extensions = filter.AllowedExtensions.ToList(),
                    }).ToList(),
                Title = viewModel.Title,
            }.ShowAsync(window as Window ?? MainWindow);
        }
    }
}
