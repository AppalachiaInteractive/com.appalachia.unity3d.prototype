#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Core
{
    public abstract partial class ActivityBarSubwidget<TSubwidget, TSubwidgetMetadata>
    {
        #region Constants and Static Readonly

        private const string NAVIGATION_BUTTON_GROUP = nameof(JumpToTooltipButton);

        #endregion

        private bool _canJumpToDevTooltipSubwidget => devTooltipSubwidget != null;

        [EnableIf(nameof(_canJumpToDevTooltipSubwidget))]
        [ButtonGroup(NAVIGATION_BUTTON_GROUP)]
        [GUIColor(nameof(NavigationColor))]
        [LabelText("Jump To Tooltip")]
        private void JumpToTooltipButton()
        {
            using (_PRF_JumpToTooltipButton.Auto())
            {
                UnityEditor.Selection.activeGameObject = devTooltipSubwidget.gameObject;
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_JumpToTooltipButton =
            new ProfilerMarker(_PRF_PFX + nameof(JumpToTooltipButton));

        #endregion
    }
}

#endif
