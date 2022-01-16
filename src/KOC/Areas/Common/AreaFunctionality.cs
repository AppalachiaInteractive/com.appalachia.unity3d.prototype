using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.Common
{
    [CallStaticConstructorInEditor]
    public abstract class AreaFunctionality<TFunctionality, TFunctionalityMetadata, TAreaManager,
                                            TAreaMetadata> : SingletonAppalachiaBehaviour<TFunctionality>
        where TFunctionality : AreaFunctionality<TFunctionality, TFunctionalityMetadata, TAreaManager,
            TAreaMetadata>
        where TFunctionalityMetadata : AreaFunctionalityMetadata<TFunctionality, TFunctionalityMetadata,
            TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        static AreaFunctionality()
        {
            RegisterDependency<TFunctionalityMetadata>(i => metadata = i);
            AreaManager<TAreaManager, TAreaMetadata>.InstanceAvailable += i => areaManager = i;
        }

        #region Static Fields and Autoproperties

        protected static TAreaManager areaManager;

        [ShowInInspector, InlineEditor]
        protected static TFunctionalityMetadata metadata;

        #endregion

        protected abstract void ApplyMetadataInternal();

        [ButtonGroup("Subscriptions")]
        [LabelText("Subscribe")]
        protected abstract void SubscribeToAllFunctionalties();

        [ButtonGroup("Subscriptions")]
        [LabelText("Unsubscribe")]
        protected abstract void UnsubscribeFromAllFunctionalities();

        protected override void AfterInitialization()
        {
            using (_PRF_AfterInitialization.Auto())
            {
                base.AfterInitialization();

                ApplyMetadata();
            }
        }

        protected override async AppaTask WhenDisabled()
        {
            await base.WhenDisabled();

            using (_PRF_WhenDisabled.Auto())
            {
                metadata.SettingsChanged -= ApplyMetadata;
                UnsubscribeFromAllFunctionalities();
            }
        }

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                ApplyMetadata();
            }
        }

        [ButtonGroup("Metadata")]
        public void ApplyMetadata()
        {
            using (_PRF_ApplyMetadata.Auto())
            {
                metadata.SettingsChanged -= ApplyMetadata;
                metadata.SettingsChanged += ApplyMetadata;
                metadata.Apply(this as TFunctionality);
                ApplyMetadataInternal();
                UpdateFunctionalitySubscriptions();
            }
        }

        [ButtonGroup("Subscriptions")]
        [LabelText("Refresh")]
        private void UpdateFunctionalitySubscriptions()
        {
            using (_PRF_UpdateFunctionalitySubscriptions.Auto())
            {
                UnsubscribeFromAllFunctionalities();
                SubscribeToAllFunctionalties();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_AfterInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialization));

        private static readonly ProfilerMarker _PRF_ApplyMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyMetadata));

        protected static readonly ProfilerMarker _PRF_ApplyMetadataInternal =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyMetadataInternal));

        protected static readonly ProfilerMarker _PRF_SubscribeToAllFunctionalties =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeToAllFunctionalties));

        protected static readonly ProfilerMarker _PRF_UnsubscribeFromAllFunctionalities =
            new ProfilerMarker(_PRF_PFX + nameof(UnsubscribeFromAllFunctionalities));

        private static readonly ProfilerMarker _PRF_UpdateFunctionalitySubscriptions =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateFunctionalitySubscriptions));

        #endregion
    }
}
