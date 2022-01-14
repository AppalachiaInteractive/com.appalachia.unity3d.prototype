#if UNITY_EDITOR
using Appalachia.Utility.Execution;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog
{
    public partial class DebugLogManager
    {
        #region Event Functions

        private void OnValidate()
        {
            using (_PRF_OnValidate.Auto())
            {
                if (AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    if ((references == null) ||
                        (references.resizeButton == null) ||
                        (_debugLogSettings == null))
                    {
                        return;
                    }

                    references.resizeButton.sprite = _debugLogSettings.window.enableHorizontalResizing
                        ? _debugLogSettings.visuals.resizeIconAllDirections
                        : _debugLogSettings.visuals.resizeIconVerticalOnly;
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnValidate = new(_PRF_PFX + nameof(OnValidate));

        #endregion
    }
}

#endif
