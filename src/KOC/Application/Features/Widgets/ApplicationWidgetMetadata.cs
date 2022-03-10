using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Components.Sets;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Model;
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
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Application.Features.Widgets
{
    [CallStaticConstructorInEditor]
    public abstract class ApplicationWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
                                                    TFunctionalitySet, TIService, TIWidget, TManager> :
        ApplicationFunctionalityMetadata<TWidget, TWidgetMetadata, TManager>,
        IApplicationWidgetMetadata<TWidget>
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
        static ApplicationWidgetMetadata()
        {
            var callbacks = RegisterInstanceCallbacks
               .For<ApplicationWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet,
                    TIService, TIWidget, TManager>>();

            callbacks.When.Object<TFeatureMetadata>().IsAvailableThen(i => _featureMetadata = i);
        }

        #region Static Fields and Autoproperties

        private static TFeatureMetadata _featureMetadata;

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideRectTransformField))]
        public RectTransformData.Override rectTransform;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideCanvasField))]
        [SerializeField]
        public CanvasComponentSetData.Optional canvas;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideBackgroundField))]
        [SerializeField]
        public BackgroundComponentSetData.Optional background;

        [FoldoutGroup(APPASTR.Common)]
        [FormerlySerializedAs("roundedBackgroundStyle")]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideRoundedBackgroundField))]
        [SerializeField]
        public RoundedBackgroundComponentSetData.Optional roundedBackground;

        [FoldoutGroup(APPASTR.Common)]
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideFontStyleField))]
        public FontStyleOverride fontStyle;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideFeatureEnabledVisibilityModeField))]
        public WidgetVisibilityMode featureEnabledVisibilityMode;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideFeatureDisabledVisibilityModeField))]
        public WidgetVisibilityMode featureDisabledVisibilityMode;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideTransitionsWithFadeField))]
        public bool transitionsWithFade;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0f, 1f)]
        [HideIf(nameof(HideAnimationDurationField))]
        public float animationDuration;

        #endregion

        protected virtual bool HideAnimationDurationField => false;
        protected virtual bool HideBackgroundField => false;
        protected virtual bool HideCanvasField => false;
        protected virtual bool HideFeatureDisabledVisibilityModeField => false;
        protected virtual bool HideFeatureEnabledVisibilityModeField => false;
        protected virtual bool HideFontStyleField => false;

        protected virtual bool HideRectTransformField => false;
        protected virtual bool HideRoundedBackgroundField => false;
        protected virtual bool HideTransitionsWithFadeField => false;

        protected TFeatureMetadata FeatureMetadata => _featureMetadata;

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

                initializer.Do(this, nameof(animationDuration), () => { animationDuration = .2f; });

                initializer.Do(
                    this,
                    nameof(featureEnabledVisibilityMode),
                    () => featureEnabledVisibilityMode = WidgetVisibilityMode.Visible
                );
                initializer.Do(
                    this,
                    nameof(featureDisabledVisibilityMode),
                    () => featureDisabledVisibilityMode = WidgetVisibilityMode.NotVisible
                );
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(TWidget target)
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
        protected override void UpdateFunctionalityInternal(TWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                RectTransformData.RefreshAndApply(ref rectTransform, true, this, widget.RectTransform);

                CanvasComponentSetData.RefreshAndApply(
                    ref canvas,
                    true,
                    ref widget.canvas,
                    widget.gameObject,
                    typeof(TWidget).Name,
                    this
                );

                BackgroundComponentSetData.RefreshAndApply(
                    ref background,
                    true,
                    ref widget.background,
                    widget.canvas.GameObject,
                    typeof(TWidget).Name,
                    this
                );

                RoundedBackgroundComponentSetData.RefreshAndApply(
                    ref roundedBackground,
                    false,
                    ref widget.roundedBackground,
                    widget.canvas.GameObject,
                    typeof(TWidget).Name,
                    this
                );
            }
        }

        #region Profiling

        protected static readonly ProfilerMarker _PRF_GetCanvasGroupInvisibleAlpha =
            new ProfilerMarker(_PRF_PFX + nameof(GetCanvasGroupInvisibleAlpha));

        protected static readonly ProfilerMarker _PRF_GetCanvasGroupVisibleAlpha =
            new ProfilerMarker(_PRF_PFX + nameof(GetCanvasGroupVisibleAlpha));

        private static readonly ProfilerMarker _PRF_RefreshAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAndApply));

        #endregion
    }
}
