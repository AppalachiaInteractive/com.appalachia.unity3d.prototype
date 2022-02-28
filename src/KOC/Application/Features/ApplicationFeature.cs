using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features
{
    [ExecutionOrder(ExecutionOrders.Feature)]
    [CallStaticConstructorInEditor]
    public abstract partial class ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
                                                     TIWidget, TManager> :
        ApplicationFunctionality<TFeature, TFeatureMetadata, TManager>,
        IApplicationFeature<TFeature, TFeatureMetadata>
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

        protected const string GROUP_NAME = "Feature";

        #endregion

        static ApplicationFeature()
        {
            /*
             * intentional use of base class ApplicationFeature<> to ensure that this callback
             * runs before other TFeature callbacks.
             */
            RegisterInstanceCallbacks
               .For<ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
                    TManager>>()
               .When.Behaviour(instance)
               .AndBehaviour<TManager>()
               .AreAvailableThen(
                    (thisInstance, _) =>
                    {
                        var parentObject = thisInstance.GetFeatureParentObject();

                        thisInstance.transform.SetParent(parentObject.transform);
                    }
                );
        }

        #region Static Fields and Autoproperties

        private static TFunctionalitySet _functionalitySet;

        #endregion

        #region Fields and Autoproperties

        private GameObject _serviceParentObject;

        [ShowInInspector, ReadOnly, HorizontalGroup("State"), PropertyOrder(-1000), NonSerialized]
        private bool _isEnabled;

        [NonSerialized] private bool _hasBeenEnabledPreviously;

        #endregion

        protected static TFunctionalitySet FunctionalitySet
        {
            get
            {
                if (_functionalitySet == null)
                {
                    _functionalitySet = new TFunctionalitySet();
                }

                return _functionalitySet;
            }
        }

        public bool IsEnabled => _isEnabled;
        public GameObject ServiceParentObject => GetServiceParentObject();

        public GameObject WidgetParentObject => GetWidgetParentObject();

        public int ServiceCount => _functionalitySet?.Services?.Count ?? 0;
        public int WidgetCount => _functionalitySet?.Widgets?.Count ?? 0;

        public IReadOnlyList<TIService> Services => FunctionalitySet.Services;
        public IReadOnlyList<TIWidget> Widgets => FunctionalitySet.Widgets;

        public void AddService<TService>(TService service)
            where TService : MonoBehaviour, TIService
        {
            using (_PRF_AddService.Auto())
            {
                FunctionalitySet.AddService(service);
            }
        }

        public void AddWidget<TWidget>(TWidget widget)
            where TWidget : MonoBehaviour, TIWidget
        {
            using (_PRF_AddWidget.Auto())
            {
                FunctionalitySet.AddWidget(widget);
            }
        }

        public async AppaTask DisableFeature()
        {
            await AppaTask.WaitUntil(() => FullyInitialized);

            for (var widgetIndex = 0; widgetIndex < Widgets.Count; widgetIndex++)
            {
                var widget = Widgets[widgetIndex];

                widget.DisableFeature();
            }

            for (var serviceIndex = 0; serviceIndex < Services.Count; serviceIndex++)
            {
                var service = Services[serviceIndex];

                service.DisableFeature();
            }

            _isEnabled = false;
            await BeforeDisable();
        }

        public async AppaTask EnableFeature()
        {
            await AppaTask.WaitUntil(() => FullyInitialized);

            if (!_hasBeenEnabledPreviously)
            {
                _hasBeenEnabledPreviously = true;
                await BeforeFirstEnable();
            }

            for (var widgetIndex = 0; widgetIndex < Widgets.Count; widgetIndex++)
            {
                var widget = Widgets[widgetIndex];

                widget.EnableFeature();
            }

            for (var serviceIndex = 0; serviceIndex < Services.Count; serviceIndex++)
            {
                var service = Services[serviceIndex];

                service.EnableFeature();
            }

            _isEnabled = true;

            await BeforeEnable();
        }

        public async AppaTask ToggleFeature()
        {
            //using (_PRF_ToggleFeature.Auto())
            {
                if (_isEnabled)
                {
                    await DisableFeature();
                }
                else
                {
                    await EnableFeature();
                }
            }
        }

        protected internal virtual GameObject GetWidgetParentObject()
        {
            using (_PRF_GetWidgetParentObject.Auto())
            {
                return Manager.GetWidgetParentObject();
            }
        }

        protected internal GameObject GetFeatureParentObject()
        {
            using (_PRF_GetFeatureParentObject.Auto())
            {
                return Manager.GetFeatureParentObject();
            }
        }

        protected internal GameObject GetServiceParentObject()
        {
            using (_PRF_GetServiceParentObject.Auto())
            {
                if (_serviceParentObject == null)
                {
                    gameObject.GetOrAddChild(ref _serviceParentObject, APPASTR.ObjectNames.Services, false);
                }

                return _serviceParentObject;
            }
        }

        protected virtual async AppaTask BeforeDisable()
        {
            await AppaTask.CompletedTask;
        }

        protected virtual async AppaTask BeforeEnable()
        {
            await AppaTask.CompletedTask;
        }

        protected virtual async AppaTask BeforeFirstEnable()
        {
            await AppaTask.CompletedTask;
        }

        // ReSharper disable once UnusedParameter.Global
        protected virtual void OnApplyMetadataInternal()
        {
            using (_PRF_OnApplyMetadataInternal.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                base.UnsubscribeFromAllFunctionalities();

                foreach (var widget in _functionalitySet.Widgets)
                {
                    widget.UnsubscribeFromAllFunctionalities();
                }

                foreach (var service in _functionalitySet.Services)
                {
                    service.UnsubscribeFromAllFunctionalities();
                }
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            await SetToInitialState();
        }

        #region IApplicationFeature<TFeature,TFeatureMetadata> Members

        IEnumerable<IApplicationService> IApplicationFeature.Services =>
            FunctionalitySet.Services.Cast<IApplicationService>();

        IEnumerable<IApplicationWidget> IApplicationFeature.Widgets =>
            FunctionalitySet.Widgets.Cast<IApplicationWidget>();

        public override void ApplyMetadata()
        {
            using (_PRF_ApplyMetadata.Auto())
            {
                base.ApplyMetadata();

                for (var serviceIndex = 0; serviceIndex < Services.Count; serviceIndex++)
                {
                    var service = Services[serviceIndex];
                    service.ApplyMetadata();
                }

                for (var widgetIndex = 0; widgetIndex < Widgets.Count; widgetIndex++)
                {
                    var widget = Widgets[widgetIndex];
                    widget.ApplyMetadata();
                }
            }
        }

        public async AppaTask SetToInitialState()
        {
            if (metadata.startsEnabled)
            {
                await EnableFeature();
            }
            else
            {
                await DisableFeature();
            }
        }

        public virtual void SortServices()
        {
            using (_PRF_SortServices.Auto())
            {
                ServiceParentObject.transform.SortChildren();
            }
        }

        public virtual void SortWidgets()
        {
            using (_PRF_SortWidgets.Auto())
            {
                WidgetParentObject.transform.SortChildren();
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_AddService =
            new ProfilerMarker(_PRF_PFX + nameof(AddService));

        private static readonly ProfilerMarker _PRF_AddWidget =
            new ProfilerMarker(_PRF_PFX + nameof(AddWidget));

        protected static readonly ProfilerMarker _PRF_BeforeDisable =
            new ProfilerMarker(_PRF_PFX + nameof(BeforeDisable));

        protected static readonly ProfilerMarker _PRF_BeforeEnable =
            new ProfilerMarker(_PRF_PFX + nameof(BeforeEnable));

        protected static readonly ProfilerMarker _PRF_BeforeFirstEnable =
            new ProfilerMarker(_PRF_PFX + nameof(BeforeFirstEnable));

        private static readonly ProfilerMarker _PRF_DisableFeature =
            new ProfilerMarker(_PRF_PFX + nameof(DisableFeature));

        private static readonly ProfilerMarker _PRF_EnableFeature =
            new ProfilerMarker(_PRF_PFX + nameof(EnableFeature));

        private static readonly ProfilerMarker _PRF_GetFeatureParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetFeatureParentObject));

        protected static readonly ProfilerMarker _PRF_GetParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetFeatureParentObject));

        protected static readonly ProfilerMarker _PRF_GetServiceParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetServiceParentObject));

        protected static readonly ProfilerMarker _PRF_GetWidgetParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetWidgetParentObject));

        protected static readonly ProfilerMarker _PRF_OnApplyMetadataInternal =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplyMetadataInternal));

        private static readonly ProfilerMarker _PRF_SetToInitialState =
            new ProfilerMarker(_PRF_PFX + nameof(SetToInitialState));

        private static readonly ProfilerMarker _PRF_SortServices =
            new ProfilerMarker(_PRF_PFX + nameof(SortServices));

        protected static readonly ProfilerMarker _PRF_SortWidgets =
            new ProfilerMarker(_PRF_PFX + nameof(SortWidgets));

        private static readonly ProfilerMarker _PRF_ToggleFeature =
            new ProfilerMarker(_PRF_PFX + nameof(ToggleFeature));

        #endregion
    }
}
