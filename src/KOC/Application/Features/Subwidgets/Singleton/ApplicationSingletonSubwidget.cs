using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Model;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.UI.Controls.Extensions;
using Appalachia.UI.Controls.Sets.Canvases.Canvas;
using Appalachia.UI.Controls.Sets.Images.Background;
using Appalachia.UI.Controls.Sets.Images.RoundedBackground;
using Appalachia.UI.Core.Styling;
using Appalachia.Utility.Async;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Execution;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton
{
    [ExecutionOrder(ExecutionOrders.Subwidget)]
    [CallStaticConstructorInEditor]
    [RequireComponent(typeof(RectTransform))]
    public abstract class ApplicationSingletonSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget,
                                                        TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
                                                        TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
                                                        TManager> :
        ApplicationFunctionality<TSubwidget, TSubwidgetMetadata, TManager>,
        IApplicationSingletonSubwidget<TISubwidget, TISubwidgetMetadata>,
        IClickable
        where TSubwidget :
        ApplicationSingletonSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>, TISubwidget,
        IApplicationSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TSubwidgetMetadata :
        ApplicationSingletonSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>,
        TISubwidgetMetadata, IApplicationSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TISubwidget : class, IApplicationSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, IApplicationSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TWidget : ApplicationWidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>, TIWidget
        where TWidgetMetadata : ApplicationWidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata,
            TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
            TManager>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>
        where TFunctionalitySet : FeatureFunctionalitySet<TIService, TIWidget>, new()
        where TIService : IApplicationService
        where TIWidget : IApplicationWidget
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>, IApplicationFunctionalityManager
    {
        #region Constants and Static Readonly

        protected const string GROUP_NAME = "Subwidget";

        #endregion

        static ApplicationSingletonSubwidget()
        {
            RegisterDependency<StyleElementDefaultLookup>(i => _styleElementDefaultLookup = i);

            RegisterInstanceCallbacks
               .For<ApplicationSingletonSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata,
                    TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
                    TManager>>()
               .When.Behaviour<TWidget>()
               .AndBehaviour<TFeature>()
               .AndBehaviour<TSubwidget>()
               .AreAvailableThen(
                    (w, f, s) =>
                    {
                        _feature = f;
                        _widget = w;

                        _widget.AddSubwidget(s);
                    }
                );
        }

        #region Static Fields and Autoproperties

        private static StyleElementDefaultLookup _styleElementDefaultLookup;

        private static TFeature _feature;
        private static TWidget _widget;

        #endregion

        #region Fields and Autoproperties

        public AppaEvent.Data VisualUpdate;

        [ShowInInspector, ReadOnly, HorizontalGroup("State"), PropertyOrder(-1000), NonSerialized]
        private bool _isVisible;

        public CanvasComponentSet canvas;

        public BackgroundComponentSet background;

        public RoundedBackgroundComponentSet roundedBackground;

        private Rect _lastRect;
        private int _priority;

        #endregion

        protected static StyleElementDefaultLookup StyleElementDefaultLookup => _styleElementDefaultLookup;

        public TFeature Feature => _feature;

        public TWidget Widget => _widget;

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

        TISubwidgetMetadata IApplicationSubwidget<TISubwidget, TISubwidgetMetadata>.Metadata => metadata;

        protected virtual void EnsureSubwidgetIsCorrectSize()
        {
        }

        /// <summary>
        ///     Returns the correct parent for the current subwidget to live under.
        /// </summary>
        /// <returns>The parent <see cref="GameObject" />.</returns>
        protected virtual GameObject GetSubwidgetParentObject()
        {
            using (_PRF_GetSubwidgetParentObject.Auto())
            {
                return Widget.SubwidgetParent;
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

                Action DelegateCreator()
                {
                    return RefreshSubwidgetVisuals;
                }

                metadata.SubscribeForUpdates(this as TSubwidget, DelegateCreator);
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                var parentObject = GetSubwidgetParentObject();

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

                        if (!metadata.canvas.Value.CanvasFadeManager.IsElected)
                        {
                            return;
                        }

                        if (metadata.canvas.Value.CanvasFadeManager.Value.passiveMode)
                        {
                            return;
                        }

                        if (_isVisible)
                        {
                            canvas.CanvasFadeManager.EnsureFadeIn();
                        }
                        else
                        {
                            canvas.CanvasFadeManager.EnsureFadeOut();
                        }
                    }
                    else
                    {
                        var canvasGroupSettings = metadata.canvas.Value.CanvasGroup;
                        var canvasGroupInnerSettings = canvasGroupSettings.Value;

                        var usingCanvasGroup = canvasGroupSettings.IsElected;

                        if (_isVisible)
                        {
                            canvas.Canvas.enabled = true;

                            if (!usingCanvasGroup)
                            {
                                return;
                            }

                            var newAlpha = canvasGroupInnerSettings.alpha.Overriding
                                ? canvasGroupInnerSettings.alpha
                                : metadata.GetCanvasGroupVisibleAlpha();

                            UpdateCanvasGroupAlpha(newAlpha);
                        }
                        else
                        {
                            canvas.Canvas.enabled = false;

                            var newAlpha = !usingCanvasGroup ? metadata.GetCanvasGroupInvisibleAlpha() : 0.0f;

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

        private void RefreshSubwidgetVisuals()
        {
            using (_PRF_RefreshSubwidgetVisuals.Auto())
            {
                if (!HasBeenOrIsBeingEnabled)
                {
                    return;
                }

                ValidateVisibility();

                EnsureSubwidgetIsCorrectSize();

                VisualUpdate.RaiseEvent();
            }
        }

        private void UpdateVisibility(bool isFeatureEnabled)
        {
            using (_PRF_UpdateVisibility.Auto())
            {
                var targetCase = isFeatureEnabled
                    ? metadata.widgetEnabledVisibilityMode
                    : metadata.widgetDisabledVisibilityMode;

                switch (targetCase)
                {
                    case SubwidgetVisibilityMode.Visible:
                        SetVisibility(true);
                        break;

                    case SubwidgetVisibilityMode.NotVisible:
                        SetVisibility(false);
                        break;

                    case SubwidgetVisibilityMode.DoNotModify:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #region IApplicationSingletonSubwidget<TISubwidget,TISubwidgetMetadata> Members

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

        public virtual void OnDisableWidget()
        {
            using (_PRF_OnDisableWidget.Auto())
            {
                UpdateVisibility(false);
            }
        }

        public virtual void OnEnableWidget()
        {
            using (_PRF_OnEnableWidget.Auto())
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

        public int Priority => Metadata.Priority;

        #endregion

        #region IClickable Members

        public abstract void OnClicked();

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_EnsureSubwidgetIsCorrectSize =
            new ProfilerMarker(_PRF_PFX + nameof(EnsureSubwidgetIsCorrectSize));

        private static readonly ProfilerMarker _PRF_GetSubwidgetParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetSubwidgetParentObject));

        protected static readonly ProfilerMarker _PRF_Hide = new ProfilerMarker(_PRF_PFX + nameof(Hide));

        protected static readonly ProfilerMarker _PRF_OnClicked = new ProfilerMarker(_PRF_PFX + nameof(OnClicked));

        private static readonly ProfilerMarker _PRF_OnDisableWidget =
            new ProfilerMarker(_PRF_PFX + nameof(OnDisableWidget));

        private static readonly ProfilerMarker _PRF_OnEnableWidget =
            new ProfilerMarker(_PRF_PFX + nameof(OnEnableWidget));

        private static readonly ProfilerMarker _PRF_OnRectTransformDimensionsChange =
            new ProfilerMarker(_PRF_PFX + nameof(OnRectTransformDimensionsChange));

        protected static readonly ProfilerMarker _PRF_OnUpdate = new ProfilerMarker(_PRF_PFX + nameof(OnUpdate));

        protected static readonly ProfilerMarker _PRF_RefreshSubwidgetVisuals =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshSubwidgetVisuals));

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
