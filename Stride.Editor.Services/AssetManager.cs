using Stride.Core;
using Stride.Core.Assets;
using Stride.Core.Diagnostics;
using Stride.Editor.Design;
using Stride.Editor.Design.Core.Docking;
using Stride.Editor.Design.Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Stride.Editor.Services
{
    /// <summary>
    /// Manager of assets, asset editors, etc.
    /// </summary>
    public class AssetManager : IAssetManager, IAssetEditorRegistry
    {
        private static LoggingScope Logger = LoggingScope.Global(nameof(AssetManager));
        public AssetManager(IServiceRegistry services)
        {
            Services = services;
            Session = services.GetSafeServiceAs<Session>();
            TabManager = services.GetSafeServiceAs<ITabManager>();
        }

        private IServiceRegistry Services { get; }
        private ITabManager TabManager { get; }

        private Session Session { get; }

        private readonly List<(Type assetType, Type editorType)> editorRegistry = new List<(Type assetType, Type editorType)>();

        private readonly Dictionary<Asset, AssetRecord> openedAssets = new Dictionary<Asset, AssetRecord>();
        private readonly List<IAssetEditor> openedEditors = new List<IAssetEditor>();

        public event Action<Asset, IAssetEditor> AssetOpened;

        /// <inheritdoc/>
        public void OpenAsset(AssetItem assetItem, Type editorType = null)
        {
            Logger.Info($"Opening asset {assetItem.Asset.GetType()} \"{assetItem.Id}\"...");
            if (editorType == null)
                editorType = GetDefaultEditor(assetItem.Asset.GetType());

            Asset asset = GetOrOpenAsset(assetItem);

            var openedEditorsForAsset = openedEditors.Where(ed => ed.Asset == asset);
            var openedEditorOfDesiredType = openedEditorsForAsset.FirstOrDefault(ed => ed.GetType() == editorType);

            if (openedEditorOfDesiredType == null)
            {
                Logger.Info($"Creating new editor '{editorType.FullName}' for asset \"{assetItem.Id}\"...");
                var newEditor = CreateEditorInstance(editorType, asset);
                newEditor.AssetName = assetItem.Location.GetFileNameWithoutExtension();
                openedEditors.Add(newEditor);
                TabManager.CreateEditorTab(newEditor);

                AssetOpened?.Invoke(asset, newEditor);
            }
            else
            {
                var tab = Session.EditorViewModel.Tabs.Keys.First(t => t.ViewModel == openedEditorOfDesiredType);
                TabManager.FocusTab(tab);
            }
        }

        private Asset GetOrOpenAsset(AssetItem assetItem)
        {
            var opened = openedAssets.Keys.FirstOrDefault(ai => ai.Id == assetItem.Id);
            if (opened == null)
            {
                openedAssets.Add(assetItem.Asset, new AssetRecord { AssetItem = assetItem });
                return assetItem.Asset;
            }
            else
            {
                return opened;
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
        public void PushChange(Asset asset, Guid changeId)
        {
            var record = openedAssets[asset];
            record.Changes.Push(changeId);
            record.AssetItem.IsDirty = record.IsDirty;
        }

        /// <inheritdoc/>
        public void PopChange(Asset asset, Guid changeId)
        {
            var record = openedAssets[asset];
            var id = record.Changes.Pop();
            Debug.Assert(id == changeId);
            record.AssetItem.IsDirty = record.IsDirty;
        }

        public void SaveAsset(Asset asset)
        {
            if (!openedAssets.ContainsKey(asset))
                throw new ArgumentException($"The specified asset '{asset.Id}' has not been opened", nameof(asset));

            Logger.Info($"Saving asset '{asset.Id}'...");

            var rec = openedAssets[asset];

            if (!rec.IsDirty)
            {
                Logger.Verbose("Asset not dirty, skipping.");
                return;
            }

            var result = new LoggerResult();
            result.MessageLogged += (_, e) => Logger.Log(e.Message);
            Package.SaveSingleAsset(rec.AssetItem, result);
            //TODO: handle errors

            rec.MarkSaved();
        }

        /// <inheritdoc/>
        public async void SaveAll()
        {
            var result = await Session.SaveProject();

            if (!result.HasErrors)
                foreach (var rec in openedAssets.Values)
                    rec.MarkSaved();
        }

        /// <inheritdoc/>
        public void RegisterAssetEditor<TAsset, TAssetEditor>()
            where TAsset : Asset
            where TAssetEditor : IAssetEditor
        {
            if (editorRegistry.Any(reg => reg.editorType == typeof(TAssetEditor)))
                throw new InvalidOperationException($"An editor of type '{typeof(TAssetEditor).FullName}' has already been registered.");

            Logger.Info($"Register editor '{typeof(TAssetEditor).FullName}' for assets of type '{typeof(TAsset)}'.");
            editorRegistry.Add((typeof(TAsset), typeof(TAssetEditor)));
        }

        /// <inheritdoc/>
        public void UnregisterAssetEditor<TAssetEditor>() where TAssetEditor : IAssetEditor
        {
            var entry = editorRegistry.FirstOrDefault(reg => reg.editorType == typeof(TAssetEditor));

            if (entry.editorType == null)
                throw new InvalidOperationException($"An editor of type '{typeof(TAssetEditor).FullName}' has not been registered.");

            Logger.Info($"Unregister editor '{typeof(TAssetEditor).FullName}'.");
            editorRegistry.Remove(entry);
        }

        private class AssetRecord
        {
            public AssetItem AssetItem { get; set; }

            /// <summary>
            /// Stack of applied changes.
            /// </summary>
            public Stack<Guid> Changes { get; set; } = new Stack<Guid>();

            /// <summary>
            /// Id of the last change before save.
            /// </summary>
            public Guid? LastSavedChange { get; set; }

            public void MarkSaved() => LastSavedChange = Changes.Peek();

            /// <summary>
            /// Has there been any changes since last saved.
            /// </summary>
            public bool IsDirty
            {
                get
                {
                    if (Changes.Count == 0)
                        return false;
                    if (LastSavedChange.HasValue)
                        return Changes.Peek() != LastSavedChange.Value;
                    return true;
                }
            }
        }
    }
}
