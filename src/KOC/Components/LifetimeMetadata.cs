using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Components.OnScreenButtons;
using Appalachia.Prototype.KOC.Components.Styling;
using Appalachia.Prototype.KOC.Scenes;
using Appalachia.UI.Controls.Sets.Background;
using Appalachia.UI.Controls.Sets.RootCanvas;
using Appalachia.UI.Core.Styling;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
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

        public RootCanvasComponentSetStyle rootCanvas;

        public BackgroundComponentSetStyle rootBackground;

        public ApplicationUIStyle uiStyle;
        public DeviceButtonLookup deviceButtons;
        public AudioMixer audioMixer;
        public StyleElementDefaultLookup styleLookup;


        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

#if UNITY_EDITOR
            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
                    this,
                    nameof(RootCanvasComponentSetStyle),
                    rootCanvas == null,
                    () =>
                    {
                        if (rootCanvas == null)
                        {
                            rootCanvas = LoadOrCreateNew<RootCanvasComponentSetStyle>(
                                $"Master{nameof(RootCanvasComponentSetStyle)}",
                                ownerType: typeof(ApplicationManager)
                            );
                        }
                    }
                );

                initializer.Do(
                    this,
                    nameof(BackgroundComponentSetStyle),
                    rootBackground == null,
                    () =>
                    {
                        if (rootBackground == null)
                        {
                            rootBackground = LoadOrCreateNew<BackgroundComponentSetStyle>(
                                $"Master{nameof(BackgroundComponentSetStyle)}",
                                ownerType: typeof(ApplicationManager)
                            );
                        }
                    }
                );

                initializer.Do(
                    this,
                    nameof(ApplicationUIStyle),
                    uiStyle == null,
                    () =>
                    {
                        if (uiStyle == null)
                        {
                            uiStyle = LoadOrCreateNew<ApplicationUIStyle>(nameof(ApplicationUIStyle));
                        }
                    }
                );

                initializer.Do(
                    this,
                    nameof(DeviceButtonLookup),
                    deviceButtons == null,
                    () =>
                    {
                        if (deviceButtons == null)
                        {
                            deviceButtons = LoadOrCreateNew<DeviceButtonLookup>(nameof(DeviceButtonLookup));
                        }
                    }
                );

                /*using (_PRF_Initialize.Auto())
                {
                    if (!AppalachiaApplication.IsPlaying)
                    {
                        Scan();
                    }
                }*/
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

        /*
        [Button]
        private void Scan()
        {
#if UNITY_EDITOR
            using (_PRF_Scan.Auto())
            {
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
        }*/
    }
}
