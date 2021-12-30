using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Extensions;
using Appalachia.Prototype.KOC.Application.Styling.OnScreenButtons;
using Appalachia.Utility.Async;
using TMPro;
using Unity.Profiling;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Input.OnScreenButtons
{
    [SmartLabelChildren, SmartLabel]
    public sealed class OnScreenButtonMetadata : AppalachiaObject<OnScreenButtonMetadata>
    {
        #region Fields and Autoproperties

        public InputActionReference actionReference;

        public OnScreenButtonStyleOverride style;

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

                style.ToApplicable.Apply(actionReference, match, text);
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
#if UNITY_EDITOR
            using (_PRF_Initialize.Auto())
            {
                await initializer.Do(
                    this,
                    nameof(OnScreenButtonStyleOverride),
                    style == null,
                    () =>
                    {
                        if ((actionReference == null) || (style != null))
                        {
                            return;
                        }

                        var targetName = actionReference.ToFormattedName();

                        style = OnScreenButtonStyleOverride.LoadOrCreateNew(targetName);
                    }
                );

                if (actionReference != null)
                {
                    var targetName = actionReference.ToFormattedName();

                    if (style.name != targetName)
                    {
                        style.Rename(targetName);
                        MarkAsModified();
                    }
                }
            }
#endif
        }

        #region Menu Items

#if UNITY_EDITOR
        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + nameof(OnScreenButtonMetadata),
            priority = PKG.Menu.Assets.Priority
        )]
        public static void CreateAsset()
        {
            CreateNew<OnScreenButtonMetadata>();
        }
#endif

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(OnScreenButtonMetadata) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_ApplyText =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyText));

        private static readonly ProfilerMarker _PRF_ApplyImage =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyImage));

        #endregion
    }
}
