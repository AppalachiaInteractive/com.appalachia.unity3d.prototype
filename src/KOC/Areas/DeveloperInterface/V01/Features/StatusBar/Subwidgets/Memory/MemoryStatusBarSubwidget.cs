using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Services.Memory;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Sets;
using Appalachia.Utility.Strings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Memory
{
    public class MemoryStatusBarSubwidget : StatusBarSubwidget<MemoryStatusBarSubwidget,
        MemoryStatusBarSubwidgetMetadata, StatusBarSubwidgetComponentSet, StatusBarSubwidgetComponentSetData>
    {
        static MemoryStatusBarSubwidget()
        {
            RegisterDependency<MemoryProfilerService>(i => _memoryProfilerService = i);
        }

        #region Static Fields and Autoproperties

        private static MemoryProfilerService _memoryProfilerService;
        private static Utf8PreparedFormat<string> _tooltipFormat;

        #endregion

        internal MemoryProfilerService MemoryProfilerService => _memoryProfilerService;

        protected override bool RequiresIcon => true;

        public override string GetDevTooltipText()
        {
            using (_PRF_GetDevTooltipText.Auto())
            {
                var tooltipText = $"Reserved RAM: {MemoryProfilerService.ReservedRam}\n" +
                                  $"Allocated RAM: {MemoryProfilerService.AllocatedRam}\n" +
                                  $"Mono RAM: {MemoryProfilerService.MonoRam}\n";

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
                var allocatedRam = MemoryProfilerService.AllocatedRam;
                var result = $"Allocated RAM: {allocatedRam}";

                return result;
            }
        }
    }
}
