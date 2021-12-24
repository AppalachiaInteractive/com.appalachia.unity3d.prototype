using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Debugging.DebugConsole.Settings
{
    [DoNotReorderFields]
    public class DebugLogManagerSettings : SingletonAppalachiaObject<DebugLogManagerSettings>
    {
        #region Fields and Autoproperties

        [FoldoutGroup("General"), HideLabel]
        public GeneralSettings general;

        [FoldoutGroup("Window"), HideLabel]
        public WindowSettings window;

        [FoldoutGroup("Popup"), HideLabel]
        public PopupSettings popup;

        [FoldoutGroup("Visuals"), HideLabel]
        public VisualSettings visuals;

        [FoldoutGroup("Command"), HideLabel]
        public CommandSettings command;

        [FoldoutGroup("Mobile"), HideLabel]
        public MobileSettings mobile;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await initializer.Do(this, nameof(VisualSettings),  () => visuals.Initialize());
            await initializer.Do(this, nameof(GeneralSettings), () => general.Initialize());
            await initializer.Do(this, nameof(WindowSettings),  () => window.Initialize());
            await initializer.Do(this, nameof(PopupSettings),   () => popup.Initialize());
            await initializer.Do(this, nameof(CommandSettings), () => command.Initialize());
            await initializer.Do(this, nameof(MobileSettings),  () => mobile.Initialize());
        }

        #region Nested type: CommandSettings

        [Serializable]
        public struct CommandSettings
        {
            #region Fields and Autoproperties

#if UNITY_EDITOR || UNITY_STANDALONE

            [PropertyTooltip(
                "If enabled, on standalone platforms, command input field will automatically be focused (start receiving keyboard input) after opening the console window"
            )]
            [ToggleLeft]
            public bool autoFocusOnCommandInputField;
#endif

            [PropertyTooltip(
                "If enabled, the command input field at the bottom of the console window will automatically be cleared after entering a command"
            )]
            [ToggleLeft]
            public bool clearCommandAfterExecution;

            [PropertyTooltip(
                "If enabled, while typing a command, all of the matching commands' signatures will be displayed in a popup"
            )]
            [ToggleLeft]
            public bool showCommandSuggestions;

            [PropertyTooltip(
                "Console keeps track of the previously entered commands. This value determines the capacity of the command history (you can scroll through the history via up and down arrow keys while the command input field is focused)"
            )]
            [PropertyRange(1, 50)]
            public int commandHistorySize;

            #endregion

            public void Initialize()
            {
                clearCommandAfterExecution = true;
                commandHistorySize = 15;
                showCommandSuggestions = true;
#if UNITY_EDITOR || UNITY_STANDALONE
                autoFocusOnCommandInputField = true;
#endif
            }
        }

        #endregion

        #region Nested type: GeneralSettings

        [Serializable]
        public struct GeneralSettings
        {
            #region Fields and Autoproperties

            [PropertyTooltip("If enabled, the console window will have a searchbar")]
            [ToggleLeft]
            public bool enableSearchbar;

            [PropertyTooltip("If enabled, console window will initially be invisible")]
            [ToggleLeft]
            public bool startMinimized;

            [PropertyTooltip(
                "Width of the canvas determines whether the searchbar will be located inside the menu bar or underneath the menu bar. This way, the menu bar doesn't get too crowded on narrow screens. This value determines the minimum width of the canvas for the searchbar to appear inside the menu bar"
            )]
            [PropertyRange(200f, 500f)]
            public float topSearchbarMinWidth;

            [PropertyTooltip(
                "If a log is longer than this limit, it will be truncated. This helps avoid reaching Unity's 65000 vertex limit for UI canvases"
            )]
            [PropertyRange(1000, 10000)]
            public int maxLogLength;

            #endregion

            public void Initialize()
            {
                enableSearchbar = true;
                topSearchbarMinWidth = 360f;
                maxLogLength = 10000;
            }
        }

        #endregion

        #region Nested type: MobileSettings

        [Serializable]
        public struct MobileSettings
        {
            #region Fields and Autoproperties

            [PropertyTooltip(
                "If enabled, on Android and iOS devices with notch screens, the console window will be repositioned so that the cutout(s) don't obscure it"
            )]
            [ToggleLeft]
            public bool avoidScreenCutout;

            [PropertyTooltip(
                "If enabled, on Android platform, logcat entries of the application will also be logged to the console with the prefix \"LOGCAT: \". This may come in handy especially if you want to access the native logs of your Android plugins (like Admob)"
            )]
            [ToggleLeft]
            public bool receiveLogcatLogsInAndroid;

            [PropertyTooltip(
                "Native logs will be filtered using these arguments. If left blank, all native logs of the application will be logged to the console. But if you want to e.g. see Admob's logs only, you can enter \"-s Ads\" (without quotes) here"
            )]
            public string logcatArguments;

            #endregion

            public void Initialize()
            {
                avoidScreenCutout = true;
            }
        }

        #endregion

        #region Nested type: PopupSettings

        [Serializable]
        public struct PopupSettings
        {
            #region Fields and Autoproperties

            [PropertyTooltip("If disabled, no popup will be shown when the console window is hidden")]
            public bool enablePopup;

            [PropertyTooltip("If enabled, console will be initialized as a popup")]
            public bool startInPopupMode;

            public Color alertColorError;

            public Color alertColorInfo;

            public Color alertColorNormal;

            public Color alertColorWarning;

            #endregion

            public void Initialize()
            {
                enablePopup = true;
                startInPopupMode = true;
            }
        }

        #endregion

        #region Nested type: VisualSettings

        [Serializable]
        public struct VisualSettings
        {
            #region Fields and Autoproperties

            public Color collapseButtonNormalColor;
            public Color collapseButtonSelectedColor;
            public Color filterButtonsNormalColor;
            public Color filterButtonsSelectedColor;
            public DebugLogItem logItemPrefab;
            public Sprite errorLog;
            public Sprite infoLog;
            public Sprite resizeIconAllDirections;
            public Sprite resizeIconVerticalOnly;
            public Sprite warningLog;
            public string commandSuggestionHighlightEnd;
            public string commandSuggestionHighlightStart;
            public Text commandSuggestionPrefab;

            #endregion

            public void Initialize()
            {
                commandSuggestionHighlightEnd = "</color>";
                commandSuggestionHighlightStart = "<color=orange>";
            }
        }

        #endregion

        #region Nested type: WindowSettings

        [Serializable]
        public struct WindowSettings
        {
            #region Fields and Autoproperties

            [PropertyTooltip("If enabled, console window can be resized horizontally, as well")]
            [ToggleLeft]
            public bool enableHorizontalResizing;

            [PropertyTooltip(
                "If enabled, console window's resize button will be located at bottom-right corner. Otherwise, it will be located at bottom-left corner"
            )]
            [ToggleLeft]
            public bool resizeFromRight;

            [PropertyTooltip("Minimum height of the console window")]
            [PropertyRange(200f, 800f)]
            public float minimumHeight;

            [PropertyTooltip("Minimum width of the console window")]
            [PropertyRange(100f, 800f)]
            public float minimumWidth;

            [PropertyRange(0.1f, 1.0f)]
            public float alpha;

            #endregion

            public void Initialize()
            {
                minimumWidth = 240f;
                minimumHeight = 200f;
                resizeFromRight = true;
            }
        }

        #endregion
    }
}

/*
private bool autoFocusOnCommandInputField;
private bool clearCommandAfterExecution;
private bool showCommandSuggestions;
private int commandHistorySize;
private bool enableSearchbar;
private bool startMinimized;
private float topSearchbarMinWidth;
private int maxLogLength;
private bool avoidScreenCutout;
private bool receiveLogcatLogsInAndroid;
private string logcatArguments;
private bool enablePopup;
private bool startInPopupMode;
private Color collapseButtonNormalColor;
private Color collapseButtonSelectedColor;
private Color filterButtonsNormalColor;
private Color filterButtonsSelectedColor;
private DebugLogItem logItemPrefab;
private Sprite errorLog;
private Sprite infoLog;
private Sprite resizeIconAllDirections;
private Sprite resizeIconVerticalOnly;
private Sprite warningLog;
private string commandSuggestionHighlightEnd;
private string commandSuggestionHighlightStart;
private Text commandSuggestionPrefab;
private bool enableHorizontalResizing;
private bool resizeFromRight;
private float minimumHeight;
private float minimumWidth;
*/
