using Avalonia.Controls;
using Stride.Core;
using Stride.Core.Extensions;
using Stride.Editor.Presentation;
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
        public void UpdateAssetEditorView<TEditor>(TEditor editor)
        {
            Type editorViewType;
            if (!viewCache.TryGetValue(typeof(TEditor), out editorViewType))
            {
                editorViewType = typeof(ViewBase<TEditor>).GetInheritedTypes().First();
                viewCache.Add(typeof(TEditor), editorViewType);
            }

            var editorView = (ViewBase<TEditor>)Activator.CreateInstance(editorViewType, Services);
            container.Content = editorView.CreateView(editor);
        }
    }
}
