using System;
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
    public partial class LifetimeComponentManager
    {
        #region Constants and Static Readonly

        private const string GROUP_FEATURES = GROUP_BASE + PARENT_NAME_FEATURES;

        private const string PARENT_NAME_FEATURES = "Features";
        private const string PARENT_NAME_SERVICES = "Services";
        private const string PARENT_NAME_WIDGETS = "Widgets";

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

        #endregion

        public static LifetimeFeatureSet Features => _featureSet;

        public GameObject FeaturesObject
        {
            get
            {
                if (_featuresObject == null)
                {
                    gameObject.GetOrAddChild(ref _featuresObject, PARENT_NAME_FEATURES, false);
                }

                return _featuresObject;
            }
        }

        public GameObject WidgetsObject => _widgetsObject;
        public ViewScalingFeature ViewScaling => _viewScalingFeature;

        private async AppaTask InitializeFeatures()
        {
            using (_PRF_InitializeFeatures.Auto())
            {
                gameObject.GetOrAddChild(ref _featuresObject, PARENT_NAME_FEATURES, false);

                _rootCanvas.GameObject.GetOrAddChild(ref _widgetsObject, PARENT_NAME_WIDGETS, true);
                _widgetsObject.GetComponent<RectTransform>().FullScreen(true);

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

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeFeatures =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeFeatures));

        #endregion
    }
}
