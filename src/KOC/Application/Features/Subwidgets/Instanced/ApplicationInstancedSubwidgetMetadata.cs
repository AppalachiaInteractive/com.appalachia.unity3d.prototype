using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Components.Sets;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Model;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.UI.Controls.Sets.Canvases.Canvas;
using Appalachia.UI.Controls.Sets.Images.Background;
using Appalachia.UI.Controls.Sets.Images.RoundedBackground;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Styling.Fonts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Collections;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Extensions.Debugging;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

// ReSharper disable UnusedMember.Global

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced
{
    [CallStaticConstructorInEditor]
    public abstract partial class ApplicationInstancedSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget,
                                                                        TISubwidgetMetadata, TWidget, TWidgetMetadata,
                                                                        TFeature, TFeatureMetadata, TFunctionalitySet,
                                                                        TIService, TIWidget, TManager> :
        AppalachiaObject<TSubwidgetMetadata>,
        IApplicationInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
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
        static ApplicationInstancedSubwidgetMetadata()
        {
            var callbacks = RegisterInstanceCallbacks
               .For<ApplicationInstancedSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget,
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
        [SerializeField]
        public int priority;

        [SerializeField, HideInInspector]
        private bool _showAdvancedOptions;

        private ReusableDelegateCollection<TSubwidget> _delegates;

        [NonSerialized] private bool _showAllFields;

        [SerializeField, HideInInspector]
        private bool _hideAllFields;

        [SerializeField, HideInInspector]
        private bool _disableAllFields;

        [SerializeField, HideInInspector]
        private bool _suspendFieldApplication;

        private Func<bool> _shouldEnable;

        [SerializeField, HideInInspector]
        private bool _notReadyForRelease;

        /// <summary>
        ///     Offers notifications whenever this metadata is applied to a subwidget.
        ///     Use this to drive any further behaviour needed to keep the subwidget in sync.
        /// </summary>
        public AppaEvent.Data Updated;

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

        public bool NotReadyForRelease => _notReadyForRelease;

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

        /// <summary>
        ///     Subscribes the provided subwidget to metadata updates, using the provided delegate
        ///     creator to produce an event handler.
        /// </summary>
        /// <param name="subwidget">The subwidget to subscribe.</param>
        /// <param name="delegateCreator">
        ///     A function that produces an event handler.  Essentially,
        ///     this is the "what you want to happen" when the event is raised.
        /// </param>
        public void SubscribeForUpdates(TSubwidget subwidget, Func<Action> delegateCreator)
        {
            using (_PRF_SubscribeForUpdates.Auto())
            {
                _delegates ??= new();
                _delegates.Subscribe(subwidget, ref Updated, delegateCreator);

                if (Updated.SubscriberCount == 0)
                {
                    APPADEBUG.BREAKPOINT(() => nameof(Updated), subwidget);
                }
            }
        }

        /// <summary>
        ///     Unsubscribes the provided subwidget from any subsequent metadata updates.
        ///     This will only have an impact if you have previously subscribed for updates
        ///     via <see cref="SubscribeForUpdates" />.
        /// </summary>
        /// <param name="subwidget">The subwidget to unsubscribe.</param>
        public void UnsubscribeFromUpdates(TSubwidget subwidget)
        {
            using (_PRF_UnsubscribeFromUpdates.Auto())
            {
                _delegates ??= new();
                _delegates.Unsubscribe(subwidget, ref Updated);
            }
        }

        /// <summary>
        ///     Given a subwidget, applies this metadata to it to ensure that its
        ///     using the current metadata settings.  Like a "settings sync".
        /// </summary>
        /// <param name="subwidget">The subwidget to update.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="subwidget" /> is null.</exception>
        public void UpdateFunctionality(TSubwidget subwidget)
        {
            using (_PRF_UpdateFunctionality.Auto())
            {
                if (subwidget == null)
                {
                    throw new ArgumentNullException(
                        nameof(subwidget),
                        "Applying metadata before the subwidget has been assigned!"
                    );
                }

                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (subwidget == null)
                        {
                            return;
                        }

                        ExecuteUpdateFunctionalityInternal(subwidget);
                    }

                    return SubscribableDelegate;
                }

                _delegates ??= new();

                _delegates.Subscribe(subwidget, ref subwidget.Changed, CreateSubscribableDelegate);
                _delegates.Subscribe(subwidget, ref Changed,           CreateSubscribableDelegate);

                if (subwidget.Changed.SubscriberCount == 0)
                {
                    APPADEBUG.BREAKPOINT(() => nameof(subwidget.Changed), subwidget);
                }

                if (Changed.SubscriberCount == 0)
                {
                    APPADEBUG.BREAKPOINT(() => nameof(Changed), subwidget);
                }

                ExecuteUpdateFunctionalityInternal(subwidget);
            }
        }

        protected virtual void AfterUpdateFunctionality(TSubwidget subwidget)
        {
            using (_PRF_AfterUpdateFunctionality.Auto())
            {
                SubscribeResponsiveComponents(subwidget);

                Updated.RaiseEvent();

                subwidget.ApplyingMetadata = false;
            }
        }

        protected virtual void BeforeUpdateFunctionality(TSubwidget subwidget)
        {
            using (_PRF_BeforeUpdateFunctionality.Auto())
            {
                subwidget.ApplyingMetadata = true;
            }
        }

        protected virtual void SubscribeResponsiveComponents(TSubwidget target)
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

        protected virtual void UpdateFunctionalityInternal(TSubwidget subwidget)
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

        /// <summary>
        ///     Returns an asset name which which concatenates the current subwidget <see cref="Type" />.<see cref="Type.Name" />
        ///     with the <see cref="Type" />.<see cref="Type.Name" /> of the provided <see cref="T" />.
        /// </summary>
        /// <typeparam name="T">The type whose name should be the second half of the output name.</typeparam>
        /// <returns>The formatted name.</returns>
        /// <example>
        ///     If the <see cref="TSubwidget" /> is "MySpecialWidget", and
        ///     <see cref="T" /> is "MySpecialComponent", the resulting output will be:
        ///     "MySpecialWidgetMySpecialComponent"
        /// </example>
        protected static string GetAssetName<T>()
        {
            using (_PRF_GetAssetName.Auto())
            {
                return typeof(TSubwidget).Name + typeof(T).Name;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
#if UNITY_EDITOR
                ValidateAddressableInformation();
#endif

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

        protected void RefreshAndApply<TComponentSet, TComponentSetData>(
            ref TComponentSet set,
            ref TComponentSetData setData,
            TSubwidget subwidget,
            GameObject parent = null,
            string setName = null)
            where TComponentSet : ComponentSet<TComponentSet, TComponentSetData>, new()
            where TComponentSetData : ComponentSetData<TComponentSet, TComponentSetData>, new()
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                setName ??= typeof(TSubwidget).Name;

                if (parent == null)
                {
                    parent = subwidget.gameObject;
                }

                ComponentSetData<TComponentSet, TComponentSetData>.RefreshAndApply(
                    ref setData,
                    ref set,
                    parent,
                    setName,
                    this
                );
            }
        }

        private void ExecuteUpdateFunctionalityInternal(TSubwidget subwidget)
        {
            using (_PRF_ExecuteUpdateFunctionalityInternal.Auto())
            {
                BeforeUpdateFunctionality(subwidget);
                UpdateFunctionalityInternal(subwidget);
                AfterUpdateFunctionality(subwidget);
            }
        }

        #region IApplicationInstancedSubwidgetMetadata<TISubwidget,TISubwidgetMetadata> Members

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

        private static readonly ProfilerMarker _PRF_RefreshAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAndApply));

        private static readonly ProfilerMarker _PRF_GetAssetName = new ProfilerMarker(_PRF_PFX + nameof(GetAssetName));

        protected static readonly ProfilerMarker _PRF_AfterUpdateFunctionality =
            new ProfilerMarker(_PRF_PFX + nameof(AfterUpdateFunctionality));

        protected static readonly ProfilerMarker _PRF_BeforeUpdateFunctionality =
            new ProfilerMarker(_PRF_PFX + nameof(BeforeUpdateFunctionality));

        private static readonly ProfilerMarker _PRF_ExecuteUpdateFunctionalityInternal =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteUpdateFunctionalityInternal));

        private static readonly ProfilerMarker _PRF_SubscribeForUpdates =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeForUpdates));

        protected static readonly ProfilerMarker _PRF_SubscribeResponsiveComponents =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeResponsiveComponents));

        private static readonly ProfilerMarker _PRF_UnsubscribeFromUpdates =
            new ProfilerMarker(_PRF_PFX + nameof(UnsubscribeFromUpdates));

        protected static readonly ProfilerMarker _PRF_UpdateFunctionality =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateFunctionality));

        protected static readonly ProfilerMarker _PRF_UpdateFunctionalityInternal =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateFunctionalityInternal));

        #endregion
    }
}
