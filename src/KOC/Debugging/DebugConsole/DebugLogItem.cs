﻿using System.Diagnostics;
using System.Text.RegularExpressions;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Debugging.DebugConsole.Collections.Recycling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// A UI element to show information about a debug entry
namespace Appalachia.Prototype.KOC.Debugging.DebugConsole
{
    public class DebugLogItem : AppalachiaBehaviour<DebugLogItem>, IPointerClickHandler
    {
        #region Platform Specific Elements

#if !UNITY_2018_1_OR_NEWER
#if !UNITY_EDITOR && UNITY_ANDROID
		private static AndroidJavaClass m_ajc = null;
		private static AndroidJavaClass AJC
		{
			get
			{
				if( m_ajc == null )
					m_ajc = new AndroidJavaClass( "com.yasirkula.unity.DebugConsole" );

				return m_ajc;
			}
		}

		private static AndroidJavaObject m_context = null;
		private static AndroidJavaObject Context
		{
			get
			{
				if( m_context == null )
				{
					using( AndroidJavaObject unityClass =
 new AndroidJavaClass( "com.unity3d.player.UnityPlayer" ) )
					{
						m_context = unityClass.GetStatic<AndroidJavaObject>( "currentActivity" );
					}
				}

				return m_context;
			}
		}
#elif !UNITY_EDITOR && UNITY_IOS
		[System.Runtime.InteropServices.DllImport( "__Internal" )]
		private static extern void _DebugConsole_CopyText( string text );
#endif
#endif

        #endregion

#pragma warning disable 0649

        // Cached components
        [SerializeField] private RectTransform transformComponent;
        public RectTransform Transform => transformComponent;

        [SerializeField] private Image imageComponent;
        public Image Image => imageComponent;

        [SerializeField] private CanvasGroup canvasGroupComponent;
        public CanvasGroup CanvasGroup => canvasGroupComponent;

        [SerializeField] private Text logText;
        [SerializeField] private Image logTypeImage;

        // Objects related to the collapsed count of the debug entry
        [SerializeField] private GameObject logCountParent;
        [SerializeField] private Text logCountText;

        [SerializeField] private RectTransform copyLogButton;
#pragma warning restore 0649

        // Debug entry to show with this log item
        private DebugLogEntry logEntry;
        public DebugLogEntry Entry => logEntry;

        // Index of the entry in the list of entries
        private int entryIndex;
        public int Index => entryIndex;

        private Vector2 logTextOriginalPosition;
        private Vector2 logTextOriginalSize;
        private float copyLogButtonHeight;

        private DebugLogRecycledListView manager;

        public void Initialize(DebugLogRecycledListView manager)
        {
            this.manager = manager;

            logTextOriginalPosition = logText.rectTransform.anchoredPosition;
            logTextOriginalSize = logText.rectTransform.sizeDelta;
            copyLogButtonHeight =
                copyLogButton.anchoredPosition.y +
                copyLogButton.sizeDelta.y +
                2f; // 2f: space between text and button

#if !UNITY_EDITOR && UNITY_WEBGL
			copyLogButton.gameObject.AddComponent<DebugLogItemCopyWebGL>().Initialize( this );
#endif
        }

        public void SetContent(DebugLogEntry logEntry, int entryIndex, bool isExpanded)
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

        // Show the collapsed count of the debug entry
        public void ShowCount()
        {
            logCountText.text = logEntry.count.ToString();

            if (!logCountParent.activeSelf)
            {
                logCountParent.SetActive(true);
            }
        }

        // Hide the collapsed count of the debug entry
        public void HideCount()
        {
            if (logCountParent.activeSelf)
            {
                logCountParent.SetActive(false);
            }
        }

        // This log item is clicked, show the debug entry's stack trace
        public void OnPointerClick(PointerEventData eventData)
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
                        AssetDatabaseManager.LoadAssetAtPath<UnityEditor.MonoScript>(
                            line.Substring(0, lineSeparator)
                        );
                    if (script != null)
                    {
                        AssetDatabaseManager.OpenAsset(script, int.Parse(line.Substring(lineSeparator + 1)));
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

        public void CopyLog()
        {
#if UNITY_EDITOR || !UNITY_WEBGL
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

        public float CalculateExpandedHeight(string content)
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

        // Return a string containing complete information about the debug entry
        [DebuggerStepThrough]
        public override string ToString()
        {
            return logEntry.ToString();
        }
    }
}