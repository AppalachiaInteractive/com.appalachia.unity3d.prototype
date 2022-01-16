using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Components.OnScreenButtons;
using Appalachia.Prototype.KOC.Components.Styling;
using Appalachia.Prototype.KOC.Scenes;
using Appalachia.UI.Core.Styling;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Audio;

namespace Appalachia.Prototype.KOC.Components
{
    public sealed class LifetimeMetadata : SingletonAppalachiaObject<LifetimeMetadata>
    {
        static LifetimeMetadata()
        {
            

            RegisterDependency<MainAreaSceneInformationCollection>(
                i => mainAreaSceneInformationCollection = i
            );
        }

        #region Static Fields and Autoproperties

        public static MainAreaSceneInformationCollection mainAreaSceneInformationCollection;

        #endregion

        #region Fields and Autoproperties

        [InlineProperty, HideLabel, BoxGroup("Clear Camera")]
        public ClearCameraSettings clearCamera;

        public ApplicationUIStyle uiStyle;
        public DeviceButtonLookup deviceButtons;
        public AudioMixer audioMixer;
        public StyleElementDefaultLookup styleLookup;

        public List<ScriptableObject> criticalReferences;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

#if UNITY_EDITOR
            initializer.Do(
                this,
                nameof(ApplicationUIStyle),
                uiStyle == null,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        if (uiStyle == null)
                        {
                            uiStyle = LoadOrCreateNew<ApplicationUIStyle>(nameof(ApplicationUIStyle));
                        }
                    }
                }
            );

            initializer.Do(
                this,
                nameof(DeviceButtonLookup),
                deviceButtons == null,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        if (deviceButtons == null)
                        {
                            deviceButtons = LoadOrCreateNew<DeviceButtonLookup>(nameof(DeviceButtonLookup));
                        }
                    }
                }
            );

            using (_PRF_Initialize.Auto())
            {
                if (!AppalachiaApplication.IsPlaying)
                {
                    Scan();
                }
            }
#endif
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

        #region Nested type: ClearCameraSettings

        [Serializable]
        public struct ClearCameraSettings
        {
            #region Fields and Autoproperties

            public bool enabled;

            public bool enabledEditor;
            public Color color;       //000000
            public Color colorEditor; //33322B

            #endregion
        }

        #endregion

        #region Profiling

        

        private static readonly ProfilerMarker _PRF_Scan = new(_PRF_PFX + nameof(Scan));

        #endregion
    }
}
