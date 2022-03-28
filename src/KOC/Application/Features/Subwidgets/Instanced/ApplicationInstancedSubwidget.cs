using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Objects.Routing;
using Appalachia.Prototype.KOC.Application.Features.Availability;
using Appalachia.Prototype.KOC.Application.Features.Availability.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Model;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
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

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced
{
    [ExecutionOrder(ExecutionOrders.Subwidget)]
    [CallStaticConstructorInEditor]
    [RequireComponent(typeof(RectTransform))]
    public abstract partial class ApplicationInstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget,
                                                                TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
                                                                TFeatureMetadata, TFunctionalitySet, TIService,
                                                                TIWidget, TManager> : AppalachiaBehaviour<TSubwidget>,
        IApplicationInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TSubwidget :
        ApplicationInstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>, TISubwidget,
        IApplicationInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TSubwidgetMetadata :
        ApplicationInstancedSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>,
        TISubwidgetMetadata, IApplicationInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TISubwidget : class, IApplicationInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, IApplicationInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TWidget : ApplicationWidgetWithInstancedSubwidgets<TSubwidget, TSubwidgetMetadata, TISubwidget,
            TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>, TIWidget
        where TWidgetMetadata : ApplicationWidgetWithInstancedSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata,
            TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet,
            TIService, TIWidget, TManager>
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

        static ApplicationInstancedSubwidget()
        {
            RegisterDependency<StyleElementDefaultLookup>(i => _styleLookup = i);

            var callbacks = RegisterInstanceCallbacks
               .For<ApplicationInstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata,
                    TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
                    TManager>>();

            callbacks.When.Behaviour<TManager>().IsAvailableThen(manager => { _manager = manager; });
            callbacks.When.Behaviour<TFeature>().IsAvailableThen(feature => _feature = feature);
            callbacks.When.Behaviour<TWidget>().IsAvailableThen(widget => _widget = widget);

            callbacks.When.Behaviour<TFeature>()
                     .AndBehaviour<TWidget>()
                     .AreAvailableThen(
                          (f, w) =>
                          {
                              _feature = f;
                              _widget = w;

                              ObjectEnableEventRouter.SubscribeTo<TSubwidget>(sw => _widget.AddSubwidget(sw));
                          }
                      );
        }

        #region Static Fields and Autoproperties

        [PropertyOrder(-10)]
        [ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        protected static TSubwidgetMetadata metadata;

        private static IFeatureAvailabilitySet _when;

        private static StyleElementDefaultLookup _styleLookup;

        private static TFeature _feature;
        private static TManager _manager;
        private static TWidget _widget;

        #endregion

        #region Fields and Autoproperties

        public AppaEvent.Data VisualUpdate;

        [ShowInInspector, ReadOnly, HorizontalGroup("State"), PropertyOrder(-1000), NonSerialized]
        private bool _isVisible;

        public CanvasControl canvas;

        public BackgroundControl background;

        public RoundedBackgroundControl roundedBackground;

        private Rect _lastRect;
        private int _priority;

        private bool _applyingMetadata;

        #endregion

        protected new static IFeatureAvailabilitySet When
        {
            get
            {
                if (_when == null)
                {
                    _when = new FeatureAvailabilitySet(typeof(TSubwidget));
                }

                return _when;
            }
        }

        protected static StyleElementDefaultLookup StyleLookup => _styleLookup;

        protected static TManager Manager => _manager;

        public TFeature Feature => _feature;

        public TSubwidgetMetadata Metadata => metadata;

        public TWidget Widget => _widget;

        public bool ApplyingMetadata
        {
            get => _applyingMetadata;
            internal set => _applyingMetadata = value;
        }

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

        protected virtual async AppaTask DelayEnabling()
        {
            await AppaTask.WaitUntil(() => Manager != null);
            await AppaTask.WaitUntil(() => Manager.FullyInitialized);
        }

        protected virtual void EnsureSubwidgetIsCorrectSize()
        {
            using (_PRF_EnsureSubwidgetIsCorrectSize.Auto())
            {
            }
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

        protected virtual void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
            }
        }

        protected new static void RegisterDependency<TDependency>(
            SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaBehaviour<TDependency>, IRepositoryDependencyTracker<TDependency>
        {
            using (_PRF_RegisterDependency.Auto())
            {
                /*if (typeof(TDependency).ImplementsOrInheritsFrom(typeof(IApplicationFunctionality)))
                {
                    throw new NotSupportedException(
                        ZString.Format(
                            "{0} may not depend on {1}.  Dependencies should be defined at the feature level using the 'Feature Set'.",
                            typeof(TSubwidget).FormatForLogging(),
                            typeof(TDependency).FormatForLogging()
                        )
                    );
                }*/

                _dependencyTracker.RegisterDependency(handler);
            }
        }

        protected override async AppaTask AfterEnabled()
        {
            await base.AfterEnabled();

            using (_PRF_AfterEnabled.Auto())
            {
                metadata.UpdateFunctionality(this as TSubwidget);
            }
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
            await AppaTask.WaitUntil(() => _feature != null);

            name = GetType().Name;

            using (_PRF_WhenEnabled.Auto())
            {
                var parentObject = GetSubwidgetParentObject();

                transform.SetParent(parentObject.transform);

                UpdateVisibility(_widget.IsVisible);

                metadata.SubscribeToChanges(OnRequiresUpdate);
            }
        }

        protected void OnRequiresUpdate()
        {
            using (_PRF_OnApplyMetadata.Auto())
            {
                if (!FullyInitialized)
                {
                    return;
                }

                metadata.UpdateFunctionality(this as TSubwidget);
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
                canvas.canvas.CanvasGroup.alpha = alpha;
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
                            canvas.Canvas.CanvasFadeManager.EnsureFadeIn();
                        }
                        else
                        {
                            canvas.Canvas.CanvasFadeManager.EnsureFadeOut();
                        }
                    }
                    else
                    {
                        var canvasGroupSettings = metadata.canvas.Value.Canvas.CanvasGroup;
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

        private void UpdateVisibility(bool isWidgetVisible)
        {
            using (_PRF_UpdateVisibility.Auto())
            {
                var targetCase = isWidgetVisible
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

        #region IApplicationInstancedSubwidget<TISubwidget,TISubwidgetMetadata> Members

        TISubwidgetMetadata IApplicationSubwidget<TISubwidget, TISubwidgetMetadata>.Metadata => metadata;

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

        public virtual void ApplyMetadata()
        {
            using (_PRF_ApplyMetadata.Auto())
            {
                metadata.UpdateFunctionality(this as TSubwidget);
            }
        }

        void IApplicationFunctionality.UnsubscribeFromAllFunctionalities()
        {
            UnsubscribeFromAllFunctionalities();
        }

        #endregion

        #region IClickable Members

        public abstract void OnClicked();

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_ApplyMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyMetadata));

        protected static readonly ProfilerMarker _PRF_EnsureSubwidgetIsCorrectSize =
            new ProfilerMarker(_PRF_PFX + nameof(EnsureSubwidgetIsCorrectSize));

        private static readonly ProfilerMarker _PRF_GetSubwidgetParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetSubwidgetParentObject));

        protected static readonly ProfilerMarker _PRF_Hide = new ProfilerMarker(_PRF_PFX + nameof(Hide));

        protected static readonly ProfilerMarker _PRF_OnApplyMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(OnRequiresUpdate));

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

        protected static readonly ProfilerMarker _PRF_UnsubscribeFromAllFunctionalities =
            new ProfilerMarker(_PRF_PFX + nameof(UnsubscribeFromAllFunctionalities));

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
