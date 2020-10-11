using Stride.Core.Assets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Design
{
    /// <summary>
    /// Describes a ViewModel of an editor for some Assets.
    /// </summary>
    public interface IAssetEditor
    {
        /// <summary>
        /// Asset that is being edited.
        /// </summary>
        Asset Asset { get; }

        /// <summary>
        /// Name of the <see cref="Asset"/>.
        /// </summary>
        string AssetName { get; set; }
    }
}
