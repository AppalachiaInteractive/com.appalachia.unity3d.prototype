using System.Diagnostics;
using System.Text.RegularExpressions;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Collections;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Model;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#pragma warning disable 0649

// A UI element to show information about a debug entry
namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Components
{
    public class DebugLogItem : AppalachiaBehaviour<DebugLogItem>, IPointerClickHandler
    {
        #region Fields and Autoproperties

        // Cached components
        [SerializeField] private RectTransform transformComponent;

        [SerializeField] private Image imageComponent;

        [SerializeField] private CanvasGroup canvasGroupComponent;

        [SerializeField] private Text logText;
        [SerializeField] private Image logTypeImage;

        // Objects related to the collapsed count of the debug entry
        [SerializeField] private GameObject logCountParent;
        [SerializeField] private Text logCountText;

        [SerializeField] private RectTransform copyLogButton;

        // Debug entry to show with this log item
        private DebugLogEntry logEntry;

        // Index of the entry in the list of entries
        private int entryIndex;

        private Vector2 logTextOriginalPosition;
        private Vector2 logTextOriginalSize;
        private float copyLogButtonHeight;

        private DebugLogRecycledListView manager;

        #endregion

        public CanvasGroup CanvasGroup => canvasGroupComponent;
        public DebugLogEntry Entry => logEntry;
        public Image Image => imageComponent;
        public int Index => entryIndex;
        public RectTransform Transform => transformComponent;

        // Return a string containing complete information about the debug entry
        [DebuggerStepThrough]
        public override string ToString()
        {
            using (_PRF_ToString.Auto())
            {
                return logEntry.ToString();
            }
        }

        public float CalculateExpandedHeight(string content)
        {
            using (_PRF_CalculateExpandedHeight.Auto())
            {
                var text = logText.text;
                var wrapMode = logText.horizontalOverflow;

                logText.text = content;
                logText.horizontalOverflow = HorizontalWrapMode.Wrap;

                var result = logText.preferredHeight + copyLogButtonHeight;

                logText.text = text;
                logText.horizontalOverflow = wrapMode;

                return Mathf.Max(manager.ItemHeight, result);
            }
        }

        public void CopyLog()
        {
            using (_PRF_CopyLog.Auto())
            {
#if UNITY_EDITOR
                var log = logEntry.ToString();
                if (string.IsNullOrEmpty(log))
                {
                    return;
                }

#if UNITY_EDITOR || UNITY_2018_1_OR_NEWER || ( !UNITY_ANDROID && !UNITY_IOS )
                GUIUtility.systemCopyBuffer = log;
#elif UNITY_ANDROID
			AJC.CallStatic( "CopyText", Context, log );
#elif UNITY_IOS
			_DebugConsole_CopyText( log );
#endif
#endif
            }
        }

        // Hide the collapsed count of the debug entry
        public void HideCount()
        {
            using (_PRF_HideCount.Auto())
            {
                if (logCountParent.activeSelf)
                {
                    logCountParent.SetActive(false);
                }
            }
        }

        public void Initialize(DebugLogRecycledListView manager)
        {
            using (_PRF_Initialize.Auto())
            {
                this.manager = manager;

                logTextOriginalPosition = logText.rectTransform.anchoredPosition;
                logTextOriginalSize = logText.rectTransform.sizeDelta;
                copyLogButtonHeight =
                    copyLogButton.anchoredPosition.y +
                    copyLogButton.sizeDelta.y +
                    2f; // 2f: space between text and button
            }
        }

        public void SetContent(DebugLogEntry logEntry, int entryIndex, bool isExpanded)
        {
            using (_PRF_SetContent.Auto())
            {
                this.logEntry = logEntry;
                this.entryIndex = entryIndex;

                var size = transformComponent.sizeDelta;
                if (isExpanded)
                {
                    logText.horizontalOverflow = HorizontalWrapMode.Wrap;
                    size.y = manager.SelectedItemHeight;

                    if (!copyLogButton.gameObject.activeSelf)
                    {
                        copyLogButton.gameObject.SetActive(true);

                        logText.rectTransform.anchoredPosition = new Vector2(
                            logTextOriginalPosition.x,
                            logTextOriginalPosition.y + (copyLogButtonHeight * 0.5f)
                        );
                        logText.rectTransform.sizeDelta =
                            logTextOriginalSize - new Vector2(0f, copyLogButtonHeight);
                    }
                }
                else
                {
                    logText.horizontalOverflow = HorizontalWrapMode.Overflow;
                    size.y = manager.ItemHeight;

                    if (copyLogButton.gameObject.activeSelf)
                    {
                        copyLogButton.gameObject.SetActive(false);

                        logText.rectTransform.anchoredPosition = logTextOriginalPosition;
                        logText.rectTransform.sizeDelta = logTextOriginalSize;
                    }
                }

                transformComponent.sizeDelta = size;

                logText.text = isExpanded ? logEntry.ToString() : logEntry.logString;
                logTypeImage.sprite = logEntry.logTypeSpriteRepresentation;
            }
        }

        // Show the collapsed count of the debug entry
        public void ShowCount()
        {
            using (_PRF_ShowCount.Auto())
            {
                logCountText.text = logEntry.count.ToString();

                if (!logCountParent.activeSelf)
                {
                    logCountParent.SetActive(true);
                }
            }
        }

        #region IPointerClickHandler Members

        // This log item is clicked, show the debug entry's stack trace
        public void OnPointerClick(PointerEventData eventData)
        {
            using (_PRF_OnPointerClick.Auto())
            {
#if UNITY_EDITOR
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    var regex = Regex.Match(
                        logEntry.stackTrace,
                        @"\(at .*\.cs:[0-9]+\)$",
                        RegexOptions.Multiline
                    );
                    if (regex.Success)
                    {
                        var line = logEntry.stackTrace.Substring(regex.Index + 4, regex.Length - 5);
                        var lineSeparator = line.IndexOf(':');
                        var script =
                            AssetDatabaseManager.LoadAssetAtPath<MonoScript>(
                                line.Substring(0, lineSeparator)
                            );
                        if (script != null)
                        {
                            AssetDatabaseManager.OpenAsset(
                                script,
                                int.Parse(line.Substring(lineSeparator + 1))
                            );
                        }
                    }
                }
                else
                {
                    manager.OnLogItemClicked(this);
                }
#else
			manager.OnLogItemClicked( this );
#endif
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_CalculateExpandedHeight =
            new ProfilerMarker(_PRF_PFX + nameof(CalculateExpandedHeight));

        private static readonly ProfilerMarker _PRF_CopyLog = new ProfilerMarker(_PRF_PFX + nameof(CopyLog));

        private static readonly ProfilerMarker _PRF_HideCount =
            new ProfilerMarker(_PRF_PFX + nameof(HideCount));

        private static readonly ProfilerMarker _PRF_OnPointerClick =
            new ProfilerMarker(_PRF_PFX + nameof(OnPointerClick));

        private static readonly ProfilerMarker _PRF_SetContent =
            new ProfilerMarker(_PRF_PFX + nameof(SetContent));

        private static readonly ProfilerMarker _PRF_ShowCount =
            new ProfilerMarker(_PRF_PFX + nameof(ShowCount));

        private static readonly ProfilerMarker _PRF_ToString =
            new ProfilerMarker(_PRF_PFX + nameof(ToString));

        #endregion
    }
}

#pragma warning restore 0649
