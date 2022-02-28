using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed class ActivityBarWidget : DeveloperInterfaceManager_V01.Widget<ActivityBarWidget,
        ActivityBarWidgetMetadata, ActivityBarFeature, ActivityBarFeatureMetadata>
    {
        #region Constants and Static Readonly

        private const string ACTIVITY_BAR_ENTRY_PARENT_NAME = "Activity Bar Entries";

        #endregion

        static ActivityBarWidget()
        {
            When.Widget(_menuBarWidget).IsAvailableThen(menuBarWidget => { _menuBarWidget = menuBarWidget; });
            When.Widget(_statusBarWidget)
                .IsAvailableThen(statusBarWidget => { _statusBarWidget = statusBarWidget; });
        }

        #region Static Fields and Autoproperties

        private static MenuBarWidget _menuBarWidget;
        private static StatusBarWidget _statusBarWidget;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask DelayEnabling()
        {
            await base.DelayEnabling();

            await AppaTask.WaitUntil(() => _menuBarWidget != null);
            await AppaTask.WaitUntil(() => _statusBarWidget != null);
        }

        /// <inheritdoc />
        protected override void EnsureWidgetIsCorrectSize()
        {
            using (_PRF_EnsureWidgetIsCorrectSize.Auto())
            {
                base.EnsureWidgetIsCorrectSize();

                var menuBar = _menuBarWidget;
                var statusBar = _statusBarWidget;

                var anchorMin = RectTransform.anchorMin;
                var anchorMax = RectTransform.anchorMax;

                anchorMin.x = 0.00f;
                anchorMax.x = metadata.width;

                anchorMin.y = statusBar.EffectiveAnchorHeight;
                anchorMax.y = 1.0f - menuBar.EffectiveAnchorHeight;

                UpdateAnchorMin(anchorMin);
                UpdateAnchorMax(anchorMax);
            }
        }

        /// <inheritdoc />
        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                base.UnsubscribeFromAllFunctionalities();

                _menuBarWidget.VisualUpdate.Event -= OnRequiresUpdate;
                _statusBarWidget.VisualUpdate.Event -= OnRequiresUpdate;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                _statusBarWidget.VisualUpdate.Event += OnRequiresUpdate;
                _menuBarWidget.VisualUpdate.Event += OnRequiresUpdate;
            }
        }
    }
}
