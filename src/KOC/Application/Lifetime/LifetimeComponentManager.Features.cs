using System;
using Appalachia.CI.Constants;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.ViewScaling;
using Appalachia.UI.Core.Extensions;
using Appalachia.Utility.Async;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime
{
    public partial class LifetimeComponentManager : IApplicationFunctionalityManager
    {
        #region Constants and Static Readonly

        private const string GROUP_FEATURES = GROUP_BASE + APPASTR.Features;

        #endregion

        #region Static Fields and Autoproperties

        [NonSerialized]
        [ShowInInspector]
        [FoldoutGroup(GROUP_FEATURES)]
        private static LifetimeFeatureSet _featureSet;

        #endregion

        #region Fields and Autoproperties

        private ViewScalingFeature _viewScalingFeature;

        [FoldoutGroup(GROUP_FEATURES), SerializeField]
        private GameObject _widgetsObject;

        [FoldoutGroup(GROUP_FEATURES), SerializeField]
        private GameObject _featuresObject;

        private LifetimeFeatureManager _lifetimeFeatureManager;

        public AppaEvent<LifetimeFeatureManager>.Data LifetimeFeatureManagerReady;
        public AppaEvent<ViewScalingFeature>.Data ViewScalingFeatureReady;
        public AppaEvent<CursorFeature>.Data CursorFeatureReady;
        public AppaEvent<RuntimeGizmoDrawerFeature>.Data RuntimeGizmoDrawerFeatureReady;

        private CursorFeature _cursorFeature;
        private RuntimeGizmoDrawerFeature _runtimeGizmoDrawerFeature;

        #endregion

        public CursorFeature CursorFeature => _cursorFeature;

        public LifetimeFeatureManager LifetimeFeatureManager => _lifetimeFeatureManager;

        public LifetimeFeatureSet Features => _featureSet;

        public RuntimeGizmoDrawerFeature RuntimeGizmoDrawerFeature => _runtimeGizmoDrawerFeature;

        public ViewScalingFeature ViewScalingFeature => _viewScalingFeature;

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

                _featuresObject.GetOrAddComponent(ref _lifetimeFeatureManager);

                _lifetimeFeatureManager.Functionality = _featureSet;

                return _featuresObject;
            }
        }

        public GameObject GetWidgetParentObject()
        {
            using (_PRF_GetWidgetParentObject.Auto())
            {
                _rootCanvas.ScaledCanvas.gameObject.GetOrAddChild(
                    ref _widgetsObject,
                    IApplicationFunctionalityManager.PARENT_NAME_WIDGETS,
                    true
                );

                return _widgetsObject;
            }
        }

        public async AppaTask InitializeFeatures()
        {
            using (_PRF_InitializeFeatures.Auto())
            {
                GetWidgetParentObject().GetComponent<RectTransform>().FullScreen(true);

                _featureSet.RegisterFeature<ViewScalingFeature>(
                    _dependencyTracker,
                    i => { _viewScalingFeature = i; }
                );
                _featureSet.RegisterFeature<CursorFeature>(_dependencyTracker, i => { _cursorFeature = i; });
                _featureSet.RegisterFeature<RuntimeGizmoDrawerFeature>(
                    _dependencyTracker,
                    i => { _runtimeGizmoDrawerFeature = i; }
                );

                await _featureSet.SetFeaturesToInitialState();

                RuntimeGizmoDrawerFeatureReady.RaiseEvent(RuntimeGizmoDrawerFeature);
                CursorFeatureReady.RaiseEvent(CursorFeature);
                LifetimeFeatureManagerReady.RaiseEvent(LifetimeFeatureManager);
                ViewScalingFeatureReady.RaiseEvent(ViewScalingFeature);
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetFeatureParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetFeatureParentObject));

        private static readonly ProfilerMarker _PRF_GetWidgetParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetWidgetParentObject));

        private static readonly ProfilerMarker _PRF_InitializeFeatures =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeFeatures));

        #endregion
    }
}
