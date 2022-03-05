using Appalachia.Utility.Events;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Controlled
{
    public interface IApplicationControlledSubwidget
    {
        void RequestUpdate();
        void SubscribeToUpdateRequests(AppaEvent.Handler onUpdateRequested);
        void UpdateSubwidget();
    }
}
