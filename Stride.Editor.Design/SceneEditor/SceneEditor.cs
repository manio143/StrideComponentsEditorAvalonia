using Stride.Assets.Entities;
using Stride.Core;
using Stride.Core.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.Editor.Design.SceneEditor
{
    public class SceneEditor : IAssetEditor
    {
        public SceneEditor(SceneAsset sceneAsset, IServiceRegistry services)
        {
            Asset = sceneAsset;
            Scene = new SceneViewModel(sceneAsset);

            // DEBUG: TEST
            SelectedEntity = (EntityViewModel)Scene.Items.First(e => e is EntityViewModel);
            SelectedEntity.IsSelected = true;
        }

        public SceneEditor(SceneAsset sceneAsset, Guid selectedEntity, IServiceRegistry services)
            : this(sceneAsset, services)
        {
            SelectedEntity = Scene.FindById(selectedEntity);
        }

        public SceneViewModel Scene { get; private set; }
        public EntityViewModel SelectedEntity { get; set; }

        public Asset Asset { get; }
        public string AssetName { get; set; }
    }
}
