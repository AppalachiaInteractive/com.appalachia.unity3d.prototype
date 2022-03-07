using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Strings;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.DebugStatus
{
    public class
        DebuggerStatusBarSubwidget : StatusBarSubwidget<DebuggerStatusBarSubwidget, DebuggerStatusBarSubwidgetMetadata>
    {
        #region Constants and Static Readonly

        private const string TOOLTIP_FORMAT_STRING = "<i>Loaded Scenes:</i>\n{0}";

        #endregion

        #region Static Fields and Autoproperties

        private static Utf8PreparedFormat<string> _tooltipFormat;

        #endregion

        protected override bool RequiresIcon => true;

        public override string GetDevTooltipText()
        {
            using (_PRF_GetDevTooltipText.Auto())
            {
                var tooltipText = "green: game is release mode\n" + "magenta: game is debug mode";
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

        protected override Color GetStatusBarColor()
        {
            if (AppalachiaApplication.IsReleaseBuild)
            {
                return Color.green;
            }

            return Color.magenta;
        }

        protected override string GetStatusBarText()
        {
            using (_PRF_GetStatusBarText.Auto())
            {
                return null;
            }
        }
    }
}
