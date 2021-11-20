using System;
using Appalachia.Utility.Logging;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Areas.MainMenu.Settings
{
    [Serializable]
    public class SettingsManager : AreaManager<SettingsManager, SettingsMetadata>
    {
        public override ApplicationArea Area => ApplicationArea.MainMenu_Settings;
        public override ApplicationArea ParentArea => ApplicationArea.MainMenu;
        public override bool HasParent => true;

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

        #region Profiling

        private const string _PRF_PFX = nameof(SettingsManager) + ".";

        private static readonly ProfilerMarker _PRF_NewGame = new ProfilerMarker(_PRF_PFX + nameof(NewGame));

        private static readonly ProfilerMarker
            _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(Activate));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(Deactivate));

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        #endregion
    }
}
