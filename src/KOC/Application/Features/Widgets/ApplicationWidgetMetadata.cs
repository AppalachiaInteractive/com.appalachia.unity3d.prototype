using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Objects.Sets;
using Appalachia.Prototype.KOC.Application.Features.Services;
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
        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        public RectTransformData.Override rectTransform;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        public CanvasComponentSetData.Optional canvas;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        public BackgroundComponentSetData.Optional background;

        [FoldoutGroup(APPASTR.Common)]
        [FormerlySerializedAs("roundedBackgroundStyle")]
        [OnValueChanged(nameof(OnChanged))]
        public RoundedBackgroundComponentSetData.Optional roundedBackground;

        [FoldoutGroup(APPASTR.Common)]
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        [OnValueChanged(nameof(OnChanged))]
        public FontStyleOverride fontStyle;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        public WidgetVisibilityMode featureEnabledVisibilityMode;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        public WidgetVisibilityMode featureDisabledVisibilityMode;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        public bool transitionsWithFade;

        [FoldoutGroup(APPASTR.Visibility)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0f, 1f)]
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
                target.VisuallyChanged.Event += OnChanged;
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

        protected void RefreshAndUpdateComponentSet<TSet, TSetData>(
            ref TSet set,
            ref TSetData setData,
            TWidget widget,
            GameObject parent = null,
            string setName = null)
            where TSet : ComponentSet<TSet, TSetData>, new()
            where TSetData : ComponentSetData<TSet, TSetData>
        {
            using (_PRF_RefreshAndUpdateComponentSet.Auto())
            {
                setName ??= typeof(TWidget).Name;

                if (parent == null)
                {
                    parent = widget.gameObject;
                }

                ComponentSetData<TSet, TSetData>.RefreshAndUpdateComponentSet(
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
