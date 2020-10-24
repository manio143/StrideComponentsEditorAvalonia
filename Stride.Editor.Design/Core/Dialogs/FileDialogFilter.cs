using System.Collections.Generic;

namespace Stride.Editor.Design.Core.Dialogs
{
    public struct FileDialogFilter
    {
        /// <summary>
        /// File type description for the set of allowed extensions.
        /// </summary>
        /// <example>Name = "Image", AllowedExtensions = {"jpg", "png"}</example>
        public string Name { get; set; }

        /// <summary>
        /// Extensions (without the dot) to filter files by.
        /// </summary>
        public IList<string> AllowedExtensions { get; set; }
    }
}
