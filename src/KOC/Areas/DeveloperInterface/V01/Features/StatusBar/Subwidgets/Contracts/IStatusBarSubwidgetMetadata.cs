using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton;
using Appalachia.UI.Controls.Sets2.Buttons.Button;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts
{
    public interface IStatusBarSubwidgetMetadata : IAreaSingletonSubwidgetMetadata<IStatusBarSubwidget, IStatusBarSubwidgetMetadata>, IPrioritySubwidgetMetadata, IEnabledSubwidgetMetadata
    {

        IButtonComponentSetData Button { get; }

        StatusBarSection Section { get; }
    }
}
