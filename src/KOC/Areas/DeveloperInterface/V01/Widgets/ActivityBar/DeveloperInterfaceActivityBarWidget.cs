using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.ActivityBar.Model;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.MenuBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.StatusBar;
using Appalachia.UI.Controls.Sets.Button;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.ActivityBar
{
    [CallStaticConstructorInEditor]
    public sealed class DeveloperInterfaceActivityBarWidget : DeveloperInterfaceWidget<
        DeveloperInterfaceActivityBarWidget, DeveloperInterfaceActivityBarWidgetMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        #region Constants and Static Readonly

        private const string BUTTON_PARENT_NAME = "Activity Buttons";

        #endregion

        static DeveloperInterfaceActivityBarWidget()
        {
            RegisterDependency<DeveloperInterfaceMenuBarWidget>(i => _developerInterfaceMenuBarWidget = i);
            RegisterDependency<DeveloperInterfaceStatusBarWidget>(
                i => _developerInterfaceStatusBarWidget = i
            );
        }

        #region Static Fields and Autoproperties

        private static DeveloperInterfaceMenuBarWidget _developerInterfaceMenuBarWidget;
        private static DeveloperInterfaceStatusBarWidget _developerInterfaceStatusBarWidget;

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] private List<ActivityBarEntry> _entries;
        [NonSerialized] private List<ButtonComponentSet> _buttons;
        [SerializeField] private GameObject _buttonParent;

        #endregion

        public IReadOnlyList<ActivityBarEntry> Entries
        {
            get
            {
                _entries ??= new List<ActivityBarEntry>();
                return _entries;
            }
        }

        public IReadOnlyList<ButtonComponentSet> Buttons
        {
            get
            {
                _buttons ??= new List<ButtonComponentSet>();
                return _buttons;
            }
        }

        public void RegisterActivity(ActivityBarEntry activity)
        {
            using (_PRF_RegisterActivity.Auto())
            {
                _entries.Add(activity);
                ApplyMetadata();
            }
        }

        protected override void OnApplyMetadataInternal()
        {
            using (_PRF_OnApplyMetadataInternal.Auto())
            {
                _entries ??= new List<ActivityBarEntry>();
                _buttons ??= new List<ButtonComponentSet>();

                if (_buttonParent == null)
                {
                    gameObject.GetOrAddChild(ref _buttonParent, BUTTON_PARENT_NAME, true);
                }

                for (var index = _entries.Count - 1; index >= 0; index--)
                {
                    var entry = _entries[index];

                    if (entry == null)
                    {
                        _entries.RemoveAt(index);
                    }
                }

                while (_buttons.Count > _entries.Count)
                {
                    var lastIndex = _buttons.Count - 1;

                    var last = _buttons[lastIndex];
                    last.GameObject.DestroySafely();

                    _buttons.RemoveAt(lastIndex);
                }

                while (_buttons.Count < _entries.Count)
                {
                    var entry = _entries[_buttons.Count];

                    ButtonComponentSet newButtonComponentSet = null;

                    metadata.buttonStyle.PrepareAndConfigure(
                        ref newButtonComponentSet,
                        _buttonParent,
                        entry.name
                    );

                    _buttons.Add(newButtonComponentSet);
                }
            }
        }

        protected override void SubscribeToAllFunctionalties()
        {
            using (_PRF_SubscribeToAllFunctionalties.Auto())
            {
                _developerInterfaceMenuBarWidget.VisuallyChanged += ApplyMetadata;
                _developerInterfaceStatusBarWidget.VisuallyChanged += ApplyMetadata;
            }
        }

        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                _developerInterfaceMenuBarWidget.VisuallyChanged -= ApplyMetadata;
                _developerInterfaceStatusBarWidget.VisuallyChanged -= ApplyMetadata;
            }
        }

        protected override void UpdateSizeInternal()
        {
            using (_PRF_UpdateSizeInternal.Auto())
            {
                var menuBar = _developerInterfaceMenuBarWidget;
                var statusBar = _developerInterfaceStatusBarWidget;

                var anchorMin = rectTransform.anchorMin;
                var anchorMax = rectTransform.anchorMax;

                anchorMin.x = 0.00f;
                anchorMax.x = metadata.width;

                anchorMin.y = statusBar.EffectiveAnchorHeight;
                anchorMax.y = 1.0f - menuBar.EffectiveAnchorHeight;

                UpdateAnchorMin(anchorMin);
                UpdateAnchorMax(anchorMax);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterActivity =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterActivity));

        #endregion
    }
}
