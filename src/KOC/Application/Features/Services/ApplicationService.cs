using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Model;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Features.Services
{
    [ExecutionOrder(ExecutionOrders.Service)]
    [CallStaticConstructorInEditor]
    public abstract partial class ApplicationService<TService, TServiceMetadata, TFeature, TFeatureMetadata,
                                                     TFunctionalitySet, TIService, TIWidget, TManager> :
        ApplicationFunctionality<TService, TServiceMetadata, TManager>,
        IApplicationService
        where TService : ApplicationService<TService, TServiceMetadata, TFeature, TFeatureMetadata,
            TFunctionalitySet, TIService, TIWidget, TManager>, TIService
        where TServiceMetadata : ApplicationServiceMetadata<TService, TServiceMetadata, TFeature,
            TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget
            , TManager>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata, TFunctionalitySet,
            TIService, TIWidget, TManager>
        where TFunctionalitySet : FeatureFunctionalitySet<TIService, TIWidget>, new()
        where TIService : IApplicationService
        where TIWidget : IApplicationWidget
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>,
        IApplicationFunctionalityManager
    {
        #region Constants and Static Readonly

        protected const string GROUP_NAME = "Service";

        #endregion

        static ApplicationService()
        {
            /*
             * intentional use of base class ApplicationService<> to ensure that this callback
             * runs before other TService callbacks.
             */
            RegisterInstanceCallbacks
               .For<ApplicationService<TService, TServiceMetadata, TFeature, TFeatureMetadata,
                    TFunctionalitySet, TIService, TIWidget, TManager>>()
               .When.Behaviour(instance)
               .AndBehaviour<TFeature>()
               .AndBehaviour<TManager>()
               .AreAvailableThen(
                    (thisInstance, feature, _) =>
                    {
                        _feature = feature;
                        _feature.AddService(instance);

                        var parentObject = _feature.GetServiceParentObject();

                        thisInstance.transform.SetParent(parentObject.transform);
                    }
                );
        }

        #region Static Fields and Autoproperties

        private static TFeature _feature;

        #endregion

        #region Fields and Autoproperties

        [ShowInInspector, ReadOnly, HorizontalGroup("State"), PropertyOrder(-1000), NonSerialized]
        private bool _isEnabled;

        #endregion

        public static TFeature Feature => _feature;

        protected override bool ShouldSkipUpdate => !_isEnabled || base.ShouldSkipUpdate;

        /// <inheritdoc />
        protected override async AppaTask DelayEnabling()
        {
            await base.DelayEnabling();

            await AppaTask.WaitUntil(() => Manager != null);
            await AppaTask.WaitUntil(() => Manager.FullyInitialized);
        }

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            await AppaTask.WaitUntil(() => _feature != null);
            
            using (_PRF_WhenEnabled.Auto())
            {
                UpdateExecution(_feature.IsEnabled);
            }
        }

        private void UpdateExecution(bool isFeatureEnabled)
        {
            using (_PRF_UpdateExecution.Auto())
            {
                var targetCase = isFeatureEnabled
                    ? metadata.featureEnabledExecutionMode
                    : metadata.featureDisabledExecutionMode;

                switch (targetCase)
                {
                    case ServiceExecutionMode.Enabled:
                        EnableService();
                        break;

                    case ServiceExecutionMode.Disabled:
                        DisableService();
                        break;

                    case ServiceExecutionMode.DoNotModify:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #region IApplicationService Members

        public virtual void DisableService()
        {
            using (_PRF_DisableService.Auto())
            {
                _isEnabled = false;
            }
        }

        public virtual void EnableService()
        {
            using (_PRF_EnableService.Auto())
            {
                _isEnabled = true;
            }
        }

        public virtual void DisableFeature()
        {
            using (_PRF_DisableFeature.Auto())
            {
                UpdateExecution(false);
            }
        }

        public virtual void EnableFeature()
        {
            using (_PRF_EnableFeature.Auto())
            {
                UpdateExecution(true);
            }
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_DisableFeature =
            new ProfilerMarker(_PRF_PFX + nameof(DisableFeature));

        protected static readonly ProfilerMarker _PRF_DisableService =
            new ProfilerMarker(_PRF_PFX + nameof(DisableService));

        protected static readonly ProfilerMarker _PRF_EnableFeature =
            new ProfilerMarker(_PRF_PFX + nameof(EnableFeature));

        protected static readonly ProfilerMarker _PRF_EnableService =
            new ProfilerMarker(_PRF_PFX + nameof(EnableService));

        private static readonly ProfilerMarker _PRF_UpdateExecution =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateExecution));

        #endregion
    }
}
