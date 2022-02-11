using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Components.OnScreenButtons;
using Appalachia.Prototype.KOC.Components.Styling;
using Appalachia.Prototype.KOC.Scenes;
using Appalachia.UI.Controls.Sets.Background;
using Appalachia.UI.Controls.Sets.RootCanvas;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Styling;
using Appalachia.Utility.Async;
using UnityEngine.Audio;

namespace Appalachia.Prototype.KOC.Lifetime
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

        public CameraData clearCameraData;

        public RootCanvasComponentSetData rootCanvas;

        public BackgroundComponentSetData rootBackground;

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
