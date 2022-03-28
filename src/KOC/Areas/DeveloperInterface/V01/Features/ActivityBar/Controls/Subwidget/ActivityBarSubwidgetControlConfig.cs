using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Controls.Subwidget.Contracts;
using Appalachia.UI.Functionality.Buttons.Controls.Default.Base;
using Appalachia.UI.Functionality.Tooltips.Styling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Controls.Subwidget
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class ActivityBarSubwidgetControlConfig :
        BaseAppaButtonControlConfig<ActivityBarSubwidgetControl, ActivityBarSubwidgetControlConfig>,
        IActivityBarSubwidgetControlConfig
    {
        protected override void BeforeApplying(ActivityBarSubwidgetControl control)
        {
            using (_PRF_BeforeApplying.Auto())
            {
                base.BeforeApplying(control);

                tooltip.tooltipStyle = TooltipStyleTypes.ActivityBar;
            }
        }
    }
}
