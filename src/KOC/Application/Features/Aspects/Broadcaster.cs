using Appalachia.Core.Events;
using Appalachia.Core.Events.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Features.Aspects
{
    public static class Broadcaster
    {
        #region Nested type: Functionality

        public sealed class
            Functionality<TService, TServiceMetadata, TServiceArgs> : IService<TService, TServiceMetadata,
                TServiceArgs>
            where TService : IService<TService, TServiceMetadata, TServiceArgs>
            where TServiceMetadata : IServiceMetadata<TService, TServiceMetadata, TServiceArgs>
            where TServiceArgs : IArgs
        {
            #region Fields and Autoproperties

            public ValueEvent<TServiceArgs>.Data Broadcast;

            #endregion

            #region IService<TService,TServiceMetadata,TServiceArgs> Members

            [ReadOnly, ShowInInspector]
            public int Subscribers => Broadcast.SubscriberCount;

            public void OnBroadcast(TServiceArgs args)
            {
                using (_PRF_OnBroadcast.Auto())
                {
                    Broadcast.RaiseEvent(args);
                }
            }

            #endregion

            #region Profiling

            private const string _PRF_PFX =
                nameof(Functionality<TService, TServiceMetadata, TServiceArgs>) + ".";

            private static readonly ProfilerMarker _PRF_OnBroadcast =
                new ProfilerMarker(_PRF_PFX + nameof(OnBroadcast));

            #endregion
        }

        #endregion

        #region Nested type: IArgs

        public interface IArgs
        {
        }

        #endregion

        #region Nested type: IService

        public interface IService<TService, TServiceMetadata, TServiceArgs>
            where TService : IService<TService, TServiceMetadata, TServiceArgs>
            where TServiceMetadata : IServiceMetadata<TService, TServiceMetadata, TServiceArgs>
            where TServiceArgs : IArgs
        {
            int Subscribers { get; }

            void OnBroadcast(TServiceArgs args);
        }

        #endregion

        #region Nested type: IServiceMetadata

        public interface IServiceMetadata<TService, TServiceMetadata, TServiceArgs>
            where TService : IService<TService, TServiceMetadata, TServiceArgs>
            where TServiceMetadata : IServiceMetadata<TService, TServiceMetadata, TServiceArgs>
            where TServiceArgs : IArgs
        {
        }

        #endregion
    }
}
