using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Models;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Services;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;
using Appalachia.Utility.Strings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.SceneInfo
{
    public class SceneInfoStatusBarSubwidget : StatusBarSubwidget<SceneInfoStatusBarSubwidget,
        SceneInfoStatusBarSubwidgetMetadata>
    {
        #region Constants and Static Readonly

        private const string TOOLTIP_FORMAT_STRING = "<i>Loaded Scenes:</i>\n{0}";

        #endregion

        static SceneInfoStatusBarSubwidget()
        {
            RegisterDependency<DeveloperInfoProviderService>(i => _developerInfoProviderService = i);
        }

        #region Static Fields and Autoproperties

        private static DeveloperInfoProviderService _developerInfoProviderService;
        private static Utf8PreparedFormat<string> _tooltipFormat;

        #endregion

        internal DeveloperInfoProviderService DeveloperInfoProviderService => _developerInfoProviderService;

        public override void OnClicked()
        {
            using (_PRF_OnClicked.Auto())
            {
                throw new NotImplementedException();
            }
        }

        public override string GetDevTooltipText()
        {
            using (_PRF_GetDevTooltipText.Auto())
            {
                var sceneList = _developerInfoProviderService.GetUpdatedText(DeveloperInfoType.SceneList);

                _tooltipFormat ??= new Utf8PreparedFormat<string>(TOOLTIP_FORMAT_STRING);

                return _tooltipFormat.Format(sceneList);
            }
        }

        protected override bool RequiresIcon => true;

        protected override string GetStatusBarText()
        {
            using (_PRF_GetStatusBarText.Auto())
            {
                var activeSceneNameAndCount =
                    _developerInfoProviderService.GetUpdatedText(DeveloperInfoType.ActiveSceneNameAndCount);

                return activeSceneNameAndCount;
            }
        }
    }
}
