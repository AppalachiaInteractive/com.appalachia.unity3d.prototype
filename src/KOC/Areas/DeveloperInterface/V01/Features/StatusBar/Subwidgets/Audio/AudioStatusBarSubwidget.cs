using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Services.Audio;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;
using Appalachia.Utility.Strings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Audio
{
    public class AudioStatusBarSubwidget : StatusBarSubwidget<AudioStatusBarSubwidget, AudioStatusBarSubwidgetMetadata>
    {
        #region Constants and Static Readonly

        private const string TOOLTIP_FORMAT_STRING = "<i>Loaded Scenes:</i>\n{0}";

        #endregion

        static AudioStatusBarSubwidget()
        {
            RegisterDependency<AudioProfilerService>(i => _audioProfilerService = i);
        }

        #region Static Fields and Autoproperties

        private static AudioProfilerService _audioProfilerService;
        private static Utf8PreparedFormat<string> _tooltipFormat;

        #endregion

        internal AudioProfilerService AudioProfilerService => _audioProfilerService;

        protected override bool RequiresIcon => true;

        public override string GetDevTooltipText()
        {
            using (_PRF_GetDevTooltipText.Auto())
            {
                var tooltipText = $"Current Maximum Decibals: {AudioProfilerService.MaxDB}";
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

        protected override string GetStatusBarText()
        {
            using (_PRF_GetStatusBarText.Auto())
            {
                var maximumDecibals = AudioProfilerService.MaxDB;
                var result = $"Max dB: {maximumDecibals}";

                return result;
            }
        }
    }
}
