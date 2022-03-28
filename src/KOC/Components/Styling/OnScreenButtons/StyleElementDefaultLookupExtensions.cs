using Appalachia.Prototype.KOC.Components.Styling.OnScreenButtons;
using Unity.Profiling;

namespace Appalachia.UI.Styling.OnScreenButtons
{
    public static class StyleElementDefaultLookupExtensions
    {
        public static OnScreenButtonStyleOverride GetOnScreenButton(
            this StyleElementDefaultLookup lookup,
            OnScreenButtonStyleTypes type)
        {
            using (_PRF_GetOnScreenButton.Auto())
            {
                return lookup
                   .GetOverride<OnScreenButtonStyle, OnScreenButtonStyleOverride, IOnScreenButtonStyle,
                        OnScreenButtonStyleTypes>(type);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(StyleElementDefaultLookupExtensions) + ".";

        private static readonly ProfilerMarker _PRF_GetOnScreenButton =
            new ProfilerMarker(_PRF_PFX + nameof(GetOnScreenButton));

        #endregion
    }
}
