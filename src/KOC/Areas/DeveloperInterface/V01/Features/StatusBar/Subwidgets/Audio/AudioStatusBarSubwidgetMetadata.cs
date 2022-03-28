using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Audio
{
    public class AudioStatusBarSubwidgetMetadata : StatusBarSubwidgetMetadata<AudioStatusBarSubwidget,
        AudioStatusBarSubwidgetMetadata>
    {
        public override StatusBarSection DefaultSection => StatusBarSection.Left;
        protected override int DefaultPriority => 50;
    }
}
