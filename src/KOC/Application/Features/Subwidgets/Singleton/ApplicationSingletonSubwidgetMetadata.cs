using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Core.ControlModel.Extensions;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Model;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.Functionality.Canvas.Controls.Default;
using Appalachia.UI.Functionality.Images.Controls.Background;
using Appalachia.UI.Functionality.Images.Controls.RoundedBackground;
using Appalachia.UI.Styling;
using Appalachia.UI.Styling.Elements;
using Appalachia.UI.Styling.Fonts;
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
            RegisterDependency<StyleElementDefaultLookup>(i => _styleLookup = i);

            var callbacks = RegisterInstanceCallbacks
               .For<ApplicationSingletonSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget,
                    TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet,
                    TIService, TIWidget, TManager>>();

            callbacks.When.Object<TFeatureMetadata>().IsAvailableThen(i => _featureMetadata = i);
            callbacks.When.Object<TWidgetMetadata>().IsAvailableThen(i => _widgetMetadata = i);
        }

        #region Static Fields and Autoproperties

        private static StyleElementDefaultLookup _styleLookup;
        protected StyleElementDefaultLookup StyleLookup => _styleLookup;

        private static TFeatureMetadata _featureMetadata;
        private static TWidgetMetadata _widgetMetadata;

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf("@HideAllFields || HideRectTransformField")]
        public RectTransformConfig.Override rectTransform;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf("@HideAllFields || HideCanvasField")]
        [SerializeField]
        public CanvasControlConfig.Optional canvas;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf("@HideAllFields || HideBackgroundField")]
        [SerializeField]
        public BackgroundControlConfig.Optional background;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf("@HideAllFields || HideRoundedBackgroundField")]
        [SerializeField]
        public RoundedBackgroundControlConfig.Optional roundedBackground;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf("@HideAllFields || HideFontStyleField")]
        public FontStyleTypes fontStyle;

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

        protected abstract int DefaultPriority { get; }

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
        public BackgroundControlConfig.Optional Background => background;
        public RoundedBackgroundControlConfig.Optional RoundedBackground => roundedBackground;

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
                initializer.Do(this, nameof(DefaultPriority), () => { priority = DefaultPriority; });
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

                RectTransformConfig.Refresh(ref rectTransform, true, this);

                CanvasControlConfig.Refresh(ref canvas, true, this);

                BackgroundControlConfig.Refresh(ref background, true, this);

                RoundedBackgroundControlConfig.Refresh(ref roundedBackground, false, this);
            }
        }

        /// <inheritdoc />
        protected override void OnApply(TSubwidget subwidget)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(subwidget);
                
                rectTransform.Apply(subwidget.RectTransform);
                canvas.Apply(subwidget.canvas);
                background.Apply(subwidget.background);
                roundedBackground.Apply(subwidget.roundedBackground);
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(TSubwidget target)
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
        protected override void UnsuspendResponsiveComponents(TSubwidget target)
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
        protected override void SuspendResponsiveComponents(TSubwidget target)
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

        #region IApplicationSingletonSubwidgetMetadata<TISubwidget,TISubwidgetMetadata> Members

        void IApplicationFunctionalityMetadata<TISubwidget>.Apply(TISubwidget functionality)
        {
            Apply(functionality as TSubwidget);
        }

        public int Priority => priority;
        public CanvasControlConfig.Optional Canvas => canvas;
        public RectTransformConfig.Override RectTransform => rectTransform;
        public bool TransitionsWithFade => transitionsWithFade;
        public float AnimationDuration => animationDuration;

        public FontStyleTypes FontStyle
        {
            get => fontStyle;
            set => fontStyle = value;
        }

        public SubwidgetVisibilityMode WidgetDisabledVisibilityMode => widgetDisabledVisibilityMode;
        public SubwidgetVisibilityMode WidgetEnabledVisibilityMode => widgetEnabledVisibilityMode;
        void IApplicationSubwidgetMetadata.SubscribeResponsiveComponents(IApplicationSubwidget target)
        {
            SubscribeResponsiveComponents(target as TSubwidget);
        }

        void IApplicationSubwidgetMetadata.UnsuspendResponsiveComponents(IApplicationSubwidget target)
        {
            UnsuspendResponsiveComponents(target as TSubwidget);
        }

        void IApplicationSubwidgetMetadata.SuspendResponsiveComponents(IApplicationSubwidget target)
        {
            SuspendResponsiveComponents(target as TSubwidget);
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_GetCanvasGroupInvisibleAlpha =
            new ProfilerMarker(_PRF_PFX + nameof(GetCanvasGroupInvisibleAlpha));

        protected static readonly ProfilerMarker _PRF_GetCanvasGroupVisibleAlpha =
            new ProfilerMarker(_PRF_PFX + nameof(GetCanvasGroupVisibleAlpha));

        #endregion
    }
}
