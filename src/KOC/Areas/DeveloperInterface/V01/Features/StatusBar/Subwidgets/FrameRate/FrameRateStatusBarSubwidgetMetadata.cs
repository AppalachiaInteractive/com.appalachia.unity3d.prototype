using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.FrameRate
{
    public class FrameRateStatusBarSubwidgetMetadata : StatusBarSubwidgetMetadata<FrameRateStatusBarSubwidget,
        FrameRateStatusBarSubwidgetMetadata>
    {
        public override StatusBarSection DefaultSection => StatusBarSection.Left;
        protected override int DefaultPriority => 40;
    }
}
