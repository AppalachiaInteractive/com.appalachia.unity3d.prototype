using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton.Contracts;
using Appalachia.UI.Controls.Sets.Buttons.Button;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Styling.Fonts;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts
{
    public interface IStatusBarSubwidgetMetadata :
        IAreaSingletonSubwidgetMetadata<IStatusBarSubwidget, IStatusBarSubwidgetMetadata>,
        IEnabledSubwidgetMetadata
    {
        IButtonComponentSetData Button { get; }

        StatusBarSection Section { get; }

        void UpdateSubwidgetFont(FontStyleOverride fontStyleOverride);
        void UpdateSubwidgetIconSize(RectTransformData rectTransformData);
    }
}
