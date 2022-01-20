using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Delegates;
using Appalachia.Core.Objects.Delegates.Extensions;
using Appalachia.UI.Controls.Extensions;
using Appalachia.UI.Controls.Sets.Background;
using Appalachia.UI.Controls.Sets.Canvas;
using Appalachia.UI.Core.Styling;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Widgets
{
    [CallStaticConstructorInEditor]
    public abstract class
        ApplicationWidget<TWidget, TWidgetMetadata> : ApplicationFunctionality<TWidget, TWidgetMetadata>,
                                                      IApplicationWidget
        where TWidget : ApplicationWidget<TWidget, TWidgetMetadata>
        where TWidgetMetadata : ApplicationWidgetMetadata<TWidget, TWidgetMetadata>

    {
        public event EventHandler SizeChanged;
        public event EventHandler VisibilityChanged;
        public event EventHandler VisuallyChanged;
        public event EventHandler WidgetHidden;
        public event EventHandler WidgetShown;

        #region Constants and Static Readonly

        protected const string GROUP_NAME = "Widget";

        #endregion

        static ApplicationWidget()
        {
            RegisterDependency<StyleElementDefaultLookup>(i => _styleElementDefaultLookup = i);
        }

        #region Static Fields and Autoproperties

        private static StyleElementDefaultLookup _styleElementDefaultLookup;

        #endregion

        #region Fields and Autoproperties

        [ShowInInspector, ReadOnly]
        private bool _isVisible;

        [SerializeField, FoldoutGroup(APPASTR.Canvas), HideLabel]
        public CanvasComponentSet canvas;

        [SerializeField, FoldoutGroup(APPASTR.Background), HideLabel]
        public BackgroundComponentSet background;

        #endregion

        protected static StyleElementDefaultLookup StyleElementDefaultLookup => _styleElementDefaultLookup;
        public RectTransform EffectiveTransform => canvas.Rect;

        public RectTransform rectTransform => _rectTransform;

        protected override bool ParentObjectIsUI => true;
        protected override string ParentObjectName => APPASTR.ObjectNames.Widgets;

        #region Event Functions

        protected virtual void Update()
        {
            using (_PRF_Update.Auto())
            {
                ConfirmVisibilityAccuracy();
            }
        }

        #endregion

        protected abstract void OnApplyMetadataInternal();

        protected abstract void UpdateSizeInternal();

        protected virtual void UpdateComponentVisibility()
        {
        }

        protected override void ApplyMetadataInternal()
        {
            using (_PRF_ApplyMetadataInternal.Auto())
            {
                OnApplyMetadataInternal();
                UpdateSize();
            }
        }

        protected override void BeforeApplyMetadataInternal()
        {
            using (_PRF_BeforeApplyMetadataInternal.Auto())
            {
                base.BeforeApplyMetadataInternal();

                metadata.canvasStyle.PrepareAndConfigure(ref canvas, gameObject, typeof(TWidget).Name);

                metadata.backgroundStyle.PrepareAndConfigure(
                    ref background,
                    canvas.GameObject,
                    typeof(TWidget).Name
                );
            }
        }

        protected void OnSizeChanged()
        {
            using (_PRF_OnSizeChanged.Auto())
            {
                SizeChanged.RaiseEvent();
                VisuallyChanged.RaiseEvent();
            }
        }

        protected void OnVisibilityChanged()
        {
            using (_PRF_OnVisibilityChanged.Auto())
            {
                VisibilityChanged.RaiseEvent();
                VisuallyChanged.RaiseEvent();
            }
        }

        protected void OnWidgetHidden()
        {
            using (_PRF_OnWidgetHidden.Auto())
            {
                WidgetHidden.RaiseEvent();
            }
        }

        protected void OnWidgetShown()
        {
            using (_PRF_OnWidgetShown.Auto())
            {
                WidgetShown.RaiseEvent();
            }
        }

        protected void UpdateAnchorMax(Vector2 endValue)
        {
            using (_PRF_UpdateAnchorMax.Auto())
            {
                _rectTransform.DOAnchorMax(endValue, metadata.animationDuration);
            }
        }

        protected void UpdateAnchorMin(Vector2 endValue)
        {
            using (_PRF_UpdateAnchorMin.Auto())
            {
                _rectTransform.DOAnchorMin(endValue, metadata.animationDuration);
            }
        }

        private void ConfirmVisibilityAccuracy()
        {
            using (_PRF_ConfirmVisibilityAccuracy.Auto())
            {
                if (IsVisible)
                {
                    if (!canvas.CanvasFadeManager.IsFading)
                    {
                        if (canvas.CanvasGroup.alpha < (1.0f - float.Epsilon))
                        {
                            canvas.CanvasGroup.alpha = 1.0f;
                        }
                    }
                }
                else
                {
                    if (!canvas.CanvasFadeManager.IsFading)
                    {
                        if (canvas.CanvasGroup.alpha > float.Epsilon)
                        {
                            canvas.CanvasGroup.alpha = 0.0f;
                        }
                    }
                }
            }
        }

        #region IApplicationWidget Members

        public bool IsVisible => _isVisible;
        public float EffectiveAnchorHeight => IsVisible ? rectTransform.GetAnchorHeight() : 0f;
        public float EffectiveAnchorWidth => IsVisible ? rectTransform.GetAnchorWidth() : 0f;

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

                var wasShown = false;
                var wasHidden = false;

                if (visibilityChanged)
                {
                    if (_isVisible)
                    {
                        wasShown = true;

                        if (metadata.transitionsWithFade)
                        {
                            canvas.CanvasFadeManager.EnsureFadeIn();
                        }
                        else
                        {
                            canvas.CanvasGroup.alpha = 1.0f;
                        }
                    }
                    else
                    {
                        wasHidden = true;

                        if (metadata.transitionsWithFade)
                        {
                            canvas.CanvasFadeManager.EnsureFadeOut();
                        }
                        else
                        {
                            canvas.CanvasGroup.alpha = 0.0f;
                        }
                    }

                    UpdateComponentVisibility();

                    if (doRaiseEvents)
                    {
                        OnVisibilityChanged();
                    }

                    if (wasShown)
                    {
                        OnWidgetShown();
                    }

                    if (wasHidden)
                    {
                        OnWidgetHidden();
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

                if (doRaiseEvents)
                {
                    OnSizeChanged();
                }
            }
        }

        #endregion

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

        private static readonly ProfilerMarker _PRF_OnWidgetHidden =
            new ProfilerMarker(_PRF_PFX + nameof(OnWidgetHidden));

        private static readonly ProfilerMarker _PRF_OnWidgetShown =
            new ProfilerMarker(_PRF_PFX + nameof(OnWidgetShown));

        private static readonly ProfilerMarker _PRF_SetVisibility =
            new ProfilerMarker(_PRF_PFX + nameof(SetVisibility));

        private static readonly ProfilerMarker _PRF_Show = new ProfilerMarker(_PRF_PFX + nameof(Show));

        private static readonly ProfilerMarker _PRF_Toggle =
            new ProfilerMarker(_PRF_PFX + nameof(ToggleVisibility));

        private static readonly ProfilerMarker _PRF_UpdateAnchorMax =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateAnchorMax));

        private static readonly ProfilerMarker _PRF_UpdateAnchorMin =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateAnchorMin));

        protected static readonly ProfilerMarker _PRF_UpdateComponentVisibility =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentVisibility));

        private static readonly ProfilerMarker _PRF_UpdateSize =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSize));

        protected static readonly ProfilerMarker _PRF_UpdateSizeInternal =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSizeInternal));

        #endregion
    }
}
