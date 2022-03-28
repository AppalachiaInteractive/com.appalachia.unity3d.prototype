using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Subwidget.Contracts;
using Appalachia.UI.Functionality.Buttons.Controls.Default.Base;
using Appalachia.UI.Functionality.Tooltips.Styling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Subwidget
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class StatusBarSubwidgetControlConfig :
        BaseAppaButtonControlConfig<StatusBarSubwidgetControl, StatusBarSubwidgetControlConfig>,
        IStatusBarSubwidgetControlConfig
    {
        protected override void BeforeApplying(StatusBarSubwidgetControl control)
        {
            using (_PRF_BeforeApplying.Auto())
            {
                base.BeforeApplying(control);

                tooltip.tooltipStyle = TooltipStyleTypes.StatusBar;
            }
        }
    }
}
