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

namespace Appalachia.Prototype.KOC.Application.Lifetime
{
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

        public CameraConfig clearCameraConfig;

        [SerializeField] public RootCanvasControlConfig rootCanvas;

        [SerializeField] public BackgroundControlConfig rootBackground;

        public ApplicationUIStyle uiStyle;
        public DeviceButtonLookup deviceButtons;
        public AudioMixer audioMixer;
        public StyleElementDefaultLookup styleLookup;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

#if UNITY_EDITOR
            using (_PRF_Initialize.Auto())
            {
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
            }
#endif
        }
    }
}
