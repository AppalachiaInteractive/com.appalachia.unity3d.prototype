using Unity.Profiling;
using UnityEngine.Events;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Extensions
{
    public static class SignalReceiverExtensions
    {
#if UNITY_EDITOR
        /// <summary>
        ///     Connects a method to a signal in an easy way.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="asset">The SignalAsset scriptable that is being emitted from the timeline.</param>
        /// <param name="action">An action to run when the signal is fired.</param>
        public static void ConnectMethodToSignal(
            this SignalReceiver receiver,
            SignalAsset asset,
            UnityAction action)
        {
            using (_PRF_ConnectMethodToSignal.Auto())
            {
                UnityEvent signalEvent;

                if (receiver.DoesReactionAlreadyExist(asset, action))
                {
                    return;
                }

                signalEvent = new UnityEvent();
                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(signalEvent, action);
                receiver.AddReaction(asset, signalEvent);
            }
        }

        public static bool DoesReactionAlreadyExist(
            this SignalReceiver receiver,
            SignalAsset asset,
            UnityAction action)
        {
            using (_PRF_DoesReactionAlreadyExist.Auto())
            {
                var receiverCount = receiver.Count();

                for (var receiverIndex = 0; receiverIndex < receiverCount; receiverIndex++)
                {
                    var assetAtIndex = receiver.GetSignalAssetAtIndex(receiverIndex);
                    var eventAtIndex = receiver.GetReactionAtIndex(receiverIndex);

                    if (assetAtIndex != asset)
                    {
                        continue;
                    }

                    var testEvent = new UnityEvent();
                    UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(testEvent, action);

                    var testEventCount = testEvent.GetPersistentEventCount();
                    var eventCountAtIndex = eventAtIndex.GetPersistentEventCount();

                    if (testEventCount != eventCountAtIndex)
                    {
                        continue;
                    }

                    for (var listenerIndex = 0; listenerIndex < testEventCount; listenerIndex++)
                    {
                        var testTarget = testEvent.GetPersistentTarget(listenerIndex);
                        var targetAtIndex = eventAtIndex.GetPersistentTarget(listenerIndex);

                        if (testTarget != targetAtIndex)
                        {
                            continue;
                        }

                        var testMethod = testEvent.GetPersistentMethodName(listenerIndex);
                        var methodAtIndex = eventAtIndex.GetPersistentMethodName(listenerIndex);

                        if (testMethod != methodAtIndex)
                        {
                            continue;
                        }

                        // we return from the method because we are already set up correctly.
                        return true;
                    }
                }

                return false;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(SignalReceiverExtensions) + ".";

        private static readonly ProfilerMarker _PRF_DoesReactionAlreadyExist =
            new ProfilerMarker(_PRF_PFX + nameof(DoesReactionAlreadyExist));

        private static readonly ProfilerMarker _PRF_ConnectMethodToSignal =
            new ProfilerMarker(_PRF_PFX + nameof(ConnectMethodToSignal));

        #endregion

#endif
    }
}
