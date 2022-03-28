using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Components.OnScreenButtons;
using Appalachia.Prototype.KOC.Components.Styling;
using Appalachia.Prototype.KOC.Scenes;
using Appalachia.UI.Functionality.Canvas.Controls.Root;
using Appalachia.UI.Functionality.Images.Controls.Background;
using Appalachia.UI.Functionality.Rendering.Cameras.Components;
using Appalachia.UI.Styling;
using Appalachia.Utility.Async;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Application.Lifetime
{
    [Serializable]
    public sealed class LifetimeMetadata : SingletonAppalachiaObject<LifetimeMetadata>
    {
        static LifetimeMetadata()
        {
            RegisterDependency<MainAreaSceneInformationCollection>(i => mainAreaSceneInformationCollection = i);
        }

        #region Static Fields and Autoproperties

        public static MainAreaSceneInformationCollection mainAreaSceneInformationCollection;

        #endregion

        #region Fields and Autoproperties

        [SerializeField] public CameraConfig clearCameraConfig;

        [SerializeField] public RootCanvasControlConfig rootCanvas;

        [SerializeField] public BackgroundControlConfig rootBackground;

        [SerializeField] public ApplicationUIStyle uiStyle;
        [SerializeField] public DeviceButtonLookup deviceButtons;
        [SerializeField] public AudioMixer audioMixer;
        [SerializeField] public StyleElementDefaultLookup styleLookup;

        [SerializeField] public InputActionAsset inputAsset;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                CameraConfig.Refresh(ref clearCameraConfig, this);
                RootCanvasControlConfig.Refresh(ref rootCanvas, this);
                BackgroundControlConfig.Refresh(ref rootBackground, this);
                
#if UNITY_EDITOR
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
#endif
            }
        }
    }
}
