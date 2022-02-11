using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Objects.Sets;
using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.UI.Controls.Sets.Background;
using Appalachia.UI.Controls.Sets.Canvas;
using Appalachia.UI.Controls.Sets.RoundedBackground;
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
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(OnChanged))]
        public RectTransformData.Override rectTransform;

        [OnValueChanged(nameof(OnChanged))]
        public CanvasComponentSetData.Optional canvas;

        [OnValueChanged(nameof(OnChanged))]
        public BackgroundComponentSetData.Optional background;

        [FormerlySerializedAs("roundedBackgroundStyle")]
        [OnValueChanged(nameof(OnChanged))]
        public RoundedBackgroundComponentSetData.Optional roundedBackground;

        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        [OnValueChanged(nameof(OnChanged))]
        public FontStyleOverride fontStyle;

        [FoldoutGroup("Transitions")]
        [OnValueChanged(nameof(OnChanged))]
        public bool transitionsWithFade;

        [FoldoutGroup("Transitions")]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0f, 1f)]
        public float animationDuration;

        #endregion

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
            }
        }

        protected override void SubscribeResponsiveComponents(TWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                rectTransform.Changed.Event += OnChanged;
                canvas.Changed.Event += OnChanged;
                background.Changed.Event += OnChanged;
                roundedBackground.Changed.Event += OnChanged;
                fontStyle.Changed.Event += OnChanged;
            }
        }

        protected override void UpdateFunctionality(TWidget widget)
        {
            using (_PRF_Apply.Auto())
            {
                widget.VisuallyChanged.Event += () => UpdateFunctionality(this as TWidgetMetadata, widget);

                UpdateComponent(ref rectTransform, widget.RectTransform);

                UpdateComponentSet(ref canvas, ref widget.canvas, widget);

                UpdateComponentSet(ref background, ref widget.background, widget, widget.canvas.GameObject);

                UpdateComponentSet(
                    ref roundedBackground,
                    ref widget.roundedBackground,
                    widget,
                    widget.canvas.GameObject
                );
            }
        }

        protected void UpdateComponent<TComponent, TComponentData>(
            TComponent component,
            ref TComponentData data,
            Object owner = null)
            where TComponent : Component
            where TComponentData : ComponentData<TComponent, TComponentData>, new()
        {
            using (_PRF_UpdateComponent.Auto())
            {
                if (owner == null)
                {
                    owner = this;
                }

                ComponentData<TComponent, TComponentData>.UpdateComponent(ref data, component, owner);
            }
        }

        protected void UpdateComponent<TComponent, TComponentData>(
            ref TComponentData data,
            TComponent component,
            Object owner = null)
            where TComponent : Component
            where TComponentData : ComponentData<TComponent, TComponentData>, new()
        {
            using (_PRF_UpdateComponent.Auto())
            {
                if (owner == null)
                {
                    owner = this;
                }

                ComponentData<TComponent, TComponentData>.UpdateComponent(ref data, component, owner);
            }
        }

        protected void UpdateComponent<TComponent, TComponentData>(
            TComponent component,
            ref ComponentData<TComponent, TComponentData>.Optional data,
            bool isElected = true,
            Object owner = null)
            where TComponent : Component
            where TComponentData : ComponentData<TComponent, TComponentData>, new()
        {
            using (_PRF_UpdateComponent.Auto())
            {
                if (owner == null)
                {
                    owner = this;
                }

                ComponentData<TComponent, TComponentData>.UpdateComponent(
                    ref data,
                    component,
                    isElected,
                    owner
                );
            }
        }

        protected void UpdateComponent<TComponent, TComponentData>(
            ref ComponentData<TComponent, TComponentData>.Optional data,
            TComponent component,
            bool isElected = true,
            Object owner = null)
            where TComponent : Component
            where TComponentData : ComponentData<TComponent, TComponentData>, new()
        {
            using (_PRF_UpdateComponent.Auto())
            {
                if (owner == null)
                {
                    owner = this;
                }

                ComponentData<TComponent, TComponentData>.UpdateComponent(
                    ref data,
                    component,
                    isElected,
                    owner
                );
            }
        }

        protected void UpdateComponent<TComponent, TComponentData>(
            TComponent component,
            ref ComponentData<TComponent, TComponentData>.Override data,
            bool overriding = true,
            Object owner = null)
            where TComponent : Component
            where TComponentData : ComponentData<TComponent, TComponentData>, new()
        {
            using (_PRF_UpdateComponent.Auto())
            {
                if (owner == null)
                {
                    owner = this;
                }

                ComponentData<TComponent, TComponentData>.UpdateComponent(
                    ref data,
                    component,
                    overriding,
                    owner
                );
            }
        }

        protected void UpdateComponent<TComponent, TComponentData>(
            ref ComponentData<TComponent, TComponentData>.Override data,
            TComponent component,
            bool overriding = true,
            Object owner = null)
            where TComponent : Component
            where TComponentData : ComponentData<TComponent, TComponentData>, new()
        {
            using (_PRF_UpdateComponent.Auto())
            {
                if (owner == null)
                {
                    owner = this;
                }

                ComponentData<TComponent, TComponentData>.UpdateComponent(
                    ref data,
                    component,
                    overriding,
                    owner
                );
            }
        }

        protected void UpdateComponentSet<TSet, TSetData>(
            ref TSet set,
            ref ComponentSetData<TSet, TSetData>.Override setData,
            TWidget widget,
            GameObject parent = null,
            string setName = null)
            where TSet : ComponentSet<TSet, TSetData>, new()
            where TSetData : ComponentSetData<TSet, TSetData>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                if (setName == null)
                {
                    setName = typeof(TWidget).Name;
                }

                if (parent == null)
                {
                    parent = widget.gameObject;
                }

                ComponentSet<TSet, TSetData>.UpdateComponentSet(ref set, ref setData, parent, setName);
            }
        }

        protected void UpdateComponentSet<TSet, TSetData>(
            ref TSet set,
            ref ComponentSetData<TSet, TSetData>.Optional setData,
            TWidget widget,
            GameObject parent = null,
            string setName = null)
            where TSet : ComponentSet<TSet, TSetData>, new()
            where TSetData : ComponentSetData<TSet, TSetData>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                if (setName == null)
                {
                    setName = typeof(TWidget).Name;
                }

                if (parent == null)
                {
                    parent = widget.gameObject;
                }

                ComponentSet<TSet, TSetData>.UpdateComponentSet(ref set, ref setData, parent, setName);
            }
        }

        protected void UpdateComponentSet<TSet, TSetData>(
            ref TSet set,
            ref TSetData setData,
            TWidget widget,
            GameObject parent = null,
            string setName = null)
            where TSet : ComponentSet<TSet, TSetData>, new()
            where TSetData : ComponentSetData<TSet, TSetData>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                setName ??= typeof(TWidget).Name;

                if (parent == null)
                {
                    parent = widget.gameObject;
                }

                ComponentSet<TSet, TSetData>.UpdateComponentSet(ref set, ref setData, parent, setName);
            }
        }

        protected void UpdateComponentSet<TSet, TSetData>(
            ref ComponentSetData<TSet, TSetData>.Override setData,
            ref TSet set,
            TWidget widget,
            GameObject parent = null,
            string setName = null)
            where TSet : ComponentSet<TSet, TSetData>, new()
            where TSetData : ComponentSetData<TSet, TSetData>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                setName ??= typeof(TWidget).Name;

                if (parent == null)
                {
                    parent = widget.gameObject;
                }

                ComponentSet<TSet, TSetData>.UpdateComponentSet(ref set, ref setData, parent, setName);
            }
        }

        protected void UpdateComponentSet<TSet, TSetData>(
            ref ComponentSetData<TSet, TSetData>.Optional setData,
            ref TSet set,
            TWidget widget,
            GameObject parent = null,
            string setName = null)
            where TSet : ComponentSet<TSet, TSetData>, new()
            where TSetData : ComponentSetData<TSet, TSetData>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                setName ??= typeof(TWidget).Name;

                if (parent == null)
                {
                    parent = widget.gameObject;
                }

                ComponentSet<TSet, TSetData>.UpdateComponentSet(ref set, ref setData, parent, setName);
            }
        }

        protected void UpdateComponentSet<TSet, TSetData>(
            ref TSetData setData,
            ref TSet set,
            TWidget widget,
            GameObject parent = null,
            string setName = null)
            where TSet : ComponentSet<TSet, TSetData>, new()
            where TSetData : ComponentSetData<TSet, TSetData>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                setName ??= typeof(TWidget).Name;

                if (parent == null)
                {
                    parent = widget.gameObject;
                }

                ComponentSet<TSet, TSetData>.UpdateComponentSet(ref set, ref setData, parent, setName);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_UpdateComponent =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponent));

        private static readonly ProfilerMarker _PRF_UpdateComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSet));

        #endregion
    }
}
