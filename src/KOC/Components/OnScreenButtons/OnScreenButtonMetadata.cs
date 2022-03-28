using Appalachia.Core.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Components.Styling.OnScreenButtons;
using Appalachia.UI.Styling;
using Appalachia.Utility.Async;
using TMPro;
using Unity.Profiling;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Components.OnScreenButtons
{
    [SmartLabelChildren, SmartLabel]
    [CallStaticConstructorInEditor]
    public sealed partial class OnScreenButtonMetadata : AppalachiaObject<OnScreenButtonMetadata>
    {
        static OnScreenButtonMetadata()
        {
            RegisterDependency<StyleElementDefaultLookup>(i => _styleLookup = i);
        }

        #region Static Fields and Autoproperties

        private static StyleElementDefaultLookup _styleLookup;

        #endregion

        #region Fields and Autoproperties

        public InputActionReference actionReference;

        [FormerlySerializedAs("style")]
        public OnScreenButtonStyleTypes styleType;

        #endregion

        public void ApplyImage(DeviceButtonLookup lookup, Image image)
        {
            using (_PRF_ApplyImage.Auto())
            {
                var match = lookup.Resolve(this);

                if (match == null)
                {
                    return;
                }

                var style = _styleLookup
                   .GetOverride<OnScreenButtonStyle, OnScreenButtonStyleOverride, IOnScreenButtonStyle,
                        OnScreenButtonStyleTypes>(styleType);

                style.ToApplicable.Apply(match, image);
            }
        }

        public void ApplyText(DeviceButtonLookup lookup, TextMeshProUGUI text)
        {
            using (_PRF_ApplyText.Auto())
            {
                var match = lookup.Resolve(this);

                if (match == null)
                {
                    return;
                }

                var style = _styleLookup
                   .GetOverride<OnScreenButtonStyle, OnScreenButtonStyleOverride, IOnScreenButtonStyle,
                        OnScreenButtonStyleTypes>(styleType);

                style.ToApplicable.Apply(actionReference, match, text, _styleLookup);
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

#if UNITY_EDITOR

            using (_PRF_Initialize.Auto())
            {
                styleType = OnScreenButtonStyleTypes.Default;
            }
#endif
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ApplyImage = new ProfilerMarker(_PRF_PFX + nameof(ApplyImage));

        private static readonly ProfilerMarker _PRF_ApplyText = new ProfilerMarker(_PRF_PFX + nameof(ApplyText));

        #endregion
    }
}
