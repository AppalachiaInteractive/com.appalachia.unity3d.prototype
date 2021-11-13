using Appalachia.Utility.Logging;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Areas.HUD
{
    public class HUDManager : AreaManager<HUDManager, HUDMetadata>
    {
        #region Profiling

        private const string _PRF_PFX = nameof(HUDManager) + ".";

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        private static readonly ProfilerMarker
            _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(Activate));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(Deactivate));

        #endregion

        public override ApplicationArea Area => ApplicationArea.HUD;

        public override void Activate()
        {
            using (_PRF_Activate.Auto())
            {
                AppaLog.Context.Area.Info(nameof(Activate));
            }
        }

        public override void Deactivate()
        {
            using (_PRF_Deactivate.Auto())
            {
                AppaLog.Context.Area.Info(nameof(Deactivate));
            }
        }

        protected override void ResetArea()
        {
            using (_PRF_ResetArea.Auto())
            {
                AppaLog.Context.Area.Info(nameof(ResetArea));
            }
        }
    }
}
