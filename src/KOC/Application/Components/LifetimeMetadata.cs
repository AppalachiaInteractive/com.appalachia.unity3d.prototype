using System.Collections.Generic;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Core;
using Appalachia.Core.Attributes;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons;
using Appalachia.Prototype.KOC.Application.Styling;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Application.Components
{
    public class LifetimeMetadata : SingletonAppalachiaObject<LifetimeMetadata>
    {
        #region Fields and Autoproperties

        public SingletonAppalachiaObjectLookup singletonLookup;
        public ApplicationUIStyle uiStyle;
        public DeviceButtonLookup deviceButtons;
        public InputActionAsset inputActionAsset;
        public AudioMixer audioMixer;

        public List<ScriptableObject> criticalReferences;

        public SignalAsset splashScreenFinishedAsset;

        #endregion

        #region Event Functions

        protected override void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                base.Awake();
                Initialize();
            }
        }

        protected override void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                base.OnEnable();

                Initialize();
            }
        }

        #endregion

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                initializer.Initialize(
                    this, nameof(inputActionAsset), inputActionAsset == null,
                    () =>
                    {
                        inputActionAsset =
                            AssetDatabaseManager.FindFirstAssetMatch<InputActionAsset>("KOCInputActions");
                    });
                initializer.Initialize(
                    this,
                    nameof(SingletonAppalachiaObjectLookup),
                    singletonLookup == null,
                    () =>
                    {
                        if (singletonLookup == null)
                        {
                            singletonLookup =
                                LoadOrCreateNew<SingletonAppalachiaObjectLookup>(
                                    nameof(SingletonAppalachiaObjectLookup)
                                );
                        }
                    }
                );

                singletonLookup.InitializeExternal();

                initializer.Initialize(
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

                uiStyle.InitializeExternal();

                initializer.Initialize(
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

                deviceButtons.InitializeExternal();

                if (!UnityEngine.Application.isPlaying)
                {
                    singletonLookup.Scan();
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

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_Scan = new(_PRF_PFX + nameof(Scan));

        #endregion
    }
}
