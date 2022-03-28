using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Core;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton.Contracts;
using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.Functionality.Buttons.Controls.Default.Contracts;
using Appalachia.UI.Functionality.Tooltips.Contracts;
using Appalachia.UI.Functionality.Tooltips.Styling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Contracts
{
    public interface IActivityBarSubwidgetMetadata :
        IAreaSingletonSubwidgetMetadata<IActivityBarSubwidget, IActivityBarSubwidgetMetadata>,
        IEnabledSubwidgetMetadata
    {
        ActivityBarSection Section { get; }

        IAppaButtonControlConfig Button { get; }

        void UpdateTooltipStyle(TooltipStyleTypes tooltipStyle);
        void UpdateSubwidgetIconSize(RectTransformConfig rectTransformData);
    }
}
