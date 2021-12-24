using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Behaviours;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Extensions;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using TMPro;
using Unity.Profiling;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Input.OnScreenButtons
{
    [SmartLabelChildren, SmartLabel]
    public sealed class OnScreenButton : AppalachiaApplicationBehaviour<OnScreenButton>
    {
        #region Fields and Autoproperties

        public OnScreenButtonMetadata metadata;

        private TextMeshProUGUI text;
        private Image image;

        #endregion

        #region Event Functions

        #endregion

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                var baseName = metadata.actionReference.ToFormattedName();

                var buttonName = ZString.Format("{0} - {1}", nameof(OnScreenButton), baseName);

                gameObject.name = buttonName;

                gameObject.GetOrCreateComponentInChild(ref text,  ZString.Format("Text - {0}",  baseName));
                gameObject.GetOrCreateComponentInChild(ref image, ZString.Format("Image - {0}", baseName));

                await initializer.Do(
                    this,
                    nameof(text),
                    Initializer.TagState.NonSerialized,
                    () => { text.rectTransform.Reset(RectResetOptions.All); }
                );

                await initializer.Do(
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
