using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton.Contracts;
using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.Functionality.Buttons.Controls.Default.Contracts;
using Appalachia.UI.Functionality.Tooltips.Contracts;
using Appalachia.UI.Functionality.Tooltips.Styling;
using Appalachia.UI.Styling.Fonts;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts
{
    public interface IStatusBarSubwidgetMetadata :
        IAreaSingletonSubwidgetMetadata<IStatusBarSubwidget, IStatusBarSubwidgetMetadata>,
        IEnabledSubwidgetMetadata
    {
        IAppaButtonControlConfig Button { get; }
        StatusBarSection Section { get; }
        void UpdateFontStyle(FontStyleTypes style);
        void UpdateTooltipStyle(TooltipStyleTypes style);
        void UpdateSubwidgetIconSize(RectTransformConfig rectTransformData);
    }
}
