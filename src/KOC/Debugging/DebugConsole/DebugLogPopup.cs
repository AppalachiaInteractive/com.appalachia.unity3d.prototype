using System.Collections;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Debugging.DebugConsole.Settings;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Manager class for the debug popup
namespace Appalachia.Prototype.KOC.Debugging.DebugConsole
{
    [CallStaticConstructorInEditor]
    [ExecutionOrder(ExecutionOrders.DebugLogPopup)]
    public sealed class DebugLogPopup : AppalachiaBehaviour<DebugLogPopup>,
                                        IPointerClickHandler,
                                        IBeginDragHandler,
                                        IDragHandler,
                                        IEndDragHandler
    {
        static DebugLogPopup()
        {
            RegisterDependency<DebugLogManagerSettings>(i => _debugLogManagerSettings = i);
        }

        #region Static Fields and Autoproperties

        private static DebugLogManagerSettings _debugLogManagerSettings;

        #endregion

        #region Fields and Autoproperties

        private bool isPopupBeingDragged;

        // Canvas group to modify visibility of the popup
        private CanvasGroup canvasGroup;

        // Coroutines for simple code-based animations
        private IEnumerator moveToPosCoroutine;

        // Background image that will change color to indicate an alert
        private Image backgroundImage;

        // Number of new debug entries since the log window has been closed
        private int newInfoCount, newWarningCount, newErrorCount;
        private RectTransform popupTransform;

        // Dimensions of the popup divided by 2
        private Vector2 halfSize;
        private Vector2 normalizedPosition;

        [SerializeField] private DebugLogManager debugManager;

        [SerializeField] private Text newInfoCountText;

        [SerializeField] private Text newWarningCountText;

        [SerializeField] private Text newErrorCountText;

        #endregion

        // Hide the popup
        public void Hide()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;

            isPopupBeingDragged = false;
        }

        public void NewLogsArrived(int newInfo, int newWarning, int newError)
        {
            if (newInfo > 0)
            {
                newInfoCount += newInfo;
                newInfoCountText.text = newInfoCount.ToString();
            }

            if (newWarning > 0)
            {
                newWarningCount += newWarning;
                newWarningCountText.text = newWarningCount.ToString();
            }

            if (newError > 0)
            {
                newErrorCount += newError;
                newErrorCountText.text = newErrorCount.ToString();
            }

            if (newErrorCount > 0)
            {
                backgroundImage.color = _debugLogManagerSettings.popup.alertColorError;
            }
            else if (newWarningCount > 0)
            {
                backgroundImage.color = _debugLogManagerSettings.popup.alertColorWarning;
            }
            else
            {
                backgroundImage.color = _debugLogManagerSettings.popup.alertColorInfo;
            }
        }

        // Hides the log window and shows the popup
        public void Show()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;

            // Reset the counters
            Reset();

