using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Core;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton;
using Appalachia.UI.Controls.Sets2.Buttons.SelectableButton;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Contracts
{
    public interface IActivityBarSubwidgetMetadata :
        IAreaSingletonSubwidgetMetadata<IActivityBarSubwidget, IActivityBarSubwidgetMetadata>,
        IEnabledSubwidgetMetadata,
        IPrioritySubwidgetMetadata
    {
        ActivityBarSection Section { get; }

        ISelectableButtonComponentSetData Button { get; }
    }
}
