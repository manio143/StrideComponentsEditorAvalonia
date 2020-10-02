using Avalonia.Controls;
using Stride.Core;
using Stride.Core.Extensions;
using Stride.Editor.Design;
using Stride.Editor.Presentation;
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
            ViewContainer = new ViewContainer(container, ViewBuilder);
            Services = serviceRegistry;
        }

        public IServiceRegistry Services { get; }

        private ViewContainer ViewContainer { get; }

        private Dictionary<Type, Type> viewCache = new Dictionary<Type, Type>();

        public async Task UpdateAssetEditorView(IAssetEditor editor)
        {
            Type editorViewType;
            if (!viewCache.TryGetValue(editor.GetType(), out editorViewType))
            {
                editorViewType = typeof(ViewBase<>).MakeGenericType(editor.GetType()).GetInheritedTypes().First();
                viewCache.Add(editor.GetType(), editorViewType);
            }

            var editorView = (IViewBase)Activator.CreateInstance(editorViewType, Services);
            await ViewContainer.Update(new ViewUpdateContext { Editor = editor, View = editorView });
        }

        private static IViewBuilder ViewBuilder(object context)
        {
            var viewUpdateContext = (ViewUpdateContext)context;
            return viewUpdateContext.View.CreateView(viewUpdateContext.Editor);
        }

        private class ViewUpdateContext
        {
            public IViewBase View { get; set; }
            public IAssetEditor Editor { get; set; }
        }
    }
}
