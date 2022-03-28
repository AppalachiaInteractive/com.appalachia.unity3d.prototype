using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Core.ControlModel.Extensions;
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
using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.Core.Extensions;
using Appalachia.UI.Functionality.Canvas.Controls.Default;
using Appalachia.UI.Functionality.Images.Controls.Background;
using Appalachia.UI.Functionality.Images.Controls.RoundedBackground;
using Appalachia.UI.Styling;
using Appalachia.UI.Styling.Fonts;
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
            RegisterDependency<StyleElementDefaultLookup>(i => _styleLookup = i);

            var callbacks = RegisterInstanceCallbacks
               .For<ApplicationWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet,
                    TIService, TIWidget, TManager>>();

            callbacks.When.Object<TFeatureMetadata>().IsAvailableThen(i => _featureMetadata = i);
        }

        #region Static Fields and Autoproperties

        private static StyleElementDefaultLookup _styleLookup;

        private static TFeatureMetadata _featureMetadata;

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideRectTransformField))]
        public RectTransformConfig.Override rectTransform;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideCanvasField))]
        [SerializeField]
        public CanvasControlConfig.Optional canvas;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideBackgroundField))]
        [SerializeField]
        public BackgroundControlConfig.Optional background;

        [FoldoutGroup(APPASTR.Common)]
        [FormerlySerializedAs("roundedBackgroundStyle")]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideRoundedBackgroundField))]
        [SerializeField]
        public RoundedBackgroundControlConfig.Optional roundedBackground;

        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideFontStyleField))]
        public FontStyleTypes fontStyle;

        [FoldoutGroup(APPASTR.Common + "/" + APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideFeatureEnabledVisibilityModeField))]
        public WidgetVisibilityMode featureEnabledVisibilityMode;

        [FoldoutGroup(APPASTR.Common + "/" + APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideFeatureDisabledVisibilityModeField))]
        public WidgetVisibilityMode featureDisabledVisibilityMode;

        [FoldoutGroup(APPASTR.Common + "/" + APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideTransitionsWithFadeField))]
        public bool transitionsWithFade;

        [FoldoutGroup(APPASTR.Common + "/" + APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0f, 1f)]
        [HideIf(nameof(HideAnimationDurationField))]
        public float animationDuration;

        #endregion

        protected static StyleElementDefaultLookup StyleLookup => _styleLookup;

        protected virtual bool HideAnimationDurationField => false;
        protected virtual bool HideBackgroundField => false;
        protected virtual bool HideCanvasField => false;
        protected virtual bool HideFeatureDisabledVisibilityModeField => false;
        protected virtual bool HideFeatureEnabledVisibilityModeField => false;
        protected virtual bool HideFontStyleField => false;

        protected virtual bool HideRectTransformField => false;
        protected virtual bool HideRoundedBackgroundField => false;
        protected virtual bool HideTransitionsWithFadeField => false;
        public BackgroundControlConfig.Optional Background => background;
        public CanvasControlConfig.Optional Canvas => canvas;

        public RectTransformConfig.Override RectTransform => rectTransform;
        public RoundedBackgroundControlConfig.Optional RoundedBackground => roundedBackground;

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

                RectTransformConfig.Refresh(ref rectTransform, true, this);

                CanvasControlConfig.Refresh(ref canvas, true, this);

                BackgroundControlConfig.Refresh(ref background, true, this);

                RoundedBackgroundControlConfig.Refresh(ref roundedBackground, false, this);
            }
        }

        protected override void AfterApplying(TWidget functionality)
        {
            using (_PRF_AfterApplying.Auto())
            {
                base.AfterApplying(functionality);
                
                functionality.RefreshWidgetVisuals();
            }
        }

        /// <inheritdoc />
        protected override void OnApply(TWidget widget)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(widget);
                
                rectTransform.Apply(widget.RectTransform);

                canvas.Apply(widget.canvas);

                background.Apply(widget.background);

                roundedBackground.Apply(widget.roundedBackground);
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(TWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);
                
                target.SubscribeToChanges(OnChanged);
                rectTransform.SubscribeToChanges(OnChanged);
                canvas.SubscribeToChanges(OnChanged);
                background.SubscribeToChanges(OnChanged);
                roundedBackground.SubscribeToChanges(OnChanged);
            }
        }

        /// <inheritdoc />
        protected override void UnsuspendResponsiveComponents(TWidget target)
        {
            using (_PRF_UnsuspendResponsiveComponents.Auto())
            {
                base.UnsuspendResponsiveComponents(target);
                
                target.UnsuspendChanges();
                rectTransform.UnsuspendChanges();
                canvas.UnsuspendChanges();
                background.UnsuspendChanges();
                roundedBackground.UnsuspendChanges();
            }
        }
        
        /// <inheritdoc />
        protected override void SuspendResponsiveComponents(TWidget target)
        {
            using (_PRF_SuspendResponsiveComponents.Auto())
            {
                base.SuspendResponsiveComponents(target);
                
                target.SuspendChanges();
                rectTransform.SuspendChanges();
                canvas.SuspendChanges();
                background.SuspendChanges();
                roundedBackground.SuspendChanges();
            }
        }
        #region Profiling

        protected static readonly ProfilerMarker _PRF_GetCanvasGroupInvisibleAlpha =
            new ProfilerMarker(_PRF_PFX + nameof(GetCanvasGroupInvisibleAlpha));

        protected static readonly ProfilerMarker _PRF_GetCanvasGroupVisibleAlpha =
            new ProfilerMarker(_PRF_PFX + nameof(GetCanvasGroupVisibleAlpha));

        #endregion
    }
}
