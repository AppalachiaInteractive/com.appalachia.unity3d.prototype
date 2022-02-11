using Appalachia.CI.Integration.Core;
using Appalachia.Rendering.Prefabs.Rendering;
using Appalachia.Utility.Extensions;
using GPUInstancer;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Appalachia.Prototype.KOC.Lifetime
{
    public partial class LifetimeComponentManager
    {
        #region Constants and Static Readonly

        private const string GROUP_GRAPHICS = GROUP_BASE + PARENT_NAME_GRAPHICS;

        private const string PARENT_NAME_GRAPHICS = "Graphics";

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup(GROUP_GRAPHICS)]
        [SerializeField]
        private PostProcessVolume _postProcessVolume;

        [FoldoutGroup(GROUP_GRAPHICS)]
        [SerializeField]
        private PrefabRenderingManager _prefabRenderingManager;

        [FoldoutGroup(GROUP_GRAPHICS)]
        [SerializeField]
        private GPUInstancerPrefabManager _gpuInstancerPrefabManager;

        [FoldoutGroup(GROUP_GRAPHICS), SerializeField]
        private GameObject _graphicsObject;

        #endregion

        public GameObject GraphicsObject => _graphicsObject;
        public GPUInstancerPrefabManager GpuInstancerPrefabManager => _gpuInstancerPrefabManager;
        public PostProcessVolume PostProcessVolume => _postProcessVolume;
        public PrefabRenderingManager PrefabRenderingManager => _prefabRenderingManager;

        private void InitializeGraphics()
        {
            using (_PRF_InitializeGraphics.Auto())
            {
                gameObject.GetOrAddChild(ref _graphicsObject, PARENT_NAME_GRAPHICS, false);

                _graphicsObject.GetOrAddComponentInChild(ref _postProcessVolume, nameof(PostProcessVolume));

                if (_postProcessVolume.profile == null)
                {
                    var profile = AppalachiaObjectFactory.LoadExistingOrCreateNewAsset<PostProcessProfile>(
                        "LifetimeGlobalProfile",
                        ownerType: typeof(LifetimeComponentManager)
                    );

                    _postProcessVolume.profile = profile;
                }

                _postProcessVolume.isGlobal = true;
                _postProcessVolume.weight = 1.0f;

                _graphicsObject.GetOrAddLifetimeComponentInChild(
                    ref _prefabRenderingManager,
                    nameof(PrefabRenderingManager)
                );
                _graphicsObject.GetOrAddLifetimeComponentInChild(
                    ref _gpuInstancerPrefabManager,
                    nameof(GpuInstancerPrefabManager)
                );

                _prefabRenderingManager.enabled = false;
                _gpuInstancerPrefabManager.enabled = false;
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeGraphics =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeGraphics));

        #endregion
    }
}
