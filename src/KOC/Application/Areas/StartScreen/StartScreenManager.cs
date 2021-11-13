using Appalachia.Utility.Logging;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Areas.StartScreen
{
    public class StartScreenManager : AreaManager<StartScreenManager, StartScreenMetadata>
    {
        #region Profiling

        private const string _PRF_PFX = nameof(StartScreenManager) + ".";

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        private static readonly ProfilerMarker
            _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(Activate));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(Deactivate));

        private static readonly ProfilerMarker
            _PRF_Continue = new ProfilerMarker(_PRF_PFX + nameof(Continue));

        #endregion

        public override ApplicationArea Area => ApplicationArea.StartScreen;

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

        public void Continue()
        {
            using (_PRF_Continue.Auto())
            {
                AppaLog.Context.Area.Info(nameof(Continue));
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
