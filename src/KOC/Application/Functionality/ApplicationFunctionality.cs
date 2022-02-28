using System;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Availability;
using Appalachia.Prototype.KOC.Application.Features.Availability.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Reflection.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Functionality
{
    public abstract partial class
        ApplicationFunctionality<TFunctionality, TFunctionalityMetadata, TManager> :
            SingletonAppalachiaBehaviour<TFunctionality>,
            IApplicationFunctionality
        where TFunctionality : ApplicationFunctionality<TFunctionality, TFunctionalityMetadata, TManager>
        where TFunctionalityMetadata :
        ApplicationFunctionalityMetadata<TFunctionality, TFunctionalityMetadata, TManager>
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>,
        IApplicationFunctionalityManager
    {
        static ApplicationFunctionality()
        {
            RegisterDependency<TFunctionalityMetadata>(md => { metadata = md; });

            /*
             * intentional use of base class ApplicationFunctionality<> to ensure that this callback
             * runs before other TFunctionality callbacks.
             */
            RegisterInstanceCallbacks
               .For<ApplicationFunctionality<TFunctionality, TFunctionalityMetadata, TManager>>()
               .When.Behaviour<TManager>()
               .IsAvailableThen(manager => { _manager = manager; });
        }

        #region Static Fields and Autoproperties

        [PropertyOrder(-10)]
        [ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        protected static TFunctionalityMetadata metadata;

        private static IFeatureAvailabilitySet _when;

        private static TManager _manager;

        #endregion

        #region Fields and Autoproperties

        private bool _applyingMetadata;

        #endregion

        protected new static IFeatureAvailabilitySet When
        {
            get
            {
                if (_when == null)
                {
                    _when = new FeatureAvailabilitySet(typeof(TFunctionality));
                }

                return _when;
            }
        }

        protected static TManager Manager => _manager;

        public TFunctionalityMetadata Metadata => metadata;

        public bool ApplyingMetadata
        {
            get => _applyingMetadata;
            internal set => _applyingMetadata = value;
        }

        protected internal void OnRequiresUpdate()
        {
            using (_PRF_OnApplyMetadata.Auto())
            {
                if (!FullyInitialized)
                {
                    return;
                }

                metadata.UpdateFunctionality(this as TFunctionality);
            }
        }

        protected virtual async AppaTask DelayEnabling()
        {
            await AppaTask.CompletedTask;
        }

        protected virtual void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
            }
        }

        protected new static void RegisterDependency<TDependency>(
            SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaBehaviour<TDependency>,
            IRepositoryDependencyTracker<TDependency>
        {
            using (_PRF_RegisterDependency.Auto())
            {
                if (typeof(TDependency).ImplementsOrInheritsFrom(typeof(IApplicationFunctionality)))
                {
                    throw new NotSupportedException(
                        ZString.Format(
                            "{0} may not depend on {1}.  Dependencies should be defined at the feature level using the 'Feature Set'.",
                            typeof(TFunctionality).FormatForLogging(),
                            typeof(TDependency).FormatForLogging()
                        )
                    );
                }

                _dependencyTracker.RegisterDependency(handler);
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenDisabled()
        {
            await base.WhenDisabled();

            using (_PRF_WhenDisabled.Auto())
            {
                metadata.Changed.Event -= OnRequiresUpdate;
                UnsubscribeFromAllFunctionalities();
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            await DelayEnabling();

            await AppaTask.WaitUntil(() => metadata != null);

            name = GetType().Name;
        }

        protected override async AppaTask AfterEnabled()
        {
            await base.AfterEnabled();

            using (_PRF_AfterEnabled.Auto())
            {
                metadata.UpdateFunctionality(this as TFunctionality);
            }
        }

        #region IApplicationFunctionality Members

        public virtual void ApplyMetadata()
        {
            using (_PRF_ApplyMetadata.Auto())
            {
                metadata.UpdateFunctionality(this as TFunctionality);
            }
        }

        void IApplicationFunctionality.UnsubscribeFromAllFunctionalities()
        {
            UnsubscribeFromAllFunctionalities();
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_ApplyMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyMetadata));

        protected static readonly ProfilerMarker _PRF_OnApplyMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(OnRequiresUpdate));

        protected static readonly ProfilerMarker _PRF_UnsubscribeFromAllFunctionalities =
            new ProfilerMarker(_PRF_PFX + nameof(UnsubscribeFromAllFunctionalities));

        #endregion
    }
}
