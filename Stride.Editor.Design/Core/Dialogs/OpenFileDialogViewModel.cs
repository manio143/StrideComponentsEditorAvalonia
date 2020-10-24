using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Design.Core.Dialogs
{
    public class OpenFileDialogViewModel
    {
        public bool AllowMultipleItems { get; set; }

        public IList<FileDialogFilter> Filters { get; set; }

        public string Directory { get; set; }

        public string Title { get; set; }
    }
}
