using Appalachia.Utility.Logging;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Areas.PauseMenu
{
    public class PauseMenuManager : AreaManager<PauseMenuManager, PauseMenuMetadata>
    {
        public override ApplicationArea Area => ApplicationArea.PauseMenu;
        public override ApplicationArea ParentArea => ApplicationArea.None;
        

        protected override void OnActivation()
        {
            using (_PRF_Activate.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnActivation));
            }
        }

        protected override void OnDeactivation()
        {
            using (_PRF_Deactivate.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnDeactivation));
            }
        }

        protected override void ResetArea()
        {
            using (_PRF_ResetArea.Auto())
            {
                AppaLog.Context.Area.Info(nameof(ResetArea));
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(PauseMenuManager) + ".";

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        private static readonly ProfilerMarker
            _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        #endregion
    }
}
