using System;
using Appalachia.CI.Integration.Assets;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Appalachia.Prototype.KOC.Application.References
{
    [Serializable]
    public class AppaAsset
    {
        #region Profiling

        private const string _PRF_PFX = nameof(AppaAsset) + ".";

        private static readonly ProfilerMarker _PRF_GetReference =
            new ProfilerMarker(_PRF_PFX + nameof(GetReference));

        #endregion

        #region Fields

        [SerializeField, ReadOnly]
        private AssetReferenceGameObject _assetReference;

#if UNITY_EDITOR
        [OnValueChanged(nameof(UpdateReference))]
        [SerializeField]
        private GameObject _directReference;
#endif

        //[NonSerialized] 
        private GameObject _instance;

        #endregion

        public AsyncOperationHandle<GameObject> GetReference(Transform parent, Action<GameObject> callback)
        {
            using (_PRF_GetReference.Auto())
            {
                if (_instance != null)
                {
                    return default;
                }

#if UNITY_EDITOR
                if (UnityEngine.Application.isPlaying)
                {
#endif
                    Action<AsyncOperationHandle<GameObject>> completionHandler =
                        handle => callback(handle.Result);

                    var operation = _assetReference.InstantiateIfNull(
                        _instance,
                        go => _instance = go,
                        completionHandler,
                        parent
                    );

                    return operation;
#if UNITY_EDITOR
                }

                if (_instance == null)
                {
                    _instance = PrefabUtility.InstantiatePrefab(_directReference) as GameObject;
                }

                _instance.transform.SetParent(parent);
                _instance.gameObject.hideFlags = HideFlags.DontSave;

                return default;
#endif
            }
        }

#if UNITY_EDITOR
        private void UpdateReference()
        {
            if (_directReference == null)
            {
                _assetReference = null;
                return;
            }

            _directReference.SetAddressableGroup();

            var directReferenceGuid = AssetDatabaseManager.GetAssetGuid(_directReference);
            _assetReference = new AssetReferenceGameObject(directReferenceGuid);
        }
#endif
    }
}
