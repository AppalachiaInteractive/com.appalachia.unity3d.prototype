using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.DebugStatus
{
    public class DebuggerStatusBarSubwidgetMetadata : StatusBarSubwidgetMetadata<DebuggerStatusBarSubwidget,
        DebuggerStatusBarSubwidgetMetadata>
    {
        #region Fields and Autoproperties

        private StatusBarSection _defaultSection;

        #endregion

        public override StatusBarSection DefaultSection => StatusBarSection.Right;
        protected override int DefaultPriority => 0;
    }
}
