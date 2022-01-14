using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Behaviours;
using Appalachia.Prototype.KOC.Extensions;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using TMPro;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Components.OnScreenButtons
{
    [SmartLabelChildren, SmartLabel]
    public sealed class OnScreenButton : AppalachiaApplicationBehaviour<OnScreenButton>
    {
        #region Fields and Autoproperties

        public OnScreenButtonMetadata metadata;

        private TextMeshProUGUI text;
        private Image image;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);
            using (_PRF_Initialize.Auto())
            {
                var baseName = metadata.actionReference.ToFormattedName();

                var buttonName = ZString.Format("{0} - {1}", nameof(OnScreenButton), baseName);

                gameObject.name = buttonName;

                gameObject.GetOrCreateComponentInChild(ref text,  ZString.Format("Text - {0}",  baseName));
                gameObject.GetOrCreateComponentInChild(ref image, ZString.Format("Image - {0}", baseName));
            }

            initializer.Do(
                this,
                nameof(text),
                Initializer.TagState.NonSerialized,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        text.rectTransform.Reset(RectResetOptions.All);
                    }
                }
            );

            initializer.Do(
                this,
                nameof(image),
                Initializer.TagState.NonSerialized,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        image.rectTransform.Reset(RectResetOptions.All);
                    }
                }
            );
        }
    }
}
