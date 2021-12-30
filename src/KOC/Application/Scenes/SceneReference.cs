using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Prototype.KOC.Application.Scenes.Collections;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Appalachia.Prototype.KOC.Application.Scenes
{
    public class SceneReference : AppalachiaObject<SceneReference>
    {
        #region Fields and Autoproperties

        [SerializeField] public AreaVersion currentVersion;

        [Title("Elements")]
        [SerializeField, HideLabel, InlineProperty]
        public SceneReferenceElementLookup elements;

        #endregion

        public AssetReference reference => elements[currentVersion].reference;

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                elements ??= new SceneReferenceElementLookup();

                elements.SetObjectOwnership(this);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(SceneReference) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion

#if UNITY_EDITOR

        private static readonly ProfilerMarker _PRF_IsDataValid =
            new ProfilerMarker(_PRF_PFX + nameof(IsDataValid));

        protected override bool IsDataValid()
        {
            using (_PRF_IsDataValid.Auto())
            {
                return (currentVersion != AreaVersion.None) || (elements == null) || (elements.Count == 0);
            }
        }

        public void SetSelection(AreaVersion version, UnityEditor.SceneAsset asset)
        {
            using (_PRF_SetSelection.Auto())
            {
                elements ??= new SceneReferenceElementLookup();

                var sceneReference = SceneReferenceElement.LoadOrCreateNew(asset.name);

                sceneReference.version = version;
                sceneReference.SetSelection(asset);

                elements.AddOrUpdate(version, sceneReference);
                MarkAsModified();
            }
        }

        private static readonly ProfilerMarker _PRF_SetSelection =
            new ProfilerMarker(_PRF_PFX + nameof(SetSelection));

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
