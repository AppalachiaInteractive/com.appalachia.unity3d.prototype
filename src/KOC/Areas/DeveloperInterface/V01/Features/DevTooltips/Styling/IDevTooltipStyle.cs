using Appalachia.UI.Core.Styling.Elements;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Styling
{
    public interface IDevTooltipStyle : IStyleElement
    {
        bool ShowTriangle { get; }
        Color BackgroundColor { get; }
        Color OutlineColor { get; }
        float DistanceFromTarget { get; }
        float OutlineThickness { get; }
        float TextPadding { get; }
        float TriangleSize { get; }
        Sprite TriangleSprite { get; }
        TooltipAppearanceDirection Direction { get; }

        #region Profiling

        private const string _PRF_PFX = nameof(IDevTooltipStyle) + ".";

        #endregion

        /*public void Apply(TextMeshPro component)
        {
            using (_PRF_Apply.Auto())
            {
                component.enableAutoSizing = AutoDevTooltipSize;
                Sprite triangleSprite
                TooltipAppearanceDirection direction
                float distanceFromTarget
                float textPadding
                Color backgroundColor
                Color outlineColor
                float outlineThickness
                bool showTriangle
                float triangleSize
            } 
            
            private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));
       
        }*/
    }
}
