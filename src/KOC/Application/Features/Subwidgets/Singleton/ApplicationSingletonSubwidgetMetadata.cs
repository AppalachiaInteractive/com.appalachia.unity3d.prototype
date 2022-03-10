using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Model;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.UI.Controls.Sets.Canvases.Canvas;
using Appalachia.UI.Controls.Sets.Images.Background;
using Appalachia.UI.Controls.Sets.Images.RoundedBackground;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Styling.Fonts;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

// ReSharper disable UnusedMember.Global

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton
{
    [CallStaticConstructorInEditor]
    public abstract class ApplicationSingletonSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget,
                                                                TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
                                                                TFeatureMetadata, TFunctionalitySet, TIService,
                                                                TIWidget, TManager> :
        ApplicationFunctionalityMetadata<TSubwidget, TSubwidgetMetadata, TManager>,
        IApplicationSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
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
        static ApplicationSingletonSubwidgetMetadata()
        {
            var callbacks = RegisterInstanceCallbacks
               .For<ApplicationSingletonSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget,
                    TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet,
                    TIService, TIWidget, TManager>>();

            callbacks.When.Object<TFeatureMetadata>().IsAvailableThen(i => _featureMetadata = i);
            callbacks.When.Object<TWidgetMetadata>().IsAvailableThen(i => _widgetMetadata = i);
        }

        #region Static Fields and Autoproperties

        private static TFeatureMetadata _featureMetadata;
        private static TWidgetMetadata _widgetMetadata;

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf("@HideAllFields || HideRectTransformField")]
        public RectTransformData.Override rectTransform;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf("@HideAllFields || HideCanvasField")]
        [SerializeField]
        public CanvasComponentSetData.Optional canvas;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf("@HideAllFields || HideBackgroundField")]
        [SerializeField]
        public BackgroundComponentSetData.Optional background;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf("@HideAllFields || HideRoundedBackgroundField")]
        [SerializeField]
        public RoundedBackgroundComponentSetData.Optional roundedBackground;

        [FoldoutGroup(APPASTR.Common)]
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf("@HideAllFields || HideFontStyleField")]
        public FontStyleOverride fontStyle;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf("@HideAllFields || HideFeatureEnabledVisibilityModeField")]
        public SubwidgetVisibilityMode widgetEnabledVisibilityMode;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf("@HideAllFields || HideFeatureDisabledVisibilityModeField")]
        public SubwidgetVisibilityMode widgetDisabledVisibilityMode;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf("@HideAllFields || HideTransitionsWithFadeField")]
        public bool transitionsWithFade;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0f, 1f)]
        [HideIf("@HideAllFields || HideAnimationDurationField")]
        public float animationDuration;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf("@HideAllFields || HidePriorityField")]
        public int priority;

        #endregion

        protected virtual bool HideAnimationDurationField => false;
        protected virtual bool HideBackgroundField => false;
        protected virtual bool HideCanvasField => false;
        protected virtual bool HideFeatureDisabledVisibilityModeField => false;
        protected virtual bool HideFeatureEnabledVisibilityModeField => false;
        protected virtual bool HideFontStyleField => false;
        protected virtual bool HidePriorityField => false;

        protected virtual bool HideRectTransformField => false;
        protected virtual bool HideRoundedBackgroundField => false;
        protected virtual bool HideTransitionsWithFadeField => false;

        protected TFeatureMetadata FeatureMetadata => _featureMetadata;

        protected TWidgetMetadata WidgetMetadata => _widgetMetadata;

        public virtual float GetCanvasGroupInvisibleAlpha()
        {
            using (_PRF_GetCanvasGroupInvisibleAlpha.Auto())
            {
                return 0.0f;
            }
        }

        public virtual float GetCanvasGroupVisibleAlpha()
        {
            using (_PRF_GetCanvasGroupVisibleAlpha.Auto())
            {
                return 1.0f;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
                    this,
                    nameof(FontStyleOverride),
                    fontStyle == null,
                    () =>
                    {
                        fontStyle = LoadOrCreateNew<FontStyleOverride>(
                            GetAssetName<FontStyleOverride>(),
                            ownerType: typeof(ApplicationManager)
                        );
                    }
                );
                initializer.Do(
                    this,
                    nameof(priority),
                    () =>
                    {
                        if (priority == 0)
                        {
                            priority = 100;
                        }
                    }
                );
                initializer.Do(
                    this,
                    nameof(widgetEnabledVisibilityMode),
                    () => widgetEnabledVisibilityMode = SubwidgetVisibilityMode.Visible
                );
                initializer.Do(
                    this,
                    nameof(widgetDisabledVisibilityMode),
                    () => widgetDisabledVisibilityMode = SubwidgetVisibilityMode.NotVisible
                );
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(TSubwidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                target.Changed.Event += OnChanged;
                rectTransform.Changed.Event += OnChanged;
                canvas.Changed.Event += OnChanged;
                background.Changed.Event += OnChanged;
                roundedBackground.Changed.Event += OnChanged;
                fontStyle.Changed.Event += OnChanged;
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(TSubwidget subwidget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                RectTransformData.RefreshAndApply(ref rectTransform, true, this, subwidget.RectTransform);

                CanvasComponentSetData.RefreshAndApply(
                    ref canvas,
                    true,
                    ref subwidget.canvas,
                    subwidget.gameObject,
                    typeof(TSubwidget).Name,
                    this
                );

                BackgroundComponentSetData.RefreshAndApply(
                    ref background,
                    false,
                    ref subwidget.background,
                    subwidget.canvas.GameObject,
                    typeof(TSubwidget).Name,
                    this
                );

                RoundedBackgroundComponentSetData.RefreshAndApply(
                    ref roundedBackground,
                    false,
                    ref subwidget.roundedBackground,
                    subwidget.canvas.GameObject,
                    typeof(TSubwidget).Name,
                    this
                );
            }
        }

        #region IApplicationSingletonSubwidgetMetadata<TISubwidget,TISubwidgetMetadata> Members

        void IApplicationFunctionalityMetadata<TISubwidget>.UpdateFunctionality(TISubwidget functionality)
        {
            UpdateFunctionality(functionality as TSubwidget);
        }

        public int Priority => priority;
        public BackgroundComponentSetData.Optional Background => background;
        public CanvasComponentSetData.Optional Canvas => canvas;
        public RectTransformData.Override RectTransform => rectTransform;
        public RoundedBackgroundComponentSetData.Optional RoundedBackground => roundedBackground;
        public bool TransitionsWithFade => transitionsWithFade;
        public float AnimationDuration => animationDuration;
        public FontStyleOverride FontStyle
        {
            get => fontStyle;
            set => fontStyle = value;
        }

        public SubwidgetVisibilityMode WidgetDisabledVisibilityMode => widgetDisabledVisibilityMode;
        public SubwidgetVisibilityMode WidgetEnabledVisibilityMode => widgetEnabledVisibilityMode;

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_GetCanvasGroupInvisibleAlpha =
            new ProfilerMarker(_PRF_PFX + nameof(GetCanvasGroupInvisibleAlpha));

        protected static readonly ProfilerMarker _PRF_GetCanvasGroupVisibleAlpha =
            new ProfilerMarker(_PRF_PFX + nameof(GetCanvasGroupVisibleAlpha));

        #endregion
    }
}
