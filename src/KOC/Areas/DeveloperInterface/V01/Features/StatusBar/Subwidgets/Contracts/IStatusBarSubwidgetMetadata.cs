using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton;
using Appalachia.UI.Controls.Sets.Buttons.Button;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts
{
    public interface IStatusBarSubwidgetMetadata : IAreaSingletonSubwidgetMetadata<IStatusBarSubwidget, IStatusBarSubwidgetMetadata>
    {
        bool Enabled { get; }

        IButtonComponentSetData Button { get; }

        int Priority { get; }

        StatusBarSection Section { get; }
    }
}
