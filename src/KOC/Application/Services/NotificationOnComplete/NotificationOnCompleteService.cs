using System;
using Appalachia.Utility.Async;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Services.NotificationOnComplete
{
    public abstract class
        NotificationOnCompleteService<TService, TServiceMetadata, TNotificationDelegate> : ApplicationService<
            TService, TServiceMetadata>
        where TService : NotificationOnCompleteService<TService, TServiceMetadata, TNotificationDelegate>
        where TServiceMetadata : ApplicationServiceMetadata<TService, TServiceMetadata>
        where TNotificationDelegate : Delegate
    {
        public void InitiateServiceTask(TNotificationDelegate notificationDelegate)
        {
            ExecuteServiceTask(notificationDelegate).Forget();
        }

        protected abstract AppaTask ExecuteServiceTask(TNotificationDelegate notificationDelegate);

        #region Profiling

        protected static readonly ProfilerMarker _PRF_ExecuteServiceTask =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteServiceTask));

        #endregion
    }
}
