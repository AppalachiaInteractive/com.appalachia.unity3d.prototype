using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Components.Animation;
using Appalachia.Prototype.KOC.Application.Components.Cursors.Metadata;
using Appalachia.Prototype.KOC.Application.Components.Fading;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors
{
    [ExecuteAlways]
    [ExecutionOrder(ExecutionOrders.CursorInstance)]
    [RequireComponent(typeof(CanvasFadeManager))]
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Animator))]
#if UNITY_EDITOR
    [RequireComponent(typeof(AnimationRemapper))]
#endif
    public class ComplexCursorInstance : AppalachiaApplicationBehaviour
    {
        #region Fields and Autoproperties

        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        [SerializeField]
        public ComplexCursorMetadata metadata;

        [SerializeField, HideLabel, BoxGroup("State Data"), InlineProperty]
        public CursorStateData stateData;

        [HideLabel, InlineProperty, FoldoutGroup("References")]
        public CursorInstanceComponents components;

        [NonSerialized] private bool _hasSearchedSpeedLayer;

        [NonSerialized] private bool _hasFoundSpeedLayer;

        [NonSerialized] private int _speedLayerIndex;

        [NonSerialized, ShowInInspector]
        [FoldoutGroup("State")]
        private bool _activated;

        [FoldoutGroup("State")]
        [SerializeField]
        public Vector2 size;

        [FoldoutGroup("Testing")]
        public CursorInstanceTestAnimationData testData;

        [SerializeField]
        [FoldoutGroup("State")]
        private Vector2 currentVelocity;

        #endregion

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (!_activated)
                {
                    return;
                }

#if UNITY_EDITOR
                HandleCursorAnimation();
#else
                HandleCursorMovement();
#endif

                UpdateAnimatorSpeedLayer();
            }
        }

        #endregion

        [ButtonGroup, PropertyOrder(-10)]
        public void Activate()
        {
            using (_PRF_Activate.Auto())
            {
                _activated = true;
            }
        }

        [ButtonGroup, PropertyOrder(-10)]
        public void Deactivate()
        {
            using (_PRF_Deactivate.Auto())
            {
                _activated = false;
            }
        }

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                if (size == Vector2.zero)
                {
                    size = Vector2.one * 120f;
                }

                components.Initialize(gameObject, size);

                components.cursorManager.CursorTypeChanged -= OnCursorTypeChanged;
                components.cursorManager.CursorTypeChanged += OnCursorTypeChanged;
                components.cursorManager.CursorColorChanged -= OnCursorColorChanged;
                components.cursorManager.CursorColorChanged += OnCursorColorChanged;
                components.cursorManager.CursorLockedChanged -= OnCursorLockedChanged;
                components.cursorManager.CursorLockedChanged += OnCursorLockedChanged;
                components.cursorManager.CursorStateChanged -= OnCursorStateChanged;
                components.cursorManager.CursorStateChanged += OnCursorStateChanged;
                components.cursorManager.CursorVisibilityChanged -= OnCursorVisibilityChanged;
                components.cursorManager.CursorVisibilityChanged += OnCursorVisibilityChanged;
            }
        }

        private void CanvasFadeManagerOnFadeInCompleted()
        {
            using (_PRF_CanvasFadeManagerOnFadeInCompleted.Auto())
            {
                gameObject.SetActive(true);
            }
        }

        private void CanvasFadeManagerOnFadeOutCompleted()
        {
            using (_PRF_CanvasFadeManagerOnFadeOutCompleted.Auto())
            {
                gameObject.SetActive(false);
            }
        }

        private void HandleCursorAnimation()
        {
#if UNITY_EDITOR
            using (_PRF_HandleCursorAnimation.Auto())
            {
                if (testData.animate)
                {
                    var screenSize = components.cursorManager.screenSize;
                    var scaledScreenSize = screenSize * (Vector2.one / components.canvasRect.localScale);

                    var rootPosition = scaledScreenSize * .5f;

                    var radius = scaledScreenSize * testData.animationRadius;

                    var time = Time.time;
                    var previousFrameDuration = Time.deltaTime;
                    var previousTime = time - previousFrameDuration;

                    var animationProgressPercentage = (time % testData.animationMovementDuration) /
                                                      testData.animationMovementDuration;

                    var previousStateChangeProgressPercentage =
                        (previousTime % testData.animationStateChangeDuration) /
                        testData.animationStateChangeDuration;
                    var stateChangeProgressPercentage = (time % testData.animationStateChangeDuration) /
                                                        testData.animationStateChangeDuration;

                    if (testData.animateState)
                    {
                        if (stateChangeProgressPercentage < previousStateChangeProgressPercentage)
                        {
                            var newState = stateData.state + 1;

                            if (!Enum.IsDefined(typeof(CursorState), stateData.state))
                            {
                                newState = CursorState.Normal;
                            }

                            OnCursorStateChanged(newState);
                        }
                    }

                    if (testData.animateMovement)
                    {
                        var angle = animationProgressPercentage * (Mathf.PI / 180f) * 360f;

                        var animatedPosition = components.rectTransform.anchoredPosition;

                        animatedPosition.x = radius.x * Mathf.Cos(angle);
                        animatedPosition.y = radius.y * Mathf.Sin(angle);

                        var finalPosition = rootPosition + animatedPosition;
                        components.rectTransform.anchoredPosition = finalPosition;
                    }

                    EditorApplication.QueuePlayerLoopUpdate();
                }

                if (!testData.animateMovement || !testData.animate)
                {
                    HandleCursorMovement();
                }
            }
#endif
        }

        private void HandleCursorMovement()
        {
            using (_PRF_HandleCursorMovement.Auto())
            {
                if (stateData.locked)
                {
                    return;
                }

                var currentPosition = components.rectTransform.anchoredPosition;

                var nextPosition = components.cursorManager.currentPosition;

                var unscaledNextPosition = nextPosition / components.canvasRect.localScale;

                var realPosition = Vector2.SmoothDamp(
                    currentPosition,
                    unscaledNextPosition,
                    ref currentVelocity,
                    metadata.smoothTime,
                    metadata.maxVelocity
                );

                components.rectTransform.anchoredPosition = realPosition;
            }
        }

        private void OnCursorColorChanged(Color color)
        {
            using (_PRF_OnCursorColorChanged.Auto())
            {
                stateData.color = color;

                foreach (var sprite in components.sprites)
                {
                    sprite.CrossFadeColor(
                        stateData.color,
                        metadata.cursorColorChangeDuration,
                        true,
                        true,
                        true
                    );
                }
            }
        }

        private void OnCursorLockedChanged(bool locked)
        {
            using (_PRF_OnCursorLockedChanged.Auto())
            {
                stateData.locked = locked;
            }
        }

        private void OnCursorStateChanged(CursorState state)
        {
            using (_PRF_OnCursorStateChanged.Auto())
            {
                if (!AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    return;
                }

                var isNormal = components.animator.GetBool(APPASTR.ANIMATOR.Normal);
                var isHovering = components.animator.GetBool(APPASTR.ANIMATOR.Hovering);
                var isPressed = components.animator.GetBool(APPASTR.ANIMATOR.Pressed);
                var isDisabled = components.animator.GetBool(APPASTR.ANIMATOR.Disabled);

                var shouldBeNormal = stateData.state == CursorState.Normal;
                var shouldBeHovering = stateData.state == CursorState.Hovering;
                var shouldBePressed = stateData.state == CursorState.Pressed;
                var shouldBeDisabled = stateData.state == CursorState.Disabled;

                if (isNormal || shouldBeNormal)
                {
                    components.animator.SetBool(APPASTR.ANIMATOR.Normal, shouldBeNormal);
                }

                if (isHovering || shouldBeHovering)
                {
                    components.animator.SetBool(APPASTR.ANIMATOR.Hovering, shouldBeHovering);
                }

                if (isPressed || shouldBePressed)
                {
                    components.animator.SetBool(APPASTR.ANIMATOR.Pressed, shouldBePressed);
                }

                if (isDisabled || shouldBeDisabled)
                {
                    components.animator.SetBool(APPASTR.ANIMATOR.Disabled, shouldBeDisabled);
                }
            }
        }

        private void OnCursorTypeChanged(CursorMetadata oldCursor, CursorMetadata newCursor)
        {
            using (_PRF_OnComplexCursorTypeChanged.Auto())
            {
                var thisCursorWasActive = oldCursor == metadata;
                var thisCursorIsBecomingActive = newCursor == metadata;

                var disableCursor = thisCursorWasActive && !thisCursorIsBecomingActive;
                var enableCursor = !thisCursorWasActive && thisCursorIsBecomingActive;

                var doNothing = !disableCursor && !enableCursor;

                if (doNothing)
                {
                    return;
                }

                if (disableCursor)
                {
                    components.canvasFadeManager.FadeOutCompleted -= CanvasFadeManagerOnFadeOutCompleted;
                    components.canvasFadeManager.FadeOutCompleted += CanvasFadeManagerOnFadeOutCompleted;

                    components.canvasFadeManager.FadeOut();

                    return;
                }

                if (enableCursor)
                {
                    gameObject.SetActive(true);

                    components.canvasFadeManager.Hide();
                    components.canvasFadeManager.FadeIn();
                }
            }
        }

        private void OnCursorVisibilityChanged(bool visible)
        {
            using (_PRF_OnCursorVisibilityChanged.Auto())
            {
                stateData.visible = visible;

                if (visible)
                {
                    components.canvasFadeManager.FadeIn();
                }
                else
                {
                    components.canvasFadeManager.FadeOut();
                }
            }
        }

        private void UpdateAnimatorSpeedLayer()
        {
            using (_PRF_UpdateAnimatorSpeedLayer.Auto())
            {
                if (!_hasSearchedSpeedLayer)
                {
                    _hasSearchedSpeedLayer = true;

                    for (var i = 0; i < components.animator.layerCount; i++)
                    {
                        var layer = components.animator.GetLayerName(i);

                        if (layer == APPASTR.ANIMATOR.LAYERS.SpeedLayer)
                        {
                            _speedLayerIndex = i;
                            _hasFoundSpeedLayer = true;
                            break;
                        }
                    }
                }

                if (_hasFoundSpeedLayer)
                {
                    var normalizedVelocity =
                        Mathf.Abs(currentVelocity.magnitude / metadata.maximumVelocityMagnitude);

                    var clampedVelocity = Mathf.Clamp01(normalizedVelocity);

                    components.animator.SetLayerWeight(_speedLayerIndex, clampedVelocity);
                }
            }
        }

        #region Nested type: CursorInstanceComponents

        [Serializable]
        public struct CursorInstanceComponents
        {
            #region Fields and Autoproperties

#if UNITY_EDITOR
            [SerializeField] public AnimationRemapper animationRemapper;
#endif

            [SerializeField] public Animator animator;

            [SerializeField] public Canvas canvas;

            [SerializeField] public CanvasFadeManager canvasFadeManager;

            [SerializeField] public CanvasGroup canvasGroup;

            [SerializeField] public CanvasRenderer canvasRenderer;

            [SerializeField] public CursorManager cursorManager;

            [SerializeField]
#if UNITY_EDITOR
            [SmartInlineButton(nameof(CreateMetadata), DisableIf = nameof(HideCreateMetadata))]
#endif
            public CursorMetadata metadata;

            public Image[] sprites;

            [NonSerialized] public RectTransform canvasRect;

            public RectTransform rectTransform;

            #endregion

            private bool HideCreateMetadata => metadata != null;

            public void Initialize(GameObject gameObject, Vector2 size)
            {
                if (cursorManager == null)
                {
                    cursorManager = CursorManager.instance;
                }

                gameObject.GetOrCreateComponent(ref canvasRenderer);
                gameObject.GetOrCreateComponent(ref canvasGroup);
                gameObject.GetOrCreateComponent(ref canvasFadeManager);
                gameObject.GetOrCreateComponent(ref rectTransform);
                gameObject.GetComponentInParent(ref canvas);
                gameObject.GetOrCreateComponent(ref animator);
#if UNITY_EDITOR
                gameObject.GetOrCreateComponent(ref animationRemapper);
#endif

                canvas.gameObject.GetComponentInParent(ref canvasRect);

                sprites = gameObject.GetComponentsInChildren<Image>();

                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.zero;
                rectTransform.pivot = Vector2.one * .5f;
                rectTransform.sizeDelta = size;
            }

            #region Profiling

            private const string _PRF_PFX = nameof(CursorInstanceComponents) + ".";

            #endregion

#if UNITY_EDITOR
            private static readonly ProfilerMarker _PRF_CreateMetadata =
                new ProfilerMarker(_PRF_PFX + nameof(CreateMetadata));

            public void CreateMetadata()
            {
                using (_PRF_CreateMetadata.Auto())
                {
                    metadata = AppalachiaObject.LoadOrCreateNew(animationRemapper.gameObject.name);
                }
            }
#endif
        }

        #endregion

        #region Nested type: CursorInstanceTestAnimationData

        [Serializable]
        public struct CursorInstanceTestAnimationData
        {
            #region Fields and Autoproperties

            [ShowInInspector, SerializeField]
            public bool animate;

            [ShowInInspector, SerializeField]
            public bool animateMovement;

            [ShowInInspector, SerializeField]
            public bool animateState;

            [FormerlySerializedAs("_animationDuration")]
            [ShowInInspector, SerializeField, PropertyRange(1f, 10f)]
            public float animationMovementDuration;

            [ShowInInspector, SerializeField, PropertyRange(0.01f, .5f)]
            public float animationRadius;

            [ShowInInspector, SerializeField, PropertyRange(1f, 10f)]
            public float animationStateChangeDuration;

            #endregion
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(ComplexCursorInstance) + ".";

        private static readonly ProfilerMarker _PRF_OnCursorVisibilityChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnCursorVisibilityChanged));

        private static readonly ProfilerMarker _PRF_OnCursorStateChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnCursorStateChanged));

        private static readonly ProfilerMarker _PRF_OnCursorLockedChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnCursorLockedChanged));

        private static readonly ProfilerMarker _PRF_HandleCursorMovement =
            new ProfilerMarker(_PRF_PFX + nameof(HandleCursorMovement));

        private static readonly ProfilerMarker _PRF_UpdateAnimatorSpeedLayer =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateAnimatorSpeedLayer));

        private static readonly ProfilerMarker _PRF_HandleCursorAnimation =
            new ProfilerMarker(_PRF_PFX + nameof(HandleCursorAnimation));

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        private static readonly ProfilerMarker
            _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(Activate));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(Deactivate));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_OnComplexCursorTypeChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnCursorTypeChanged));

        private static readonly ProfilerMarker _PRF_CanvasFadeManagerOnFadeInCompleted =
            new ProfilerMarker(_PRF_PFX + nameof(CanvasFadeManagerOnFadeInCompleted));

        private static readonly ProfilerMarker _PRF_CanvasFadeManagerOnFadeOutCompleted =
            new ProfilerMarker(_PRF_PFX + nameof(CanvasFadeManagerOnFadeOutCompleted));

        private static readonly ProfilerMarker _PRF_OnCursorColorChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnCursorColorChanged));

        #endregion
    }
}
