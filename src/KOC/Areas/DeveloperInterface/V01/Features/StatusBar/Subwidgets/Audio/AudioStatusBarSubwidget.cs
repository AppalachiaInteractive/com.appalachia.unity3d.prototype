using System;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Services.Audio;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Profiling;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;
using Appalachia.Utility.Strings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Audio
{
    [CallStaticConstructorInEditor]
    public class AudioStatusBarSubwidget : StatusBarSubwidget<AudioStatusBarSubwidget, AudioStatusBarSubwidgetMetadata>
    {
        #region Constants and Static Readonly

        private const string TOOLTIP_FORMAT_STRING = "<i>Loaded Scenes:</i>\n{0}";

        #endregion

        static AudioStatusBarSubwidget()
        {
            RegisterDependency<AudioProfilerService>(i => _audioProfilerService = i);
            RegisterDependency<ProfilingSideBarSubwidget>(i => _profilingSideBarSubwidget = i);
        }

        #region Static Fields and Autoproperties

        private static AudioProfilerService _audioProfilerService;
        private static ProfilingSideBarSubwidget _profilingSideBarSubwidget;
        private static Utf8PreparedFormat<string> _tooltipFormat;

        #endregion

        internal AudioProfilerService AudioProfilerService => _audioProfilerService;

        public override string GetStatusBarText()
        {
            using (_PRF_GetStatusBarText.Auto())
            {
                var maximumDecibals = AudioProfilerService.MaxDB;
                var result = $"Max dB: {maximumDecibals}";

                return result;
            }
        }

        public override string GetTooltipContent()
        {
            using (_PRF_GetTooltipContent.Auto())
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
    }
}
