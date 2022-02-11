using System;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.GameArea.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Model;
using Appalachia.Utility.Async;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.GameArea
{
    [CallStaticConstructorInEditor]
    public class GameAreaFeature : DeveloperInterfaceManager_V01.Feature<GameAreaFeature,
                                       GameAreaFeatureMetadata>,
                                   IStatusBarEntryProvider
    {
        static GameAreaFeature()
        {
            FunctionalitySet.RegisterWidget<GameAreaWidget>(_dependencyTracker, i => _gameAreaWidget = i);
        }

        #region Static Fields and Autoproperties

        private static GameAreaWidget _gameAreaWidget;

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
                await ShowFeature();
            }
        }

        protected override async AppaTask BeforeFirstEnable()
        {
            using (_PRF_BeforeFirstEnable.Auto())
            {
                await AppaTask.CompletedTask;
            }
        }

        protected override async AppaTask OnHide()
        {
            using (_PRF_OnHide.Auto())
            {
                _gameAreaWidget.Hide();
                await AppaTask.CompletedTask;
            }
        }

        protected override async AppaTask OnShow()
        {
            using (_PRF_OnShow.Auto())
            {
                _gameAreaWidget.Show();
                await AppaTask.CompletedTask;
            }
        }

        #region IStatusBarEntryProvider Members

        public StatusBarEntry[] GetStatusBarEntries()
        {
            using (_PRF_GetStatusBarEntries.Auto())
            {
                return Array.Empty<StatusBarEntry>();
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetStatusBarEntries =
            new ProfilerMarker(_PRF_PFX + nameof(GetStatusBarEntries));

        #endregion
    }
}
