/*
using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Debugging.Collections;
using Appalachia.Prototype.KOC.Debugging.DeveloperConsole.Commands;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Debugging.DeveloperConsole.Settings
{
    [Serializable]
    public class DeveloperConsoleState : AppalachiaSimpleBase
    {
        #region Fields and Autoproperties

        [NonSerialized] private bool _initialized;

        public bool isCommandPaletteVisible;
        public bool screenDimensionsChanged;
        public bool shouldUpdateRecycledListView;
        public bool snapToBottom;
        public CircularBuffer<string> commandHistory;
        public float commandPalettePreviousWidth;
        public int commandHistoryIndex;
        public int commandInputFieldPrevCaretArgumentIndex;
        public int commandInputFieldPrevCaretPos;
        public int commandInputFieldPrevParamCount;
        public int visibleCommandSuggestionInstances;
        public List<DeveloperConsoleMethodInfo> matchingCommandSuggestions;
        public List<int> commandCaretIndexIncrements;
        public List<Text> commandSuggestionInstances;
        public PointerEventData nullPointerEventData;
        public string commandInputFieldPrevCommand;
        public string commandInputFieldPrevCommandName;
        public string unfinishedCommand;
        public Utf16ValueStringBuilder commandSuggestionsStringBuilder;

        #endregion

        public bool initialized
        {
            get => _initialized;
            private set => _initialized = value;
        }

        public void InitializeState(DeveloperConsoleSettings settings)
        {
            using (_PRF_InitializeState.Auto())
            {
                commandCaretIndexIncrements = new List<int>(8);
                commandHistory = new CircularBuffer<string>(settings.command.commandHistorySize);
                commandHistoryIndex = -1;
                commandInputFieldPrevCaretArgumentIndex = -1;
                commandInputFieldPrevCaretPos = -1;
                commandInputFieldPrevParamCount = -1;
                commandSuggestionInstances = new List<Text>(8);
                commandSuggestionsStringBuilder = new Utf16ValueStringBuilder(false);
                commandSuggestionsStringBuilder = new Utf16ValueStringBuilder(false);
                isCommandPaletteVisible = true;
                matchingCommandSuggestions = new List<DeveloperConsoleMethodInfo>(8);
                nullPointerEventData = new PointerEventData(null);
                screenDimensionsChanged = true;
                snapToBottom = true;

                initialized = true;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DeveloperConsoleState) + ".";

        private static readonly ProfilerMarker _PRF_InitializeState =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeState));

        #endregion
    }
}
*/


