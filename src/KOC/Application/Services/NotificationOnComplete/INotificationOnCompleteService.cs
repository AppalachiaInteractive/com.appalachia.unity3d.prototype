namespace Appalachia.Prototype.KOC.Application.Services.NotificationOnComplete
{
    public interface INotificationOnCompleteService<in TNotificationDelegate> : IApplicationService
    {
        void InitiateServiceTask(TNotificationDelegate notificationDelegate);
    }
}
