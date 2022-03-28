using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Model;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.UI.ControlModel.Controls.Default.Contracts;
using Appalachia.UI.Core.Extensions;
using Appalachia.UI.Functionality.Canvas.Controls.Default;
using Appalachia.UI.Functionality.Images.Controls.Background;
using Appalachia.UI.Functionality.Images.Controls.RoundedBackground;
using Appalachia.UI.Styling;
using Appalachia.Utility.Async;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Execution;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features.Widgets
{
    [ExecutionOrder(ExecutionOrders.Widget)]
    [CallStaticConstructorInEditor]
    [RequireComponent(typeof(RectTransform))]
    public abstract partial class ApplicationWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
                                                    TFunctionalitySet, TIService, TIWidget, TManager> :
        ApplicationFunctionality<TWidget, TWidgetMetadata, TManager>,
        IApplicationWidget
        where TWidget : ApplicationWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet,
            TIService, TIWidget, TManager>, TIWidget
        where TWidgetMetadata : ApplicationWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TFunctionalitySet, TIService, TIWidget, TManager>
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
            TManager>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>
        where TFunctionalitySet : FeatureFunctionalitySet<TIService, TIWidget>, new()
        where TIService : IApplicationService
        where TIWidget : IApplicationWidget
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>, IApplicationFunctionalityManager

    {
        static ApplicationWidget()
        {
            RegisterDependency<StyleElementDefaultLookup>(i => _styleLookup = i);

            RegisterInstanceCallbacks
               .For<ApplicationWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService
                    , TIWidget, TManager>>()
               .When.Behaviour<TFeature>()
               .AndBehaviour<TWidget>()
               .AreAvailableThen(
                    (f, w) =>
                    {
                        _feature = f;
                        _feature.AddWidget(w);
                    }
                );
        }

        #region Static Fields and Autoproperties

        private static StyleElementDefaultLookup _styleLookup;

        private static TFeature _feature;

        #endregion

        #region Fields and Autoproperties

        public AppaEvent.Data VisualUpdate;

        [ShowInInspector, ReadOnly, HorizontalGroup("State"), PropertyOrder(-1000), NonSerialized]
        private bool _isVisible;

        public CanvasControl canvas;
        public BackgroundControl background;
        public RoundedBackgroundControl roundedBackground;
        private Rect _lastRect;

        #endregion

        protected static StyleElementDefaultLookup StyleLookup => _styleLookup;

        public BackgroundControl Background => background;
        public CanvasControl Canvas => canvas;
        public RoundedBackgroundControl RoundedBackground => roundedBackground;

        public TFeature Feature => _feature;

        #region Event Functions

        protected void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                OnUpdate();
            }
        }

        private void OnRectTransformDimensionsChange()
        {
            using (_PRF_OnRectTransformDimensionsChange.Auto())
            {
                if (AppalachiaApplication.IsCompiling)
                {
                    return;
                }

                if (AppalachiaApplication.IsQuitting)
                {
                    return;
                }

                if (AppalachiaApplication.IsDomainReloading)
                {
                    return;
                }

                var currentRect = RectTransform.rect;

                if (!ApplyingMetadata && FullyInitialized)
                {
                    if (currentRect != _lastRect)
                    {
                        VisualUpdate.RaiseEvent();
                    }
                }

                _lastRect = currentRect;
            }
        }

        #endregion

        protected virtual void EnsureWidgetIsCorrectSize()
        {
        }

        protected virtual IAppaUIControl[] GetControls()
        {
            using (_PRF_GetControls.Auto())
            {
                return GetComponentsInChildren<IAppaUIControl>();
            }
        }

        /// <summary>
        ///     Returns the correct parent for the current widget to live under.
        /// </summary>
        /// <returns>The parent <see cref="GameObject" />.</returns>
        protected virtual GameObject GetWidgetParentObject()
        {
            using (_PRF_GetWidgetParentObject.Auto())
            {
                return Feature.GetWidgetParentObject();
            }
        }

        protected virtual void OnUpdate()
        {
            using (_PRF_OnUpdate.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override async AppaTask DelayEnabling()
        {
            await base.DelayEnabling();

            await AppaTask.WaitUntil(() => Manager != null);
            await AppaTask.WaitUntil(() => Manager.FullyInitialized);
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                var unused = RectTransform;

                CanvasControl.Refresh(ref canvas, gameObject, nameof(Canvas));

                BackgroundControl.Refresh(ref background, canvas.ChildContainer, nameof(Background));

                RoundedBackgroundControl.Refresh(
                    ref roundedBackground,
                    canvas.ChildContainer,
                    nameof(RoundedBackground)
                );
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                var parentObject = GetWidgetParentObject();

                transform.SetParent(parentObject.transform);
            }

            await AppaTask.WaitUntil(() => _feature != null);

            using (_PRF_WhenEnabled.Auto())
            {
                UpdateVisibility(_feature.IsEnabled);
            }
        }

        protected void UpdateAnchorMax(Vector2 endValue)
        {
            using (_PRF_UpdateAnchorMax.Auto())
            {
                RectTransform.anchorMax = endValue;
            }
        }

        protected void UpdateAnchorMin(Vector2 endValue)
        {
            using (_PRF_UpdateAnchorMin.Auto())
            {
                RectTransform.anchorMin = endValue;
            }
        }

        protected void UpdateCanvasGroupAlpha(float alpha)
        {
            using (_PRF_UpdateAnchorMax.Auto())
            {
                canvas.CanvasGroup.alpha = alpha;
            }
        }

        protected void ValidateVisibility(bool visibilityChanged = false)
        {
            using (_PRF_ValidateVisibility.Auto())
            {
                void InternalProcessVisibility()
                {
                    if (!metadata.canvas.IsElected)
                    {
                        return;
                    }

                    if (metadata.transitionsWithFade)
                    {
                        if (!visibilityChanged)
                        {
                            return;
                        }

                        if (!metadata.canvas.Value.Canvas.CanvasFadeManager.IsElected)
                        {
                            return;
                        }

                        if (metadata.canvas.Value.Canvas.CanvasFadeManager.Value.passiveMode)
                        {
                            return;
                        }

                        if (_isVisible)
                        {
                            canvas.canvas.CanvasFadeManager.EnsureFadeIn();
                        }
                        else
                        {
                            canvas.canvas.CanvasFadeManager.EnsureFadeOut();
                        }
                    }
                    else
                    {
                        var canvasGroupSettings = metadata.canvas.Value.CanvasGroup;
                        var canvasGroupInnerSettings = canvasGroupSettings;

                        if (_isVisible)
                        {
                            canvas.enabled = true;
                            canvas.canvas.enabled = true;
                            canvas.canvas.canvas.enabled = true;

                            var newAlpha = canvasGroupInnerSettings.alpha.Overriding
                                ? canvasGroupInnerSettings.alpha
                                : metadata.GetCanvasGroupVisibleAlpha();

                            UpdateCanvasGroupAlpha(newAlpha);
                        }
                        else
                        {
                            canvas.enabled = false;
                            canvas.canvas.enabled = false;
                            canvas.canvas.canvas.enabled = false;

                            var newAlpha = metadata.GetCanvasGroupInvisibleAlpha();

                            UpdateCanvasGroupAlpha(newAlpha);
                        }
                    }
                }

                InternalProcessVisibility();

                if (visibilityChanged)
                {
                    VisualUpdate.RaiseEvent();
                }
            }
        }

        internal void RefreshWidgetVisuals()
        {
            using (_PRF_RefreshWidgetVisuals.Auto())
            {
                if (!HasBeenOrIsBeingEnabled)
                {
                    return;
                }

                ValidateVisibility();

                EnsureWidgetIsCorrectSize();

                VisualUpdate.RaiseEvent();
            }
        }

        private void UpdateVisibility(bool isFeatureEnabled)
        {
            using (_PRF_UpdateVisibility.Auto())
            {
                var targetCase = isFeatureEnabled
                    ? metadata.featureEnabledVisibilityMode
                    : metadata.featureDisabledVisibilityMode;

                switch (targetCase)
                {
                    case WidgetVisibilityMode.Visible:
                        SetVisibility(true);
                        break;

                    case WidgetVisibilityMode.NotVisible:
                        SetVisibility(false);
                        break;

                    case WidgetVisibilityMode.DoNotModify:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #region IApplicationWidget Members

        public bool IsVisible => _isVisible;
        public float EffectiveAnchorHeight => IsVisible ? RectTransform.GetAnchorHeight() : 0f;
        public float EffectiveAnchorWidth => IsVisible ? RectTransform.GetAnchorWidth() : 0f;

        [ButtonGroup("Visibility")]
        public void Hide()
        {
            using (_PRF_Hide.Auto())
            {
                SetVisibility(false);
            }
        }

        public void SetVisibility(bool setVisibilityTo)
        {
            using (_PRF_SetVisibility.Auto())
            {
                var visibilityChanged = setVisibilityTo != _isVisible;

                _isVisible = setVisibilityTo;

                ValidateVisibility(visibilityChanged);
            }
        }

        [ButtonGroup("Visibility")]
        public void Show()
        {
            using (_PRF_Show.Auto())
            {
                SetVisibility(true);
            }
        }

        public virtual void OnDisableFeature()
        {
            using (_PRF_OnDisableFeature.Auto())
            {
                UpdateVisibility(false);
            }
        }

        public virtual void OnEnableFeature()
        {
            using (_PRF_OnEnableFeature.Auto())
            {
                UpdateVisibility(true);
            }
        }

        [ButtonGroup("Visibility")]
        [LabelText("Toggle")]
        public void ToggleVisibility()
        {
            using (_PRF_Toggle.Auto())
            {
                if (!FullyInitialized)
                {
                    return;
                }

                SetVisibility(!IsVisible);
            }
        }

        public void ForEachControl(Action<IAppaUIControl> action)
        {
            using (_PRF_ForEachControl.Auto())
            {
                var controls = GetControls();

                foreach (var control in controls)
                {
                    action(control);
                }
            }
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_EnsureWidgetIsCorrectSize =
            new ProfilerMarker(_PRF_PFX + nameof(EnsureWidgetIsCorrectSize));

        private static readonly ProfilerMarker _PRF_ForEachControl =
            new ProfilerMarker(_PRF_PFX + nameof(ForEachControl));

        private static readonly ProfilerMarker _PRF_GetControls = new ProfilerMarker(_PRF_PFX + nameof(GetControls));

        protected static readonly ProfilerMarker _PRF_GetWidgetParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetWidgetParentObject));

        protected static readonly ProfilerMarker _PRF_Hide = new ProfilerMarker(_PRF_PFX + nameof(Hide));

        private static readonly ProfilerMarker _PRF_OnDisableFeature =
            new ProfilerMarker(_PRF_PFX + nameof(OnDisableFeature));

        private static readonly ProfilerMarker _PRF_OnEnableFeature =
            new ProfilerMarker(_PRF_PFX + nameof(OnEnableFeature));

        private static readonly ProfilerMarker _PRF_OnRectTransformDimensionsChange =
            new ProfilerMarker(_PRF_PFX + nameof(OnRectTransformDimensionsChange));

        protected static readonly ProfilerMarker _PRF_OnUpdate = new ProfilerMarker(_PRF_PFX + nameof(OnUpdate));

        protected static readonly ProfilerMarker _PRF_RefreshWidgetVisuals =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshWidgetVisuals));

        protected static readonly ProfilerMarker _PRF_SetVisibility =
            new ProfilerMarker(_PRF_PFX + nameof(SetVisibility));

        protected static readonly ProfilerMarker _PRF_Show = new ProfilerMarker(_PRF_PFX + nameof(Show));

        protected static readonly ProfilerMarker _PRF_Toggle = new ProfilerMarker(_PRF_PFX + nameof(ToggleVisibility));

        protected static readonly ProfilerMarker _PRF_UpdateAnchorMax =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateAnchorMax));

        protected static readonly ProfilerMarker _PRF_UpdateAnchorMin =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateAnchorMin));

        private static readonly ProfilerMarker _PRF_UpdateVisibility =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateVisibility));

        private static readonly ProfilerMarker _PRF_ValidateVisibility =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateVisibility));

        #endregion
    }
}
