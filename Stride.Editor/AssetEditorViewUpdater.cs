using Avalonia.Controls;
using Avalonia.Threading;
using Stride.Core;
using Stride.Core.Extensions;
using Stride.Editor.Design;
using Stride.Editor.Presentation;
using Stride.Editor.Presentation.VirtualDom;
using Stride.Editor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stride.Editor
{
    public class AssetEditorViewUpdater : IAssetEditorViewUpdater
    {
        public AssetEditorViewUpdater(IServiceRegistry serviceRegistry, ContentControl container)
        {
            this.container = container;
            Services = serviceRegistry;
        }

        public IServiceRegistry Services { get; }

        private ContentControl container;
        private Dictionary<Type, Type> viewCache = new Dictionary<Type, Type>();
        private IViewBuilder lastView;

        public async Task UpdateAssetEditorView(IAssetEditor editor)
        {
            Type editorViewType;
            if (!viewCache.TryGetValue(editor.GetType(), out editorViewType))
            {
                editorViewType = typeof(ViewBase<>).MakeGenericType(editor.GetType()).GetInheritedTypes().First();
                viewCache.Add(editor.GetType(), editorViewType);
            }

            // creating view has to happen on the UI thread because some data structures
            // e.g. Grid.ColumnsDefinitions require that
            if (Dispatcher.UIThread.CheckAccess())
                CreateAndUpdateView(editor, editorViewType);
            else
                await Dispatcher.UIThread.InvokeAsync(() => CreateAndUpdateView(editor, editorViewType));
        }

        private void CreateAndUpdateView(IAssetEditor editor, Type editorViewType)
        {
            var editorView = (IViewBase)Activator.CreateInstance(editorViewType, Services);
            var view = editorView.CreateView(editor);

            view.UpdateRoot(container, lastView);

            lastView = view;
        }
    }
}
