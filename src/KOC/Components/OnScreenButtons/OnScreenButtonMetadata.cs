using Appalachia.Core.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Components.Styling.OnScreenButtons;
using Appalachia.Prototype.KOC.Extensions;
using Appalachia.UI.Core.Styling;
using Appalachia.Utility.Async;
using TMPro;
using Unity.Profiling;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Components.OnScreenButtons
{
    [SmartLabelChildren, SmartLabel]
    [CallStaticConstructorInEditor]
    public sealed class OnScreenButtonMetadata : AppalachiaObject<OnScreenButtonMetadata>
    {
        static OnScreenButtonMetadata()
        {
            RegisterDependency<StyleElementDefaultLookup>(i => _styleElementDefaultLookup = i);
        }

        #region Static Fields and Autoproperties

        private static StyleElementDefaultLookup _styleElementDefaultLookup;

        #endregion

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
            await base.Initialize(initializer);

#if UNITY_EDITOR

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
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

        private static readonly ProfilerMarker _PRF_ApplyImage =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyImage));

        private static readonly ProfilerMarker _PRF_ApplyText =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyText));

        #endregion
    }
}
