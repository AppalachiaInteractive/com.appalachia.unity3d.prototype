using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed class SideBarWidget : DeveloperInterfaceManager_V01.Widget<SideBarWidget,
        SideBarWidgetMetadata, SideBarFeature, SideBarFeatureMetadata>
    {
        static SideBarWidget()
        {
            When.Widget<ActivityBarWidget>()
                .IsAvailableThen(activityBarWidget => { _activityBarWidget = activityBarWidget; });
            When.Widget<MenuBarWidget>()
                .IsAvailableThen(menuBarWidget => { _menuBarWidget = menuBarWidget; });
            When.Widget<StatusBarWidget>()
                .IsAvailableThen(statusBarWidget => { _statusBarWidget = statusBarWidget; });
        }

        #region Static Fields and Autoproperties

        private static ActivityBarWidget _activityBarWidget;
        private static MenuBarWidget _menuBarWidget;
        private static StatusBarWidget _statusBarWidget;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask DelayEnabling()
        {
            await base.DelayEnabling();

            await AppaTask.WaitUntil(() => _menuBarWidget != null);
            await AppaTask.WaitUntil(() => _activityBarWidget != null);
            await AppaTask.WaitUntil(() => _statusBarWidget != null);
        }

        /// <inheritdoc />
        protected override void EnsureWidgetIsCorrectSize()
        {
            using (_PRF_EnsureWidgetIsCorrectSize.Auto())
            {
                base.EnsureWidgetIsCorrectSize();

                var activityBar = _activityBarWidget;
                var menuBar = _menuBarWidget;
                var statusBar = _statusBarWidget;

                var anchorMin = RectTransform.anchorMin;
                var anchorMax = RectTransform.anchorMax;

                anchorMin.x = activityBar.EffectiveAnchorWidth;
                anchorMax.x = anchorMin.x + metadata.width;

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

                if (_activityBarWidget)
                {
                    _activityBarWidget.VisualUpdate.Event -= OnRequiresUpdate;
                }

                if (_menuBarWidget)
                {
                    _menuBarWidget.VisualUpdate.Event -= OnRequiresUpdate;
                }

                if (_statusBarWidget)
                {
                    _statusBarWidget.VisualUpdate.Event -= OnRequiresUpdate;
                }
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenDisabled()
        {
            await base.WhenDisabled();

            using (_PRF_WhenDisabled.Auto())
            {
                _menuBarWidget.VisualUpdate.Event += OnRequiresUpdate;
                _activityBarWidget.VisualUpdate.Event += OnRequiresUpdate;
                _statusBarWidget.VisualUpdate.Event += OnRequiresUpdate;
            }
        }
    }
}
