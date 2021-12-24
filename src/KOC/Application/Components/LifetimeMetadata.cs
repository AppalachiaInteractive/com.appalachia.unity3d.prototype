using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons;
using Appalachia.Prototype.KOC.Application.Scenes;
using Appalachia.Prototype.KOC.Application.Styling;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Audio;

namespace Appalachia.Prototype.KOC.Application.Components
{
    public sealed class LifetimeMetadata : SingletonAppalachiaObject<LifetimeMetadata>
    {
        static LifetimeMetadata()
        {
        }

        [Serializable]
        public struct ClearCameraSettings
        {
            public bool enabled;
            public Color color; //000000

            public bool enabledEditor;
            public Color colorEditor; //33322B
        }
        
        #region Fields and Autoproperties

        [InlineProperty, HideLabel, BoxGroup("Clear Camera")]
        public ClearCameraSettings clearCamera;

        public AppaLogFormats logFormats;
        public ApplicationUIStyle uiStyle;
        public DeviceButtonLookup deviceButtons;
        public AudioMixer audioMixer;
        public ApplicationStyleElementDefaultLookup styleLookup;

        public MainAreaSceneInformationCollection areaSceneInformationCollection;

        public List<ScriptableObject> criticalReferences;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

#if UNITY_EDITOR
                await initializer.Do(
                    this,
                    nameof(ApplicationUIStyle),
                    uiStyle == null,
                    () =>
                    {
                        if (uiStyle == null)
                        {
                            uiStyle = ApplicationUIStyle.LoadOrCreateNew(nameof(ApplicationUIStyle));
                        }
                    }
                );

                uiStyle.InitializeExternal();

                await initializer.Do(
                    this,
                    nameof(DeviceButtonLookup),
                    deviceButtons == null,
                    () =>
                    {
                        if (deviceButtons == null)
                        {
                            deviceButtons = DeviceButtonLookup.LoadOrCreateNew(nameof(DeviceButtonLookup));
                        }
                    }
                );
#endif

                deviceButtons.InitializeExternal();

                await initializer.Do(
                    this,
                    nameof(MainAreaSceneInformationCollection),
                    areaSceneInformationCollection == null,
                    () => { areaSceneInformationCollection = MainAreaSceneInformationCollection.instance; }
                );

                if (!AppalachiaApplication.IsPlaying)
                {
                    Scan();
                }
            }
        }

        [Button]
        private void Scan()
        {
#if UNITY_EDITOR
            using (_PRF_Scan.Auto())
            {
                if (criticalReferences == null)
                {
                    criticalReferences = new List<ScriptableObject>();
                }

                criticalReferences.Clear();

                var allTypes = ReflectionExtensions.GetAllTypes_CACHED();

                for (var i = 0; i < allTypes.Length; i++)
                {
                    var type = allTypes[i];

                    if (type.GetAttributes_CACHE<CriticalAttribute>(true).Length > 0)
                    {
                        var soPaths = AssetDatabaseManager.GetAllAssetPaths(type);

                        for (var j = 0; j < soPaths.Length; j++)
                        {
                            var path = soPaths[j];

                            var instance =
                                AssetDatabaseManager.LoadAssetAtPath(path, type) as ScriptableObject;

                            if (instance == null)
                            {
                                continue;
                            }

                            criticalReferences.Add(instance);
                        }
                    }
                }
            }
#endif
        }

        #region Profiling

        private const string _PRF_PFX = nameof(LifetimeMetadata) + ".";

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));

        

        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_Scan = new(_PRF_PFX + nameof(Scan));

        #endregion
    }
}
