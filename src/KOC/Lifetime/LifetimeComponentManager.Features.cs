using System;
using Appalachia.CI.Constants;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Core;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.ViewScaling;
using Appalachia.UI.Controls.Extensions;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime
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

        private static ViewScalingFeature _viewScalingFeature;

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup(GROUP_FEATURES), SerializeField]
        private GameObject _widgetsObject;

        [FoldoutGroup(GROUP_FEATURES), SerializeField]
        private GameObject _featuresObject;

        private LifetimeFeatureManager _featureManager;

        #endregion

        public static LifetimeFeatureSet Features => _featureSet;

        public LifetimeFeatureManager FeatureManager => _featureManager;

        public ViewScalingFeature ViewScaling => _viewScalingFeature;

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

                if (_viewScalingFeature == null)
                {
                    _featureSet.RegisterFeature<ViewScalingFeature>(
                        _dependencyTracker,
                        i => { _viewScalingFeature = i; }
                    );
                }

                await _featureSet.SetFeaturesToInitialState();
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
