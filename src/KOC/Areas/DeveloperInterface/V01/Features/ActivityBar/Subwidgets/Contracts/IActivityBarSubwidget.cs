using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton;
using Appalachia.UI.Core.Components.Data;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Contracts
{
    public interface IActivityBarSubwidget :
        IAreaSingletonSubwidget<IActivityBarSubwidget, IActivityBarSubwidgetMetadata>,
        IDevTooltipSubwidgetController,
        IActivable
    {
        void UpdateSubwidgetIconSize(RectTransformData rectTransformData);
    }
}
