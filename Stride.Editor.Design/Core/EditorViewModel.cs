using Stride.Editor.Design.Core.Menu;
using System.Collections.Generic;

namespace Stride.Editor.Design.Core
{
    public class EditorViewModel
    {
        public EditorViewModel(IMenuProvider menuProvider)
        {
            MenuProvider = menuProvider;
        }

        private IMenuProvider MenuProvider;

        public Dictionary<ITabViewModel, object> Tabs { get; set; } = new Dictionary<ITabViewModel, object>();

        public ITabViewModel ActiveTab { get; set; }

        public IAssetEditor ActiveEditor => ActiveTab != null ? Tabs[ActiveTab] as IAssetEditor : null;

        public MenuViewModel Menu => MenuProvider.GetMenu();

        public LoadingStatus LoadingStatus { get; set; }
    }
}
