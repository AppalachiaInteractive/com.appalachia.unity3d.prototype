using System;

namespace Appalachia.Prototype.KOC.Application.Features.Aspects
{
    public static class Notifier
    {
        #region Nested type: IMetadata

        public interface IMetadata<TNotificationDelegate>
            where TNotificationDelegate : Delegate
        {
        }

        #endregion

        #region Nested type: IService

        public interface IService<in TNotificationDelegate>
            where TNotificationDelegate : Delegate
        {
            void InitiateServiceTask(TNotificationDelegate notificationDelegate);
        }

        #endregion
    }
}
