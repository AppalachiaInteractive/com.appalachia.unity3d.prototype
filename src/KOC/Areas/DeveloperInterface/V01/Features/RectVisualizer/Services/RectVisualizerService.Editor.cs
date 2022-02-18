using Appalachia.CI.Constants;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Services
{
    public partial class RectVisualizerService
    {
        #region Constants and Static Readonly

        private const string GROUP_NAME_BUTTON_TARGET = nameof(DiscoverTargetsButton);

        #endregion

        [ButtonGroup(GROUP_NAME_BUTTON_TARGET)]
        [GUIColor(nameof(FunctionalityColor))]
        [LabelText(APPASTR.Update_Targets)]
        public void UpdateTargetsButton()
        {
            using (_PRF_UpdateTargetsButton.Auto())
            {
                UpdateTargets().Forget();
            }
        }

        [ButtonGroup(GROUP_NAME_BUTTON_TARGET)]
        [GUIColor(nameof(FunctionalityColor))]
        [LabelText(APPASTR.Discover_Targets)]
        private void DiscoverTargetsButton()
        {
            using (_PRF_DiscoverTargetsButton.Auto())
            {
                DiscoverTargets();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_DiscoverTargetsButton =
            new ProfilerMarker(_PRF_PFX + nameof(DiscoverTargetsButton));

        private static readonly ProfilerMarker _PRF_UpdateTargetsButton =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateTargetsButton));

        #endregion
    }
}
