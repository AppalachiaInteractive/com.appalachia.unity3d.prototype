using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Panel.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed class PanelWidget : DeveloperInterfaceManager_V01.Widget<PanelWidget, PanelWidgetMetadata,
        PanelFeature, PanelFeatureMetadata>
    {
        static PanelWidget()
        {
            When.Widget(_sideBarWidget).IsAvailableThen(sideBarWidget => { _sideBarWidget = sideBarWidget; });
            When.Widget(_activityBarWidget)
                .IsAvailableThen(activityBarWidget => { _activityBarWidget = activityBarWidget; });
            When.Widget(_statusBarWidget)
                .IsAvailableThen(statusBarWidget => { _statusBarWidget = statusBarWidget; });
        }

        #region Static Fields and Autoproperties

        private static ActivityBarWidget _activityBarWidget;
        private static SideBarWidget _sideBarWidget;
        private static StatusBarWidget _statusBarWidget;

        #endregion

        protected override async AppaTask DelayEnabling()
        {
            await base.DelayEnabling();

            await AppaTask.WaitUntil(() => _sideBarWidget != null);
            await AppaTask.WaitUntil(() => _activityBarWidget != null);
            await AppaTask.WaitUntil(() => _statusBarWidget != null);
        }

        protected override void EnsureWidgetIsCorrectSize()
        {
            using (_PRF_EnsureWidgetIsCorrectSize.Auto())
            {
                base.EnsureWidgetIsCorrectSize();

                var sideBar = _sideBarWidget;
                var activityBar = _activityBarWidget;
                var statusBar = _statusBarWidget;

                var anchorMin = RectTransform.anchorMin;
                var anchorMax = RectTransform.anchorMax;

                anchorMin.x = activityBar.EffectiveAnchorWidth + sideBar.EffectiveAnchorWidth;

                anchorMax.x = 1.0f;

                anchorMin.y = statusBar.EffectiveAnchorHeight;
                anchorMax.y = anchorMin.y + metadata.height;

                UpdateAnchorMin(anchorMin);
                UpdateAnchorMax(anchorMax);
            }
        }

        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                base.UnsubscribeFromAllFunctionalities();

                _sideBarWidget.VisuallyChanged.Event -= OnDependencyChanged;
                _activityBarWidget.VisuallyChanged.Event -= OnDependencyChanged;
                _statusBarWidget.VisuallyChanged.Event -= OnDependencyChanged;
            }
        }

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                _sideBarWidget.VisuallyChanged.Event += OnDependencyChanged;
                _activityBarWidget.VisuallyChanged.Event += OnDependencyChanged;
                _statusBarWidget.VisuallyChanged.Event += OnDependencyChanged;
            }
        }
    }
}
