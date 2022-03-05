using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Services.FPS;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;
using Appalachia.Utility.Strings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.FrameRate
{
    public class FrameRateStatusBarSubwidget : StatusBarSubwidget<FrameRateStatusBarSubwidget,
        FrameRateStatusBarSubwidgetMetadata>
    {
        #region Constants and Static Readonly

        private const string TOOLTIP_FORMAT_STRING = "<i>Loaded Scenes:</i>\n{0}";

        #endregion

        static FrameRateStatusBarSubwidget()
        {
            RegisterDependency<FPSProfilerService>(i => _fpsProfilerService = i);
        }

        #region Static Fields and Autoproperties

        private static FPSProfilerService _fpsProfilerService;
        private static Utf8PreparedFormat<string> _tooltipFormat;

        #endregion

        internal FPSProfilerService FPSProfilerService => _fpsProfilerService;

        public override string GetDevTooltipText()
        {
            using (_PRF_GetDevTooltipText.Auto())
            {
                var tooltipText = $"Current FPS: {FPSProfilerService.CurrentFPS}\n" +
                                  $"Average FPS: {FPSProfilerService.AverageFPS}\n" +
                                  $"1% FPS: {FPSProfilerService.OnePercentFPS}\n" +
                                  $"0.1% FPS: {FPSProfilerService.Zero1PercentFPS}";
                return tooltipText;
            }
        }

        public override void OnClicked()
        {
            using (_PRF_OnClicked.Auto())
            {
                throw new NotImplementedException();
            }
        }

        protected override bool RequiresIcon => true;

        protected override string GetStatusBarText()
        {
            using (_PRF_GetStatusBarText.Auto())
            {
                var averageFramesPerSecond = FPSProfilerService.AverageFPS;
                var result = $"Avg FPS: {averageFramesPerSecond}";

                return result;
            }
        }
    }
}