            // Update position in case resolution was changed while the popup was hidden
            UpdatePosition(true);
        }

        public void UpdatePosition(bool immediately)
        {
            var canvasSize = debugManager.references.canvasTR.rect.size;

            var canvasWidth = canvasSize.x;
            var canvasHeight = canvasSize.y;

            // normalizedPosition allows us to glue the popup to a specific edge of the screen. It becomes useful when
            // the popup is at the right edge and we switch from portrait screen orientation to landscape screen orientation.
            // Without normalizedPosition, popup could jump to bottom or top edges instead of staying at the right edge
            var pos = immediately
                ? new Vector2(normalizedPosition.x * canvasWidth, normalizedPosition.y * canvasHeight)
                : popupTransform.anchoredPosition;

            // Find distances to all four edges
            var distToLeft = (canvasWidth * 0.5f) + pos.x;
            var distToRight = canvasWidth - distToLeft;

            var distToBottom = (canvasHeight * 0.5f) + pos.y;
            var distToTop = canvasHeight - distToBottom;

            var horDistance = Mathf.Min(distToLeft,    distToRight);
            var vertDistance = Mathf.Min(distToBottom, distToTop);

            // Find the nearest edge's coordinates
            if (horDistance < vertDistance)
            {
                if (distToLeft < distToRight)
                {
                    pos = new Vector2((canvasWidth * -0.5f) + halfSize.x, pos.y);
                }
                else
                {
                    pos = new Vector2((canvasWidth * 0.5f) - halfSize.x, pos.y);
                }

                pos.y = Mathf.Clamp(
                    pos.y,
                    (canvasHeight * -0.5f) + halfSize.y,
                    (canvasHeight * 0.5f) - halfSize.y
                );
            }
            else
            {
                if (distToBottom < distToTop)
                {
                    pos = new Vector2(pos.x, (canvasHeight * -0.5f) + halfSize.y);
                }
                else
                {
                    pos = new Vector2(pos.x, (canvasHeight * 0.5f) - halfSize.y);
                }

                pos.x = Mathf.Clamp(
                    pos.x,
                    (canvasWidth * -0.5f) + halfSize.x,
                    (canvasWidth * 0.5f) - halfSize.x
                );
            }

            normalizedPosition.Set(pos.x / canvasWidth, pos.y / canvasHeight);

            // If another smooth movement animation is in progress, cancel it
            if (moveToPosCoroutine != null)
            {
                StopCoroutine(moveToPosCoroutine);
                moveToPosCoroutine = null;
            }

            if (immediately)
            {
                popupTransform.anchoredPosition = pos;
            }
            else
            {
                // Smoothly translate the popup to the specified position
                moveToPosCoroutine = MoveToPosAnimation(pos);
                StartCoroutine(moveToPosCoroutine);
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                popupTransform = (RectTransform)transform;
                backgroundImage = GetComponent<Image>();
                canvasGroup = GetComponent<CanvasGroup>();

                halfSize = popupTransform.sizeDelta * 0.5f;

                var pos = popupTransform.anchoredPosition;
                if ((pos.x != 0f) || (pos.y != 0f))
                {
                    normalizedPosition =
                        pos.normalized; // Respect the initial popup position set in the prefab
                }
                else
                {
                    normalizedPosition = new Vector2(0.5f, 0f); // Right edge by default
                }
            }
        }

        protected override async AppaTask WhenEnabled()
        {
            using (_PRF_WhenEnabled.Auto())
            {
                await base.WhenEnabled();

                newInfoCount = 0;
                newWarningCount = 0;
                newErrorCount = 0;

                newInfoCountText.text = "0";
                newWarningCountText.text = "0";
                newErrorCountText.text = "0";

                backgroundImage.color = debugManager.settings.popup.alertColorNormal;
            }
        }

        // A simple smooth movement animation
        private IEnumerator MoveToPosAnimation(Vector2 targetPos)
        {
            var modifier = 0f;
            var initialPos = popupTransform.anchoredPosition;

            while (modifier < 1f)
            {
                modifier += 4f * Time.unscaledDeltaTime;
                popupTransform.anchoredPosition = Vector2.Lerp(initialPos, targetPos, modifier);

                yield return null;
            }
        }

        #region IBeginDragHandler Members

        public void OnBeginDrag(PointerEventData data)
        {
            isPopupBeingDragged = true;

            // If a smooth movement animation is in progress, cancel it
            if (moveToPosCoroutine != null)
            {
                StopCoroutine(moveToPosCoroutine);
                moveToPosCoroutine = null;
            }
        }

        #endregion

        #region IDragHandler Members

        // Reposition the popup
        public void OnDrag(PointerEventData data)
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    debugManager.references.canvasTR,
                    data.position,
                    data.pressEventCamera,
                    out localPoint
                ))
            {
                popupTransform.anchoredPosition = localPoint;
            }
        }

        #endregion

        #region IEndDragHandler Members

        // Smoothly translate the popup to the nearest edge
        public void OnEndDrag(PointerEventData data)
        {
            isPopupBeingDragged = false;
            UpdatePosition(false);
        }

        #endregion

        #region IPointerClickHandler Members

        // Popup is clicked
        public void OnPointerClick(PointerEventData data)
        {
            // Hide the popup and show the log window
            if (!isPopupBeingDragged)
            {
                debugManager.ShowLogWindow();
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(DebugLogPopup) + ".";

        private static readonly ProfilerMarker _PRF_WhenEnabled =
            new ProfilerMarker(_PRF_PFX + nameof(WhenEnabled));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
