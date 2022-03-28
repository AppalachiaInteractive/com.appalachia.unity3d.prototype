using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Subwidget;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Subwidget.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core
{
    [CallStaticConstructorInEditor]
    [RequireComponent(typeof(RectTransform))]
    public abstract partial class StatusBarSubwidget<TSubwidget, TSubwidgetMetadata> :
        DeveloperInterfaceManager_V01.SingletonSubwidget<TSubwidget, TSubwidgetMetadata, IStatusBarSubwidget,
            IStatusBarSubwidgetMetadata, StatusBarWidget, StatusBarWidgetMetadata, StatusBarFeature,
            StatusBarFeatureMetadata>,
        IStatusBarSubwidget
        where TSubwidget : StatusBarSubwidget<TSubwidget, TSubwidgetMetadata>
        where TSubwidgetMetadata : StatusBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadata>
    {
        #region Fields and Autoproperties

        public StatusBarSubwidgetControl button;

        #endregion
        public abstract string GetStatusBarText();

        public virtual Color GetStatusBarColor()
        {
            return Color.white;
        }

        public void LayoutStatusBarText()
        {
            using (_PRF_LayoutStatusBarText.Auto())
            {
                var textSubset = button.buttonText;
                var text = textSubset.TextMeshProUGUI;

                if (text != null)
                {
                    text.autoSizeTextContainer = true;

                    if (text.font != null)
                    {
                        var sizeDelta = text.rectTransform.sizeDelta;
                        sizeDelta.x = text.preferredWidth;
                        sizeDelta.y = text.preferredHeight;

                        text.rectTransform.sizeDelta = sizeDelta;
                    }

                    LayoutRebuilder.MarkLayoutForRebuild(text.rectTransform);
                }
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                StatusBarSubwidgetControl.Refresh(ref button, gameObject, nameof(button));
            }
        }

        public IStatusBarSubwidgetControl Control => button;

        #region Profiling

        protected static readonly ProfilerMarker _PRF_GetStatusBarText =
            new ProfilerMarker(_PRF_PFX + nameof(GetStatusBarText));

        private static readonly ProfilerMarker _PRF_LayoutStatusBarText =
            new ProfilerMarker(_PRF_PFX + nameof(LayoutStatusBarText));

        #endregion
    }
}
