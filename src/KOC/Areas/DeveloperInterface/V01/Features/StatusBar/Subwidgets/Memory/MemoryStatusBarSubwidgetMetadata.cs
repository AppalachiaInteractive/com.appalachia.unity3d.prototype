using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Memory
{
    public class MemoryStatusBarSubwidgetMetadata : StatusBarSubwidgetMetadata<MemoryStatusBarSubwidget,
        MemoryStatusBarSubwidgetMetadata>
    {
        public override StatusBarSection DefaultSection => StatusBarSection.Left;

        protected override int DefaultPriority => 45;
    }
}
