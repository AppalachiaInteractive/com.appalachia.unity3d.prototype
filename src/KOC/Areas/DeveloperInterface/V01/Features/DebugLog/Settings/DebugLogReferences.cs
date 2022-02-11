using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Collections;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Components;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Settings
{
    [Serializable]
    public class DebugLogReferences : AppalachiaSimpleBase
    {
        #region Fields and Autoproperties

        [NonSerialized] public bool initialized;
        [SerializeField] public Button clearButton;
        [SerializeField] public Button hideButton;
        [SerializeField] public CanvasGroup logWindowCanvasGroup;
        [SerializeField] public DebugLogPopup popupManager;
        [SerializeField] public DebugLogRecycledListView recycledListView;
        [SerializeField] public GameObject snapToBottomButton;
        [SerializeField] public Image collapseButton;
        [SerializeField] public Image filterErrorButton;
        [SerializeField] public Image filterInfoButton;
        [SerializeField] public Image filterWarningButton;
        [SerializeField] public Image resizeButton;
        [SerializeField] public RectTransform canvasTR;
        [SerializeField] public RectTransform commandSuggestionsContainer;
        [SerializeField] public RectTransform logItemsContainer;
        [SerializeField] public RectTransform logItemsScrollRectTR;
        [SerializeField] public RectTransform logWindowTR;
        [SerializeField] public RectTransform searchbar;
        [SerializeField] public RectTransform searchbarSlotBottom;
        [SerializeField] public RectTransform searchbarSlotTop;
        [SerializeField] public ScrollRect logItemsScrollRect;
        [SerializeField] public Text errorEntryCountText;
        [SerializeField] public Text infoEntryCountText;
        [SerializeField] public Text warningEntryCountText;
        [SerializeField] public Vector2 logItemsScrollRectOriginalSize;

        #endregion

        public void InitalizeReferences(
            Transform transform,
            DebugLogManager manager,
            DebugLogSettings settings,
            DebugLogState state)
        {
            using (_PRF_InitializeReferences.Auto())
            {
                canvasTR = (RectTransform)transform;
                logItemsScrollRectTR = (RectTransform)logItemsScrollRect.transform;
                logItemsScrollRectOriginalSize = logItemsScrollRectTR.sizeDelta;

                // Initially, all log types are visible
                filterInfoButton.color = settings.visuals.filterButtonsSelectedColor;
                filterWarningButton.color = settings.visuals.filterButtonsSelectedColor;
                filterErrorButton.color = settings.visuals.filterButtonsSelectedColor;

                resizeButton.sprite = settings.window.enableHorizontalResizing
                    ? settings.visuals.resizeIconAllDirections
                    : settings.visuals.resizeIconVerticalOnly;

                var prefab = settings.visuals.logItemPrefab;
                var prefabComponent = prefab.GetComponent<DebugLogItem>();

                recycledListView.Initialize(prefabComponent.RectTransform.sizeDelta.y);

                recycledListView.UpdateItemsInTheList(true);

                if (!settings.window.resizeFromRight)
                {
                    var resizeButtonTR = (RectTransform)resizeButton
                                                       .GetComponentInParent<DebugLogResizeListener>()
                                                       .transform;
                    resizeButtonTR.anchorMin = new Vector2(0f, resizeButtonTR.anchorMin.y);
                    resizeButtonTR.anchorMax = new Vector2(0f, resizeButtonTR.anchorMax.y);
                    resizeButtonTR.pivot = new Vector2(0f,     resizeButtonTR.pivot.y);
                }

                if (settings.general.enableSearchbar)
                {
                    searchbar.GetComponent<InputField>()
                             .onValueChanged.AddListener(manager.SearchTermChanged);
                }
                else
                {
                    searchbar = null;
                    searchbarSlotTop.gameObject.SetActive(false);
                    searchbarSlotBottom.gameObject.SetActive(false);
                }

                if (commandSuggestionsContainer.gameObject.activeSelf)
                {
                    commandSuggestionsContainer.gameObject.SetActive(false);
                }

                hideButton.onClick.AddListener(manager.HideLogWindow);
                clearButton.onClick.AddListener(manager.ClearLogs);
                collapseButton.GetComponent<Button>().onClick.AddListener(manager.CollapseButtonPressed);
                filterInfoButton.GetComponent<Button>().onClick.AddListener(manager.FilterLogButtonPressed);
                filterWarningButton.GetComponent<Button>()
                                   .onClick.AddListener(manager.FilterWarningButtonPressed);
                filterErrorButton.GetComponent<Button>()
                                 .onClick.AddListener(manager.FilterErrorButtonPressed);
                snapToBottomButton.GetComponent<Button>()
                                  .onClick.AddListener(() => manager.SetSnapToBottom(true));

                // On new Input System, scroll sensitivity is much higher than legacy Input system
                logItemsScrollRect.scrollSensitivity *= 0.25f;

                initialized = true;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DebugLogReferences) + ".";

        private static readonly ProfilerMarker _PRF_InitializeReferences =
            new ProfilerMarker(_PRF_PFX + nameof(InitalizeReferences));

        #endregion
    }
}
