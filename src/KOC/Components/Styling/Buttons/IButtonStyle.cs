using Appalachia.Prototype.KOC.Components.Styling.Base;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Components.Styling.Buttons
{
    public interface IButtonStyle : IApplicationStyle
    {
        Color DisabledColor { get; }
        Color HighlightedColor { get; }
        Color NormalColor { get; }
        Color PressedColor { get; }
        Color SelectedColor { get; }
        float ColorMultiplier { get; }
        float FadeDuration { get; }
        float FontSize { get; }
        TextAlignmentOptions Alignment { get; }

        public void Apply(Button component)
        {
            using (_PRF_Apply.Auto())
            {
            }
        }

        public void Apply(TextMeshProUGUI component)
        {
            using (_PRF_Apply.Auto())
            {
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(IButtonStyle) + ".";

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
