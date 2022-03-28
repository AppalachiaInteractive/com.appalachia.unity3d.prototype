using Appalachia.UI.Core.Layout;
using Appalachia.UI.Styling.Elements;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Styling
{
    public interface ITooltipStyle : IStyleElement
    {
        AppearanceDirection Direction { get; }
        bool ShowTriangle { get; }
        Color BackgroundColor { get; }
        Color OutlineColor { get; }
        float DistanceFromTarget { get; }
        float OutlineThickness { get; }
        float TextPadding { get; }
        float TriangleSize { get; }
        Sprite TriangleSprite { get; }

        #region Profiling

        private const string _PRF_PFX = nameof(ITooltipStyle) + ".";

        #endregion

        /*public void Apply(TextMeshPro component)
        {
            using (_PRF_Apply.Auto())
            {
                component.enableAutoSizing = AutoTooltipSize;
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
