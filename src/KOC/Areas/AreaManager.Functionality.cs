using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.ViewScaling;
using Appalachia.Prototype.KOC.Areas.Functionality;
using Appalachia.UI.Core.Extensions;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas
{
    public abstract partial class AreaManager<TManager, TMetadata> : IApplicationFunctionalityManager
    {
        #region Static Fields and Autoproperties

        [NonSerialized]
        [ShowInInspector]
        private static AreaFeatureSet _featureSet;

        private static ViewScalingFeature _viewScalingFeature;

        #endregion

        #region Fields and Autoproperties

        private GameObject _featuresObject;
        private GameObject _widgetsObject;

        private AreaFeatureManager _featureManager;

        #endregion

        protected static AreaFeatureSet FeatureSet => _featureSet;

        public AreaFeatureManager FeatureManager => _featureManager;

        public static void SortFeatures()
        {
            using (_PRF_SortFeatures.Auto())
            {
                var task = AppaTask.WaitUntil(() => (instance != null) && (instance._featuresObject != null))
                                   .ContinueWith(
                                        () =>
                                        {
                                            var featuresObject = instance._featuresObject;
                                            featuresObject.transform.SortChildren();
                                        }
                                    );

                task.Forget();
            }
        }

        protected new static void RegisterDependency<TDependency>(
            SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaBehaviour<TDependency>,
            IRepositoryDependencyTracker<TDependency>
        {
            using (_PRF_RegisterDependency.Auto())
            {
                ValidateRegistration<TDependency>();

                _dependencyTracker.RegisterDependency(handler);
            }
        }

        protected new static void RegisterDependency<TDependency>(
            SingletonAppalachiaObject<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaObject<TDependency>,
            IRepositoryDependencyTracker<TDependency>
        {
            using (_PRF_RegisterDependency.Auto())
            {
                ValidateRegistration<TDependency>();

                _dependencyTracker.RegisterDependency(handler);
            }
        }

        private static void InitializeFunctionality()
        {
            RegisterDependency<ApplicationManager>(i => _applicationManager = i);
            RegisterDependency<ViewScalingFeature>(i => _viewScalingFeature = i);

            _featureSet = new AreaFeatureSet();
            _featureSet.AllFeaturesAvailable.Event += SortFeatures;
        }

        private static void ValidateRegistration<TDependency>()
        {
            using (_PRF_ValidateRegistration.Auto())
            {
                var dependencyType = typeof(TDependency);

                if (dependencyType.ImplementsOrInheritsFrom(typeof(IApplicationWidget)) ||
                    dependencyType.ImplementsOrInheritsFrom(typeof(IApplicationService)))
                {
                    throw new NotSupportedException(
                        "Cannot register a dependency on a service or widget from an area manager."
                    );
                }
            }
        }

        #region IApplicationFunctionalityManager Members

        public GameObject GetFeatureParentObject()
        {
            using (_PRF_GetFeatureParentObject.Auto())
            {
                gameObject.GetOrAddChild(
                    ref _featuresObject,
                    IApplicationFunctionalityManager.PARENT_NAME_FEATURES,
                    false
                );

                _featuresObject.GetOrAddComponent(ref _featureManager);

                _featureManager.Functionality = _featureSet;

                return _featuresObject;
            }
        }

        public GameObject GetWidgetParentObject()
        {
            using (_PRF_GetWidgetParentObject.Auto())
            {
                RootCanvas.ScaledCanvas.gameObject.GetOrAddChild(
                    ref _widgetsObject,
                    IApplicationFunctionalityManager.PARENT_NAME_WIDGETS,
                    true
                );

                (_widgetsObject.transform as RectTransform).FullScreen(true);

                return _widgetsObject;
            }
        }

        public virtual async AppaTask InitializeFeatures()
        {
            using (_PRF_InitializeFeatures.Auto())
            {
                GetWidgetParentObject().GetComponent<RectTransform>().FullScreen(true);
            }

            await _featureSet.SetFeaturesToInitialState();
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_SortFeatures =
            new ProfilerMarker(_PRF_PFX + nameof(SortFeatures));

        private static readonly ProfilerMarker _PRF_GetFeatureParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetFeatureParentObject));

        protected static readonly ProfilerMarker _PRF_GetWidgetParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetWidgetParentObject));

        private static readonly ProfilerMarker _PRF_InitializeFeatures =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeFeatures));

        protected static readonly ProfilerMarker _PRF_ValidateRegistration =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateRegistration));

        #endregion
    }
}
