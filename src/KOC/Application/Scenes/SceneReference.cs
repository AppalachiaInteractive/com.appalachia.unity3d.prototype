using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

namespace Appalachia.Prototype.KOC.Application.Scenes
{
    public class SceneReference  : AppalachiaObject
    {
        #region Fields and Autoproperties

        [ReadOnly] public AssetReference sceneReference;

        public bool setActiveOnLoad;

        #endregion

#if UNITY_EDITOR
        [OnValueChanged(nameof(UpdateSelection))]
        public UnityEditor.SceneAsset sceneAsset;

        public void SetSelection(UnityEditor.SceneAsset asset)
        {
            sceneAsset = asset;
            UpdateSelection();
        }

        private void UpdateSelection()
        {
            AssetDatabaseManager.TryGetGUIDAndLocalFileIdentifier(sceneAsset, out var guid, out long _);

            sceneReference = new AssetReference(guid);
            SetDirty();
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
