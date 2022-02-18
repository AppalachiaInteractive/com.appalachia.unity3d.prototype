using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Events;
using Appalachia.Core.Events.Extensions;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.UI.Controls.Extensions;
using Appalachia.UI.Controls.Sets.Background;
using Appalachia.UI.Controls.Sets.Canvas;
using Appalachia.UI.Controls.Sets.RoundedBackground;
using Appalachia.UI.Core.Styling;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features.Widgets
{
    [CallStaticConstructorInEditor]
    [RequireComponent(typeof(RectTransform))]
    public abstract partial class ApplicationWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
                                                    TFunctionalitySet, TIService, TIWidget, TManager> :
        ApplicationFunctionality<TWidget, TWidgetMetadata, TManager>,
        IApplicationWidget
        where TWidget : ApplicationWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TFunctionalitySet, TIService, TIWidget, TManager>
        where TWidgetMetadata : ApplicationWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata
            , TFunctionalitySet, TIService, TIWidget, TManager>
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

        protected const string GROUP_NAME = "Widget";

        #endregion

        static ApplicationWidget()
        {
            RegisterDependency<StyleElementDefaultLookup>(i => _styleElementDefaultLookup = i);

            RegisterInstanceCallbacks
               .For<ApplicationWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet,
                    TIService, TIWidget, TManager>>()
               .When.Behaviour<TFeature>()
               .IsAvailableThen(f => _feature = f);
        }

        #region Static Fields and Autoproperties

        private static StyleElementDefaultLookup _styleElementDefaultLookup;

        private static TFeature _feature;

        #endregion

        #region Fields and Autoproperties

        public AppaEvent.Data VisuallyChanged;

        [ShowInInspector, ReadOnly]
        private bool _isVisible;

        public CanvasComponentSet canvas;

        public BackgroundComponentSet background;

        public RoundedBackgroundComponentSet roundedBackground;

        #endregion

        public static TFeature Feature => _feature;

        protected static StyleElementDefaultLookup StyleElementDefaultLookup => _styleElementDefaultLookup;

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

                if (!ApplyingMetadata && FullyInitialized)
                {
                    VisuallyChanged.RaiseEvent();
                }
            }
        }

        #endregion

        protected virtual void EnsureWidgetIsCorrectSize()
        {
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

                Action DelegateCreator()
                {
                    return RefreshWidgetVisuals;
                }

                metadata.SubscribeForUpdates(this as TWidget, DelegateCreator);
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
                        if (_isVisible)
                        {
                            canvas.Canvas.enabled = true;

                            if (!metadata.canvas.Value.CanvasGroup.IsElected)
                            {
                                return;
                            }

                            canvas.CanvasGroup.alpha =
                                metadata.canvas.Value.CanvasGroup.Value.alpha.Overriding
                                    ? metadata.canvas.Value.CanvasGroup.Value.alpha
                                    : 1.0f;
                        }
                        else
                        {
                            canvas.Canvas.enabled = false;

                            if (!metadata.canvas.Value.CanvasGroup.IsElected)
                            {
                                return;
                            }

                            canvas.CanvasGroup.alpha = 0.0f;
                        }
                    }
                }

                InternalProcessVisibility();

                if (visibilityChanged)
                {
                    VisuallyChanged.RaiseEvent();
                }
            }
        }

        private void RefreshWidgetVisuals()
        {
            using (_PRF_RefreshWidgetVisuals.Auto())
            {
                if (!HasBeenEnabled)
                {
                    return;
                }

                ValidateVisibility();

                EnsureWidgetIsCorrectSize();

                VisuallyChanged.RaiseEvent();
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

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_EnsureWidgetIsCorrectSize =
            new ProfilerMarker(_PRF_PFX + nameof(EnsureWidgetIsCorrectSize));

        private static readonly ProfilerMarker _PRF_GetWidgetParentObject =
            new ProfilerMarker(_PRF_PFX + nameof(GetWidgetParentObject));

        protected static readonly ProfilerMarker _PRF_Hide = new ProfilerMarker(_PRF_PFX + nameof(Hide));

        private static readonly ProfilerMarker _PRF_OnRectTransformDimensionsChange =
            new ProfilerMarker(_PRF_PFX + nameof(OnRectTransformDimensionsChange));

        protected static readonly ProfilerMarker _PRF_OnUpdate =
            new ProfilerMarker(_PRF_PFX + nameof(OnUpdate));

        protected static readonly ProfilerMarker _PRF_RefreshWidgetVisuals =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshWidgetVisuals));

        protected static readonly ProfilerMarker _PRF_SetVisibility =
            new ProfilerMarker(_PRF_PFX + nameof(SetVisibility));

        protected static readonly ProfilerMarker _PRF_Show = new ProfilerMarker(_PRF_PFX + nameof(Show));

        protected static readonly ProfilerMarker _PRF_Toggle =
            new ProfilerMarker(_PRF_PFX + nameof(ToggleVisibility));

        protected static readonly ProfilerMarker _PRF_UpdateAnchorMax =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateAnchorMax));

        protected static readonly ProfilerMarker _PRF_UpdateAnchorMin =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateAnchorMin));

        private static readonly ProfilerMarker _PRF_ValidateVisibility =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateVisibility));

        #endregion
    }
}
