using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application.Behaviours;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Extensions;
using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons;
using Appalachia.Prototype.KOC.Application.Styling.OnScreenButtons;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Components.Controls
{
    [ExecuteAlways, SmartLabelChildren]
    public class OnScreenInputButton : AppalachiaApplicationBehaviour
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(Initialize))]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        public OnScreenButtonMetadata metadata;

        [SmartLabel]
        public TextMeshProUGUI text;

        [FormerlySerializedAs("button")]
        public Image image;

        #endregion

        #region Event Functions

        protected override void Start()
        {
            using (_PRF_Start.Auto())
            {
                base.Start();

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

#if UNITY_EDITOR
        [Button]
        public static void Populate()
        {
            using (_PRF_Populate.Auto())
            {
                var actions = InputActions;

                foreach (var action in actions)
                {
                    var targetName = action.ToFormattedName();

                    var metadata = AppalachiaObject.LoadOrCreateNew<OnScreenButtonMetadata>(targetName);

                    if (metadata.actionReference == null)
                    {
                        metadata.actionReference = action.GetReference();

                        metadata.MarkAsModified();

                        if (metadata.actionReference == null)
                        {
                            AppaLog.Error(
                                $"Missing [{nameof(InputActionReference)}] for [{nameof(OnScreenButtonMetadata)}] {targetName}."
                            );
                        }
                    }
                    else if (metadata.style == null)
                    {
                        metadata.InitializeExternal();

                        metadata.MarkAsModified();

                        if (metadata.style == null)
                        {
                            AppaLog.Error(
                                $"Missing [{nameof(OnScreenButtonStyleOverride)}] for [{nameof(OnScreenButtonMetadata)}] {targetName}."
                            );
                        }
                    }
                }
            }
        }
#endif

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                if (metadata == null)
                {
                    return;
                }

                var action = metadata.actionReference.action;

                var targetName = action.ToFormattedName();
                var baseName = $"{nameof(OnScreenInputButton)} - {targetName}";

                var uiStyle = LifetimeComponents.metadata.uiStyle;
                var fontStyle = uiStyle.onScreenButtonStyle.Font;

                gameObject.name = baseName;

                gameObject.CreateOrGetComponentInChild(ref text, targetName, false);
                text.name = $"Text - {baseName}";
                text.rectTransform.Reset(RectResetOptions.All);

                gameObject.CreateOrGetComponentInChild(ref image, targetName, false);
                image.name = $"Image - {baseName}";
                image.rectTransform.Reset(RectResetOptions.All);

                fontStyle.ToApplicable.Apply(text);

                var buttonLookup = DeviceButtonLookup.instance;
                metadata.ApplyImage(buttonLookup, image);
                metadata.ApplyText(buttonLookup, text);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(OnScreenInputButton) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker
            _PRF_Populate = new ProfilerMarker(_PRF_PFX + nameof(Populate));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_Start = new ProfilerMarker(_PRF_PFX + nameof(Start));

        #endregion
    }
}
