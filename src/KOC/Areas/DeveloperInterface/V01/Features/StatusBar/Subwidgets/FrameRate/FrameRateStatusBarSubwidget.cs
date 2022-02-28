using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Services.Rendering;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Sets;
using Appalachia.Utility.Strings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.FrameRate
{
    public class FrameRateStatusBarSubwidget : StatusBarSubwidget<FrameRateStatusBarSubwidget,
        FrameRateStatusBarSubwidgetMetadata, StatusBarSubwidgetComponentSet, StatusBarSubwidgetComponentSetData>
    {
        #region Constants and Static Readonly

        private const string TOOLTIP_FORMAT_STRING = "<i>Loaded Scenes:</i>\n{0}";

        #endregion

        static FrameRateStatusBarSubwidget()
        {
            RegisterDependency<RenderingProfilerService>(i => _renderingProfilerService = i);
        }

        #region Static Fields and Autoproperties

        private static RenderingProfilerService _renderingProfilerService;
        private static Utf8PreparedFormat<string> _tooltipFormat;

        #endregion

        internal RenderingProfilerService RenderingProfilerService => _renderingProfilerService;

        public override string GetDevTooltipText()
        {
            using (_PRF_GetDevTooltipText.Auto())
            {
                var tooltipText = $"Current FPS: {RenderingProfilerService.CurrentFPS}\n" +
                                  $"Average FPS: {RenderingProfilerService.AverageFPS}\n" +
                                  $"1% FPS: {RenderingProfilerService.OnePercentFPS}\n" +
                                  $"0.1% FPS: {RenderingProfilerService.Zero1PercentFps}";
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
                var averageFramesPerSecond = RenderingProfilerService.AverageFPS;
                var result = $"Average FPS: {averageFramesPerSecond}";

                return result;
            }
        }
    }
}
