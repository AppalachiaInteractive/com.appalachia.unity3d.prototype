using Appalachia.Core.Objects.Delegates;
using Appalachia.Core.Objects.Delegates.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Services.Broadcast
{
    public abstract class
        BroadcastService<TService, TServiceMetadata, TServiceArgs> : ApplicationService<TService,
            TServiceMetadata>
        where TService : BroadcastService<TService, TServiceMetadata, TServiceArgs>
        where TServiceMetadata : BroadcastServiceMetadata<TService, TServiceMetadata, TServiceArgs>
        where TServiceArgs : BroadcastArgs
    {
        public event ValueArgs<TServiceArgs>.Handler Broadcast;

        [ReadOnly, ShowInInspector]
        public int Subscribers => Broadcast?.GetInvocationList().Length ?? 0;

        protected virtual void OnBroadcast(TServiceArgs args)
        {
            using (_PRF_OnBroadcast.Auto())
            {
                Broadcast.RaiseEvent(args);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnBroadcast =
            new ProfilerMarker(_PRF_PFX + nameof(OnBroadcast));

        #endregion
    }
}
