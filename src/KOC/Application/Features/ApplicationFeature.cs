using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features
{
    [CallStaticConstructorInEditor]
    [ExecuteAlways]
    public abstract class ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
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
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>
    {
        #region Constants and Static Readonly

        protected const string GROUP_NAME = "Feature";

        #endregion

        static ApplicationFeature()
        {
            RegisterInstanceCallbacks
               .For<ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
                    TManager>>()
               .When.Behaviour(instance)
               .AndBehaviour<TManager>()
               .AreAvailableThen(
                    (thisInstance, _) =>
                    {
                        var parentObject = thisInstance.GetTargetParentObject();

                        thisInstance.transform.SetParent(parentObject.transform);
                    }
                );
        }

        #region Static Fields and Autoproperties

        private static TFunctionalitySet _functionalitySet;

        #endregion

        #region Fields and Autoproperties

        private GameObject _serviceParentObject;

        private GameObject _widgetParentObject;

        private bool _isVisible;
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

        public bool IsVisible => _isVisible;

        public async AppaTask DisableFeature()
        {
            using (_PRF_DisableFeature.Auto())
            {
                await AppaTask.WaitUntil(() => FullyInitialized);

                _isEnabled = false;
                await BeforeDisable();
            }
        }

        public async AppaTask EnableFeature()
        {
            using (_PRF_EnableFeature.Auto())
            {
                await AppaTask.WaitUntil(() => FullyInitialized);

                if (!_hasBeenEnabledPreviously)
                {
                    _hasBeenEnabledPreviously = true;
                    await BeforeFirstEnable();
                }

                _isEnabled = true;

                await BeforeEnable();
            }
        }

        [ButtonGroup("Widgets")]
        [GUIColor(nameof(disableColor))]
        [PropertyOrder(-1)]
        [LabelText("Hide")]
        public async AppaTask HideFeature()
        {
            //using (_PRF_Hide.Auto())
            {
                _isVisible = false;

                await OnHide();
            }
        }

        public async AppaTask ShowFeature()
        {
            //using (_PRF_Show.Auto())
            {
                _isVisible = true;

                await OnShow();
            }
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

        public async AppaTask ToggleVisibility()
        {
            //using (_PRF_ToggleVisibility.Auto())
            {
                if (_isVisible)
                {
                    await HideFeature();
                }
                else
                {
                    await ShowFeature();
                }
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

        protected internal GameObject GetWidgetParentObject()
        {
            using (_PRF_GetWidgetParentObject.Auto())
            {
                if (_widgetParentObject == null)
                {
                    var rootCanvasObject = GetRootCanvasGameObject();

                    rootCanvasObject.GetOrAddChild(
                        ref _widgetParentObject,
                        APPASTR.ObjectNames.Widgets,
                        true
                    );
                }

                return _widgetParentObject;
            }
        }

        protected abstract AppaTask BeforeDisable();

        protected abstract AppaTask BeforeEnable();

        protected abstract AppaTask BeforeFirstEnable();

        protected abstract GameObject GetRootCanvasGameObject();

        protected abstract GameObject GetTargetParentObject();

        protected abstract AppaTask OnHide();

        protected abstract AppaTask OnShow();

        // ReSharper disable once UnusedParameter.Global
        protected virtual void OnApplyMetadataInternal()
        {
            using (_PRF_OnApplyMetadataInternal.Auto())
            {
            }
        }

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

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                if (metadata.startsEnabled)
                {
                    await EnableFeature();
                }

                if (metadata.startsVisible)
                {
                    await ShowFeature();
                }
                else
                {
                    await HideFeature();
                }
            }
        }

        [ButtonGroup("Toggle")]
        [GUIColor(nameof(disableColor))]
        [PropertyOrder(-3)]
        [LabelText("Disable Feature")]
        private void DisableFeatureButton()
        {
            DisableFeature().Forget();
        }

        [ButtonGroup("Toggle")]
        [GUIColor(nameof(enableColor))]
        [PropertyOrder(-4)]
        [LabelText("Enable Feature")]
        private void EnableFeatureButton()
        {
            EnableFeature().Forget();
        }

        [ButtonGroup("Widgets")]
        [GUIColor(nameof(enableColor))]
        [PropertyOrder(-2)]
        [LabelText("Show")]
        private void ShowFeatureButton()
        {
            ShowFeature().Forget();
        }

        #region IApplicationFeature<TFeature,TFeatureMetadata> Members

        public async AppaTask SetToInitialState()
        {
            using (_PRF_SetToInitialState.Auto())
            {
                if (metadata.startsEnabled)
                {
                    await EnableFeature();
                }

                if (metadata.startsVisible)
                {
                    await ShowFeature();
                }

                await AppaTask.CompletedTask;
            }
        }

        #endregion

        #region Profiling

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

        protected static readonly ProfilerMarker _PRF_GetParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetTargetParentObject));

        protected static readonly ProfilerMarker _PRF_GetRootCanvasGameObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetRootCanvasGameObject));

        protected static readonly ProfilerMarker _PRF_GetServiceParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetServiceParentObject));

        protected static readonly ProfilerMarker _PRF_GetWidgetParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetWidgetParentObject));

        protected static readonly ProfilerMarker _PRF_Hide =
            new ProfilerMarker(_PRF_PFX + nameof(HideFeature));

        protected static readonly ProfilerMarker _PRF_OnApplyMetadataInternal =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplyMetadataInternal));

        protected static readonly ProfilerMarker _PRF_OnHide = new ProfilerMarker(_PRF_PFX + nameof(OnHide));

        protected static readonly ProfilerMarker _PRF_OnShow = new ProfilerMarker(_PRF_PFX + nameof(OnShow));

        private static readonly ProfilerMarker _PRF_SetToInitialState =
            new ProfilerMarker(_PRF_PFX + nameof(SetToInitialState));

        protected static readonly ProfilerMarker _PRF_Show =
            new ProfilerMarker(_PRF_PFX + nameof(ShowFeature));

        private static readonly ProfilerMarker _PRF_ToggleFeature =
            new ProfilerMarker(_PRF_PFX + nameof(ToggleFeature));

        protected static readonly ProfilerMarker _PRF_ToggleVisibility =
            new ProfilerMarker(_PRF_PFX + nameof(ToggleVisibility));

        #endregion
    }
}
