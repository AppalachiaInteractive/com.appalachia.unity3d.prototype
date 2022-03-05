using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Styling.Fonts;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts
{
    
    public interface IStatusBarSubwidget : IAreaSingletonSubwidget<IStatusBarSubwidget, IStatusBarSubwidgetMetadata>,
                                           IDevTooltipSubwidgetController
    {
        void OnClicked();
        void UpdateSubwidgetFont(FontStyleOverride fontStyleOverride);
        void UpdateSubwidgetIconSize(RectTransformData rectTransformData);
    }
}
