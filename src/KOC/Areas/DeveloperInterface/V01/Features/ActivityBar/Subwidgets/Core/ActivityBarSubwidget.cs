using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets;
using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.Functionality.Buttons.Components;
using Appalachia.UI.Functionality.Buttons.Controls.Default;
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

        public AppaButtonControl button;

        #endregion

        protected override AppaButton GetTooltipTarget()
        {
            using (_PRF_GetTooltipTarget.Auto())
            {
                return button.button.AppaButton;
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

        public void UpdateSubwidgetIconSize(RectTransformConfig rectTransformData)
        {
            using (_PRF_UpdateSubwidgetIconSize.Auto())
            {
                var buttonData = Metadata.button;
                var optionalGroupConfig = buttonData.ButtonIcon;
                var groupConfig = optionalGroupConfig.Value;

                groupConfig.UpdateRectTransformConfig(rectTransformData);
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_UpdateSubwidgetIconSize =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSubwidgetIconSize));

        #endregion
    }
}
