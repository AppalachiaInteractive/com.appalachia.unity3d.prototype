using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Behaviours;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Extensions;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using TMPro;
using Unity.Profiling;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Input.OnScreenButtons
{
    [SmartLabelChildren, SmartLabel]
    public class OnScreenButton : AppalachiaApplicationBehaviour
    {
        #region Fields and Autoproperties

        public OnScreenButtonMetadata metadata;

        private TextMeshProUGUI text;
        private Image image;

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

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                var baseName = metadata.actionReference.ToFormattedName();

                var buttonName = $"{nameof(OnScreenButton)} - {baseName}";

                gameObject.name = buttonName;

                gameObject.CreateOrGetComponentInChild(ref text,  $"Text - {baseName}");
                gameObject.CreateOrGetComponentInChild(ref image, $"Image - {baseName}");

                initializationData.Initialize(
                    this,
                    nameof(text),
                    Initializer.TagState.NonSerialized,
                    () => { text.rectTransform.Reset(RectResetOptions.All); }
                );

                initializationData.Initialize(
                    this,
                    nameof(image),
                    Initializer.TagState.NonSerialized,
                    () => { image.rectTransform.Reset(RectResetOptions.All); }
                );
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(OnScreenButton) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_Start = new ProfilerMarker(_PRF_PFX + nameof(Start));

        #endregion
    }
}
