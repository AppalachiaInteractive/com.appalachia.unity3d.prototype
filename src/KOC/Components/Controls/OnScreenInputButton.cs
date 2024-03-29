using Appalachia.Core.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Behaviours;
using Appalachia.Prototype.KOC.Components.OnScreenButtons;
using Appalachia.Prototype.KOC.Components.Styling.OnScreenButtons;
using Appalachia.Prototype.KOC.Extensions;
using Appalachia.UI.Core.Extensions;
using Appalachia.UI.Styling.Fonts;
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

namespace Appalachia.Prototype.KOC.Components.Controls
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
                }
            }
#endif
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                var action = metadata.actionReference.action;

                var targetName = action.ToFormattedName();
                var baseName = ZString.Format("{0} - {1}", nameof(OnScreenInputButton), targetName);

                var uiStyle = LifetimeComponentManager.lifetimeMetadata.uiStyle;
                var fontStyleType = uiStyle.onScreenButtonStyle.Font;
                var fontStyle = StyleLookup.GetFont(fontStyleType);

                gameObject.name = baseName;

                gameObject.GetOrAddComponentInChild(ref text, targetName, false);
                text.name = ZString.Format("Text - {0}", baseName);
                text.rectTransform.Reset(RectResetOptions.All);

                gameObject.GetOrAddComponentInChild(ref image, targetName, false);
                image.name = ZString.Format("Image - {0}", baseName);
                image.rectTransform.Reset(RectResetOptions.All);

                fontStyle.ToApplicable.Apply(text);

                metadata.ApplyImage(_deviceButtonLookup, image);
                metadata.ApplyText(_deviceButtonLookup, text);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_Populate = new ProfilerMarker(_PRF_PFX + nameof(Populate));

        #endregion
    }
}
