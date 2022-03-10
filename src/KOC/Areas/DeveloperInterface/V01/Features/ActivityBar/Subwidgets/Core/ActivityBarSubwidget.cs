using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets;
using Appalachia.UI.Controls.Components.Buttons;
using Appalachia.UI.Controls.Sets.Buttons.SelectableButton;
using Appalachia.UI.Core.Components.Data;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Core
{
    [CallStaticConstructorInEditor]
    [RequireComponent(typeof(RectTransform))]
    public abstract partial class ActivityBarSubwidget<TSubwidget, TSubwidgetMetadata> :
        DeveloperInterfaceManager_V01.SingletonSubwidget<TSubwidget, TSubwidgetMetadata, IActivityBarSubwidget,
            IActivityBarSubwidgetMetadata, ActivityBarWidget, ActivityBarWidgetMetadata, ActivityBarFeature,
            ActivityBarFeatureMetadata>,
        IActivityBarSubwidget
        where TSubwidget : ActivityBarSubwidget<TSubwidget, TSubwidgetMetadata>
        where TSubwidgetMetadata : ActivityBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadata>
    {
        #region Fields and Autoproperties

        public SelectableButtonComponentSet button;

        #endregion

        protected override AppaButton GetTooltipTarget()
        {
            using (_PRF_GetTooltipTarget.Auto())
            {
                return button.AppaButton;
            }
        }

        #region IActivityBarSubwidget Members

        public override string GetDevTooltipText()
        {
            using (_PRF_GetDevTooltipText.Auto())
            {
                return Metadata.tooltipText;
            }
        }

        public void UpdateSubwidgetIconSize(RectTransformData rectTransformData)
        {
            using (_PRF_UpdateSubwidgetIconSize.Auto())
            {
                var buttonData = Metadata.button;
                var optionalSubsetData = buttonData.ButtonIcon;
                var subsetData = optionalSubsetData.Value;

                subsetData.UpdateRectTransformData(rectTransformData);
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_UpdateSubwidgetIconSize =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSubwidgetIconSize));

        #endregion
    }
}
