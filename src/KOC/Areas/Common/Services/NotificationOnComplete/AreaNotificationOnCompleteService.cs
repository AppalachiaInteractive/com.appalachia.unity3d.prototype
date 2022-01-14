using System;
using Appalachia.Utility.Async;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.Common.Services.NotificationOnComplete
{
    public abstract class AreaNotificationOnCompleteService<TService, TServiceMetadata, TNotificationDelegate,
                                                            TAreaManager, TAreaMetadata> : AreaService<
        TService, TServiceMetadata, TAreaManager, TAreaMetadata>
        where TService : AreaNotificationOnCompleteService<TService, TServiceMetadata, TNotificationDelegate,
            TAreaManager, TAreaMetadata>
        where TServiceMetadata : AreaServiceMetadata<TService, TServiceMetadata, TAreaManager, TAreaMetadata>
        where TNotificationDelegate : Delegate
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
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
