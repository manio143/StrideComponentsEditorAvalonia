using Stride.Editor.Design.Core.Menu;
using System.Collections.Generic;

namespace Stride.Editor.Design.Core
{
    public class EditorViewModel
    {
        public EditorViewModel()
        {
        }

        public Dictionary<ITabViewModel, object> Tabs { get; set; } = new Dictionary<ITabViewModel, object>();

        public ITabViewModel ActiveTab { get; set; }

        public IAssetEditor ActiveEditor => Tabs[ActiveTab] as IAssetEditor;

        public IList<MenuItemViewModel> Menu { get; set; }
    }
}
