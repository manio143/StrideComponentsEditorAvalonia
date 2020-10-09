using Stride.Core;
using Stride.Editor.Design;
using Stride.Editor.Design.SceneEditor;
using Stride.Editor.Presentation;
using Stride.Editor.Presentation.SceneEditor;

namespace Stride.Editor.Plugins
{
    public class SceneEditorPlugin : IEditorPlugin
    {
        public void RegisterPlugin(IServiceRegistry services)
        {
            var viewRegistry = services.GetSafeServiceAs<ViewRegistry>();
            viewRegistry.RegisterView<SceneEditor, SceneEditorView>();

            // TODO: register SceneEditor as an editor of SceneAsset
        }
    }
}
