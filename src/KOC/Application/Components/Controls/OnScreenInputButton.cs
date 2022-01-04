using Appalachia.Core.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Behaviours;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Extensions;
using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons;
using Appalachia.Prototype.KOC.Application.Styling.OnScreenButtons;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
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
    [CallStaticConstructorInEditor]
    public class OnScreenInputButton : AppalachiaApplicationBehaviour<OnScreenInputButton>
    {
        static OnScreenInputButton()
        {
            RegisterDependency<DeviceButtonLookup>(i => _deviceButtonLookup = i);
        }

        #region Static Fields and Autoproperties

        private static DeviceButtonLookup _deviceButtonLookup;

        #endregion

        #region Fields and Autoproperties

        [OnValueChanged(nameof(InitializeSynchronous))]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        public OnScreenButtonMetadata metadata;

        [SmartLabel] public TextMeshProUGUI text;

        [FormerlySerializedAs("button")]
        public Image image;

        #endregion

        [Button]
        public static void Populate()
        {
#if UNITY_EDITOR
            using (_PRF_Populate.Auto())
            {
                var actions = LifetimeComponentManager.GetActions();

                foreach (var action in actions)
                {
                    var targetName = action.ToFormattedName();

                    var metadata = OnScreenButtonMetadata.LoadOrCreateNew(targetName);

                    if (metadata.actionReference == null)
                    {
                        metadata.actionReference = action.GetReference();

                        metadata.MarkAsModified();

                        if (metadata.actionReference == null)
                        {
                            StaticContext.Log.Error(
                                ZString.Format(
                                    "Missing [{0}] for [{1}] {2}.",
                                    nameof(InputActionReference),
                                    nameof(OnScreenButtonMetadata),
                                    targetName
                                )
                            );
                        }
                    }
                    else if (metadata.style == null)
                    {
                        metadata.MarkAsModified();

                        if (metadata.style == null)
                        {
                            StaticContext.Log.Error(
                                ZString.Format(
                                    "Missing [{0}] for [{1}] {2}.",
                                    nameof(OnScreenButtonStyleOverride),
                                    nameof(OnScreenButtonMetadata),
                                    targetName
                                )
                            );
                        }
                    }
                }
            }
#endif
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                var action = metadata.actionReference.action;

                var targetName = action.ToFormattedName();
                var baseName = ZString.Format("{0} - {1}", nameof(OnScreenInputButton), targetName);

                var uiStyle = LifetimeComponents.LifetimeMetadata.uiStyle;
                var fontStyle = uiStyle.onScreenButtonStyle.Font;

                gameObject.name = baseName;

                gameObject.GetOrCreateComponentInChild(ref text, targetName, false);
                text.name = ZString.Format("Text - {0}", baseName);
                text.rectTransform.Reset(RectResetOptions.All);

                gameObject.GetOrCreateComponentInChild(ref image, targetName, false);
                image.name = ZString.Format("Image - {0}", baseName);
                image.rectTransform.Reset(RectResetOptions.All);

                fontStyle.ToApplicable.Apply(text);

                metadata.ApplyImage(_deviceButtonLookup, image);
                metadata.ApplyText(_deviceButtonLookup, text);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(OnScreenInputButton) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_Start = new ProfilerMarker(_PRF_PFX + nameof(Start));

        private static readonly ProfilerMarker
            _PRF_Populate = new ProfilerMarker(_PRF_PFX + nameof(Populate));

        #endregion
    }
}
