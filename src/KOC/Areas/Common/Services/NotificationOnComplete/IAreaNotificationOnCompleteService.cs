namespace Appalachia.Prototype.KOC.Areas.Common.Services.NotificationOnComplete
{
    public interface IAreaNotificationOnCompleteService<in TNotificationDelegate> : IAreaService
    {
        void InitiateServiceTask(TNotificationDelegate notificationDelegate);
    }
}
