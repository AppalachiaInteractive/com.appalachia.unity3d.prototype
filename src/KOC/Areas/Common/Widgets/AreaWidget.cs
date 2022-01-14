using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Components.Styling;
using Appalachia.Prototype.KOC.Components.UI;
using Appalachia.Prototype.KOC.Extensions;
using Appalachia.UI.Controls.Common.Extensions;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Doozy.Engine.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Common.Widgets
{
    [CallStaticConstructorInEditor]
    public abstract class
        AreaWidget<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata> :
            AreaFunctionality<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata>,
            IAreaWidget
        where TWidget : AreaWidget<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata>
        where TWidgetMetadata : AreaWidgetMetadata<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        public delegate void SizeChangedHandler(RectTransform rectTransform);

        public delegate void VisibilityChangedHandler(RectTransform rectTransform, TWidget widget);

        public delegate void VisualChangeHandler();

        public event SizeChangedHandler SizeChanged;
        public event VisibilityChangedHandler VisibilityChanged;
        public event VisualChangeHandler VisuallyChanged;

        static AreaWidget()
        {
            AreaManager<TAreaManager, TAreaMetadata>.InstanceAvailable += i =>
            {
                var managerView = i.UnscaledView.GameObject;

                GameObject widgetContainerObject = null;
                managerView.GetOrCreateChild(ref widgetContainerObject, APPASTR.ObjectNames.Widgets, true);

                var widgetRectTransform = widgetContainerObject.GetComponent<RectTransform>();
                widgetRectTransform.Reset(RectResetOptions.All);

                instance.gameObject.SetParentTo(widgetContainerObject);
            };

            RegisterDependency<ApplicationStyleElementDefaultLookup>(
                i => _applicationStyleElementDefaultLookup = i
            );
        }

        #region Static Fields and Autoproperties

        private static ApplicationStyleElementDefaultLookup _applicationStyleElementDefaultLookup;

        #endregion

        #region Fields and Autoproperties

        public RectTransform rectTransform;
        public BackgroundCanvasComponentSet components;

        [ShowInInspector, ReadOnly]
        private bool _isVisible;

        #endregion

        protected static ApplicationStyleElementDefaultLookup ApplicationStyleElementDefaultLookup =>
            _applicationStyleElementDefaultLookup;

        protected virtual bool AdjustsAnchor => true;

        protected virtual bool AdjustsOffset => false;
        protected virtual bool AdjustsPivot => false;
        protected virtual bool AdjustsScale => false;

        public bool IsVisible => _isVisible;
        public float EffectiveAnchorHeight => IsVisible ? rectTransform.GetAnchorHeight() : 0f;
        public float EffectiveAnchorWidth => IsVisible ? rectTransform.GetAnchorWidth() : 0f;

        #region Event Functions

        protected virtual void Update()
        {
            using (_PRF_Update.Auto())
            {
                ConfirmVisibilityAccuracy();
            }
        }

        #endregion

        [ButtonGroup("Visibility")]
        public void Hide()
        {
            using (_PRF_Hide.Auto())
            {
                SetVisibility(false);
            }
        }

        public void SetVisibility(bool setVisibilityTo, bool doRaiseEvents = true)
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
                            components.fadeManager.EnsureFadeIn();
                        }
                        else
                        {
                            components.canvasGroup.alpha = 1.0f;
                        }
                    }
                    else
                    {
                        if (metadata.transitionsWithFade)
                        {
                            components.fadeManager.EnsureFadeOut();
                        }
                        else
                        {
                            components.canvasGroup.alpha = 0.0f;
                        }
                    }

                    UpdateComponentVisibility();

                    if (doRaiseEvents)
                    {
                        OnVisibilityChanged();
                    }
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
                SetVisibility(!IsVisible);
            }
        }

        [ButtonGroup("Metadata")]
        public void UpdateSize(bool doRaiseEvents = true)
        {
            using (_PRF_UpdateSize.Auto())
            {
                UpdateSizeInternal();

                if (!AdjustsOffset)
                {
                    rectTransform.offsetMin = Vector2.zero;
                    rectTransform.offsetMax = Vector2.zero;
                }

                if (!AdjustsScale)
                {
                    rectTransform.localScale = Vector3.one;
                }

                if (!AdjustsPivot)
                {
                    rectTransform.pivot = Vector2.one * .5f;
                }

                if (!AdjustsAnchor)
                {
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.zero;
                }

                if (doRaiseEvents)
                {
                    OnSizeChanged();
                }
            }
        }

        protected abstract void OnApplyMetadataInternal();

        protected abstract void UpdateSizeInternal();

        protected virtual void UpdateComponentVisibility()
        {
        }

        protected override void ApplyMetadataInternal()
        {
            using (_PRF_ApplyMetadataInternal.Auto())
            {
                components.background.color = metadata.backgroundColor;
                components.background.rectTransform.FullScreen(true);

                var overrideSortOrder = metadata.sortOrder != 0;
                components.canvas.overrideSorting = overrideSortOrder;
                components.canvas.sortingOrder = metadata.sortOrder;

                OnApplyMetadataInternal();

                UpdateSize();
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                rectTransform = initializer.GetOrCreate(
                    gameObject,
                    rectTransform,
                    nameof(RectTransform),
                    rectTransform == null
                );

                components ??= new BackgroundCanvasComponentSet();
                components.Configure(gameObject, name);
            }
        }

        protected void OnSizeChanged()
        {
            using (_PRF_OnSizeChanged.Auto())
            {
                SizeChanged?.Invoke(rectTransform);
                VisuallyChanged?.Invoke();
            }
        }

        protected void OnVisibilityChanged()
        {
            using (_PRF_OnVisibilityChanged.Auto())
            {
                VisibilityChanged?.Invoke(rectTransform, this as TWidget);
                VisuallyChanged?.Invoke();
            }
        }

        private void ConfirmVisibilityAccuracy()
        {
            using (_PRF_ConfirmVisibilityAccuracy.Auto())
            {
                if (IsVisible)
                {
                    if (!components.fadeManager.IsFading)
                    {
                        if (components.canvasGroup.alpha < (1.0f - float.Epsilon))
                        {
                            components.canvasGroup.alpha = 1.0f;
                        }
                    }
                }
                else
                {
                    if (!components.fadeManager.IsFading)
                    {
                        if (components.canvasGroup.alpha > float.Epsilon)
                        {
                            components.canvasGroup.alpha = 0.0f;
                        }
                    }
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ConfirmVisibilityAccuracy =
            new ProfilerMarker(_PRF_PFX + nameof(ConfirmVisibilityAccuracy));

        private static readonly ProfilerMarker _PRF_Hide = new ProfilerMarker(_PRF_PFX + nameof(Hide));

        protected static readonly ProfilerMarker _PRF_OnApplyMetadataInternal =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplyMetadataInternal));

        private static readonly ProfilerMarker _PRF_OnSizeChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnSizeChanged));

        private static readonly ProfilerMarker _PRF_OnVisibilityChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnVisibilityChanged));

        private static readonly ProfilerMarker _PRF_SetVisibility =
            new ProfilerMarker(_PRF_PFX + nameof(SetVisibility));

        private static readonly ProfilerMarker _PRF_Show = new ProfilerMarker(_PRF_PFX + nameof(Show));

        private static readonly ProfilerMarker _PRF_Toggle =
            new ProfilerMarker(_PRF_PFX + nameof(ToggleVisibility));

        protected static readonly ProfilerMarker _PRF_UpdateComponentVisibility =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentVisibility));

        private static readonly ProfilerMarker _PRF_UpdateSize =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSize));

        protected static readonly ProfilerMarker _PRF_UpdateSizeInternal =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSizeInternal));

        #endregion
    }
}
