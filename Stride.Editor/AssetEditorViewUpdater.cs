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

        public void UpdateAssetEditorView(IAssetEditor editor)
        {
            Type editorViewType;
            if (!viewCache.TryGetValue(editor.GetType(), out editorViewType))
            {
                editorViewType = typeof(ViewBase<>).MakeGenericType(editor.GetType()).GetInheritedTypes().First();
                viewCache.Add(editor.GetType(), editorViewType);
            }

            var editorView = (IViewBase)Activator.CreateInstance(editorViewType, Services);
            var view = editorView.CreateView(editor);

            if (Dispatcher.UIThread.CheckAccess())
                view.UpdateRoot(container, lastView);
            else
                Dispatcher.UIThread.Post(() => view.UpdateRoot(container, lastView));

            lastView = view;
        }
    }
}
