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
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features.Widgets
{
    [CallStaticConstructorInEditor]
    [RequireComponent(typeof(RectTransform))]
    public abstract class ApplicationWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
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
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>
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

        protected virtual void OnUpdate()
        {
            using (_PRF_OnUpdate.Auto())
            {
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                var unused = RectTransform;
            }
        }

        protected override async AppaTask DelayEnabling()
        {
            await base.DelayEnabling();

            await AppaTask.WaitUntil(() => Manager != null);
            await AppaTask.WaitUntil(() => Manager.FullyInitialized);
        }

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                var parentObject = Feature.GetWidgetParentObject();

                transform.SetParent(parentObject.transform);

                metadata.Changed.Event += ExecuteSizeUpdate;
                metadata.Updated.Event += ExecuteSizeUpdate;
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

        private void ExecuteSizeUpdate()
        {
            using (_PRF_ExecuteSizeUpdate.Auto())
            {
                if (!HasBeenEnabled)
                {
                    return;
                }

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

                if (visibilityChanged)
                {
                    if (_isVisible)
                    {
                        if (metadata.transitionsWithFade)
                        {
                            canvas.CanvasFadeManager.EnsureFadeIn();
                        }
                        else
                        {
                            canvas.CanvasGroup.alpha = 1.0f;
                            canvas.Canvas.enabled = true;
                        }
                    }
                    else
                    {
                        if (metadata.transitionsWithFade)
                        {
                            canvas.CanvasFadeManager.EnsureFadeOut();
                        }
                        else
                        {
                            canvas.CanvasGroup.alpha = 0.0f;
                            canvas.Canvas.enabled = false;
                        }
                    }

                    VisuallyChanged.RaiseEvent();
                }
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

        protected static readonly ProfilerMarker _PRF_ExecuteSizeUpdate =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteSizeUpdate));

        protected static readonly ProfilerMarker _PRF_Hide = new ProfilerMarker(_PRF_PFX + nameof(Hide));

        private static readonly ProfilerMarker _PRF_OnRectTransformDimensionsChange =
            new ProfilerMarker(_PRF_PFX + nameof(OnRectTransformDimensionsChange));

        protected static readonly ProfilerMarker _PRF_OnUpdate =
            new ProfilerMarker(_PRF_PFX + nameof(OnUpdate));

        protected static readonly ProfilerMarker _PRF_SetVisibility =
            new ProfilerMarker(_PRF_PFX + nameof(SetVisibility));

        protected static readonly ProfilerMarker _PRF_Show = new ProfilerMarker(_PRF_PFX + nameof(Show));

        protected static readonly ProfilerMarker _PRF_Toggle =
            new ProfilerMarker(_PRF_PFX + nameof(ToggleVisibility));

        protected static readonly ProfilerMarker _PRF_UpdateAnchorMax =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateAnchorMax));

        protected static readonly ProfilerMarker _PRF_UpdateAnchorMin =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateAnchorMin));

        #endregion
    }
}
