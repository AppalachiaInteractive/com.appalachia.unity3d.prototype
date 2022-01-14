using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Scenes
{
    [Serializable]
    public class SceneReferenceElement : AppalachiaObject<SceneReferenceElement>
    {
        #region Fields and Autoproperties

        [SerializeField]
        [FormerlySerializedAs("sceneReference")]
        [ReadOnly]
        public AssetReference reference;

        [SerializeField] public AreaVersion version;

        #endregion

#if UNITY_EDITOR
        [OnValueChanged(nameof(UpdateSelection))]
        [SerializeField]
        public UnityEditor.SceneAsset sceneAsset;

        private void UpdateSelection()
        {
            reference = sceneAsset.ToAssetReference();
            MarkAsModified();
        }

        public void SetSelection(UnityEditor.SceneAsset asset)
        {
            sceneAsset = asset;
            UpdateSelection();
            MarkAsModified();
        }
#endif
    }
}
