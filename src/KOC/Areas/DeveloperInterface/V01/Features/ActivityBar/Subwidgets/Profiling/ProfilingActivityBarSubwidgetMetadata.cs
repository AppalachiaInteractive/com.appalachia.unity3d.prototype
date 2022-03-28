using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Core;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Profiling
{
    public class ProfilingActivityBarSubwidgetMetadata : ActivityBarSubwidgetMetadata<ProfilingActivityBarSubwidget,
        ProfilingActivityBarSubwidgetMetadata>
    {
        public override ActivityBarSection DefaultSection => ActivityBarSection.Top;
        protected override int DefaultPriority => 10;
    }
}
