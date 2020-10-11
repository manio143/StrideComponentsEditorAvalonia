using Stride.Core;
using Stride.Core.Assets;
using Stride.Editor.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.Editor.Services
{
    /// <summary>
    /// Manager of assets, asset editors, etc.
    /// </summary>
    public class AssetManager : IAssetManager, IAssetEditorRegistry
    {
        public AssetManager(IServiceRegistry services)
        {
            Services = services;
            Session = services.GetSafeServiceAs<Session>();
        }

        private IServiceRegistry Services { get; }

        private Session Session { get; }

        private readonly List<(Type assetType, Type editorType)> editorRegistry = new List<(Type assetType, Type editorType)>();

        private readonly List<AssetItem> openedAssets = new List<AssetItem>();
        private readonly List<IAssetEditor> openedEditors = new List<IAssetEditor>();

        /// <inheritdoc/>
        public void OpenAsset(AssetItem assetItem, Type editorType = null)
        {
            if (editorType == null)
                editorType = GetDefaultEditor(assetItem.Asset.GetType());

            Asset asset = GetOrOpenAsset(assetItem);

            var openedEditorsForAsset = openedEditors.Where(ed => ed.Asset == asset);
            var openedEditorOfDesiredType = openedEditorsForAsset.FirstOrDefault(ed => ed.GetType() == editorType);

            if (openedEditorOfDesiredType == null)
            {
                var newEditor = CreateEditorInstance(editorType, asset);
                newEditor.AssetName = assetItem.Location.GetFileNameWithoutExtension();
                openedEditors.Add(newEditor);
                // TODO: add editor to the Session.EditorViewModel
            }
            else
            {
                // TODO: focus existing editor
            }
            // check if this asset (by Id) has been opened with this editor
            // if not, open a new one (_next to the last opened one_)
        }

        private Asset GetOrOpenAsset(AssetItem assetItem)
        {
            var opened = openedAssets.FirstOrDefault(ai => ai.Id == assetItem.Id);
            if (opened == null)
            {
                // make a copy that will be edited
                var clonedAsset = assetItem.Clone();
                openedAssets.Add(clonedAsset);
                return clonedAsset.Asset;
            }
            else
            {
                return opened.Asset;
            }
        }

        private IAssetEditor CreateEditorInstance(Type editorType, Asset asset)
        {
            var ctor = editorType.GetConstructor(new[] { asset.GetType(), typeof(IServiceRegistry) });
            if (ctor != null)
            {
                return (IAssetEditor)ctor.Invoke(new object[] { asset, Services });
            }
            throw new Exception($"Registered editor '{editorType.FullName}' does not have a constructor ({asset.GetType()} asset, {typeof(IServiceRegistry)} services)");
        }

        private Type GetDefaultEditor(Type type)
        {
            var entries = editorRegistry.Where(reg => reg.assetType == type).ToList();

            if (entries.Count == 0)
                throw new InvalidOperationException($"No editor has been registered for asset type '{type}'.");

            // The first registered editor is the default one
            return entries.First().editorType;
        }

        /// <inheritdoc/>
        public void RegisterAssetEditor<TAsset, TAssetEditor>()
            where TAsset : Asset
            where TAssetEditor : IAssetEditor
        {
            if (editorRegistry.Any(reg => reg.editorType == typeof(TAssetEditor)))
                throw new InvalidOperationException($"An editor of type '{typeof(TAssetEditor).FullName}' has already been registered.");

            editorRegistry.Add((typeof(TAsset), typeof(TAssetEditor)));
        }

        /// <inheritdoc/>
        public void UnregisterAssetEditor<TAssetEditor>() where TAssetEditor : IAssetEditor
        {
            var entry = editorRegistry.FirstOrDefault(reg => reg.editorType == typeof(TAssetEditor));

            if (entry.editorType == null)
                throw new InvalidOperationException($"An editor of type '{typeof(TAssetEditor).FullName}' has not been registered.");

            editorRegistry.Remove(entry);
        }
    }
}
