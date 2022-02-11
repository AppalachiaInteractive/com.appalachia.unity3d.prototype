using System;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Model;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets;
using Appalachia.Utility.Async;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar
{
    [CallStaticConstructorInEditor]
    public class SideBarFeature : DeveloperInterfaceManager_V01.Feature<SideBarFeature,
                                      SideBarFeatureMetadata>,
                                  IActivityBarEntryProvider
    {
        static SideBarFeature()
        {
            FunctionalitySet.RegisterWidget<SideBarWidget>(_dependencyTracker, i => _sideBarWidget = i);
        }

        #region Static Fields and Autoproperties

        private static SideBarWidget _sideBarWidget;

        #endregion

        protected override async AppaTask BeforeDisable()
        {
            using (_PRF_BeforeDisable.Auto())
            {
                await HideFeature();
            }
        }

        protected override async AppaTask BeforeEnable()
        {
            using (_PRF_BeforeEnable.Auto())
            {
                await AppaTask.CompletedTask;
            }
        }

        protected override async AppaTask BeforeFirstEnable()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask OnHide()
        {
            using (_PRF_OnHide.Auto())
            {
                _sideBarWidget.Hide();
                await AppaTask.CompletedTask;
            }
        }

        protected override async AppaTask OnShow()
        {
            using (_PRF_OnShow.Auto())
            {
                _sideBarWidget.Show();
                await AppaTask.CompletedTask;
            }
        }

        #region IActivityBarEntryProvider Members

        public ActivityBarEntry[] GetActivityBarEntries()
        {
            using (_PRF_GetActivityBarEntries.Auto())
            {
                return Array.Empty<ActivityBarEntry>();
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetActivityBarEntries =
            new ProfilerMarker(_PRF_PFX + nameof(GetActivityBarEntries));

        #endregion
    }
}
