using Appalachia.CI.Integration.Assets;
using Appalachia.Prototype.KOC.Application.Scriptables;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Application.Scenes
{
    public class SceneReference : AppalachiaApplicationObject
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("sceneReference")] [ReadOnly] public AssetReference reference;

        public bool setActiveOnLoad;

        #endregion

#if UNITY_EDITOR
        [OnValueChanged(nameof(UpdateSelection))]
        public UnityEditor.SceneAsset sceneAsset;

        public void SetSelection(UnityEditor.SceneAsset asset)
        {
            sceneAsset = asset;
            this.MarkAsModified();
            UpdateSelection();
        }

        private void UpdateSelection()
        {
            AssetDatabaseManager.TryGetGUIDAndLocalFileIdentifier(sceneAsset, out var guid, out var _);

            reference = new AssetReference(guid);
            this.MarkAsModified();
        }

        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + nameof(SceneReference),
            priority = PKG.Menu.Assets.Priority
        )]
        public static void CreateAsset()
        {
            CreateNew<SceneReference>();
        }
#endif
    }
}
