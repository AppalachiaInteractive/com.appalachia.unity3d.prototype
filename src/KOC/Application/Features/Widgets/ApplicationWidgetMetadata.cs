using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Objects.Sets;
using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Model;
using Appalachia.Prototype.KOC.Application.Functionality;
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
    public abstract class ApplicationWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
                                                    TFunctionalitySet, TIService, TIWidget, TManager> :
        ApplicationFunctionalityMetadata<TWidget, TWidgetMetadata, TManager>,
        IApplicationWidgetMetadata<TWidget>
        where TWidget : ApplicationWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TFunctionalitySet, TIService, TIWidget, TManager>, TIWidget
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
        #region Fields and Autoproperties
        
        protected virtual bool ShowRectTransformField => true;
        protected virtual bool ShowCanvasField => true;
        protected virtual bool ShowBackgroundField => true;
        protected virtual bool ShowRoundedBackgroundField => true;
        protected virtual bool ShowFontStyleField => true;
        protected virtual bool ShowFeatureEnabledVisibilityModeField => true;
        protected virtual bool ShowFeatureDisabledVisibilityModeField => true;
        protected virtual bool ShowTransitionsWithFadeField => true;
        protected virtual bool ShowAnimationDurationField => true;
        
        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [ShowIf(nameof(ShowRectTransformField))]
        public RectTransformData.Override rectTransform;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [ShowIf(nameof(ShowCanvasField))]
        public CanvasComponentSetData.Optional canvas;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        [ShowIf(nameof(ShowBackgroundField))]
        public BackgroundComponentSetData.Optional background;

        [FoldoutGroup(APPASTR.Common)]
        [FormerlySerializedAs("roundedBackgroundStyle")]
        [OnValueChanged(nameof(OnChanged))]
        [ShowIf(nameof(ShowRoundedBackgroundField))]
        public RoundedBackgroundComponentSetData.Optional roundedBackground;

        [FoldoutGroup(APPASTR.Common)]
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        [OnValueChanged(nameof(OnChanged))]
        [ShowIf(nameof(ShowFontStyleField))]
        public FontStyleOverride fontStyle;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [ShowIf(nameof(ShowFeatureEnabledVisibilityModeField))]
        public WidgetVisibilityMode featureEnabledVisibilityMode;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [ShowIf(nameof(ShowFeatureDisabledVisibilityModeField))]
        public WidgetVisibilityMode featureDisabledVisibilityMode;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [ShowIf(nameof(ShowTransitionsWithFadeField))]
        public bool transitionsWithFade;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0f, 1f)]
        [ShowIf(nameof(ShowAnimationDurationField))]
        public float animationDuration;

        #endregion

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
                RectTransformData.RefreshAndUpdateComponent(
                    ref rectTransform,
                    true,
                    this,
                    widget.RectTransform
                );

                CanvasComponentSetData.RefreshAndUpdateComponentSet(
                    ref canvas,
                    true,
                    ref widget.canvas,
                    widget.gameObject,
                    typeof(TWidget).Name
                );

                BackgroundComponentSetData.RefreshAndUpdateComponentSet(
                    ref background,
                    true,
                    ref widget.background,
                    widget.canvas.GameObject,
                    typeof(TWidget).Name
                );

                RoundedBackgroundComponentSetData.RefreshAndUpdateComponentSet(
                    ref roundedBackground,
                    false,
                    ref widget.roundedBackground,
                    widget.canvas.GameObject,
                    typeof(TWidget).Name
                );
            }
        }

        protected void RefreshAndUpdateComponentSet<TComponentSet, TComponentSetData>(
            ref TComponentSet set,
            ref TComponentSetData setData,
            TWidget widget,
            GameObject parent = null,
            string setName = null)
            where TComponentSet : ComponentSet<TComponentSet, TComponentSetData>, new()
            where TComponentSetData : ComponentSetData<TComponentSet, TComponentSetData>
        {
            using (_PRF_RefreshAndUpdateComponentSet.Auto())
            {
                setName ??= typeof(TWidget).Name;

                if (parent == null)
                {
                    parent = widget.gameObject;
                }

                ComponentSetData<TComponentSet, TComponentSetData>.RefreshAndUpdateComponentSet(
                    ref setData,
                    ref set,
                    parent,
                    setName
                );
            }
        }

        #region Profiling

        protected static readonly ProfilerMarker _PRF_GetCanvasGroupInvisibleAlpha =
            new ProfilerMarker(_PRF_PFX + nameof(GetCanvasGroupInvisibleAlpha));

        protected static readonly ProfilerMarker _PRF_GetCanvasGroupVisibleAlpha =
            new ProfilerMarker(_PRF_PFX + nameof(GetCanvasGroupVisibleAlpha));

        private static readonly ProfilerMarker _PRF_RefreshAndUpdateComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAndUpdateComponentSet));

        #endregion
    }
}
