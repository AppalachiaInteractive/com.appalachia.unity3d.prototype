using System;
using Appalachia.Prototype.KOC.Application.Lifetime;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Pooling.Objects;
using Appalachia.Utility.Standards;
using Drawing;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Models
{
#pragma warning disable CS0612
    [Serializable]
    public class
        RectTransformVisualizationDataElement : SelfPoolingObject<RectTransformVisualizationDataElement>
#pragma warning restore CS0612
    {
        #region Fields and Autoproperties

        public bool drawLocatorLine;
        public Color color;
        public Rect rect;
        public bool isSelected;
        public float thickness;
        public float z;

        #endregion

        public Vector3 BottomLeft => rect.GetBottomLeftCorner();
        public Vector3 BottomRight => rect.GetBottomRightCorner();
        public Vector3 TopLeft => rect.GetTopLeft();
        public Vector3 TopRight => rect.GetTopRight();

        /// <inheritdoc />
        public override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                color = Color.white;
            }
        }

        /// <inheritdoc />
        public override void Reset()
        {
            using (_PRF_Reset.Auto())
            {
                rect = default;
                z = default;
                color = Color.clear;
            }
        }

        public void CopyRect(Rect rect, float z, float thickness, bool isSelected)
        {
            using (_PRF_CopyRect.Auto())
            {
                this.rect = rect;
                this.z = z;
                this.thickness = thickness;
                this.isSelected = isSelected;
            }
        }

        public void DrawAsCube(CommandBuilder drawCommandBuilder)
        {
            using (_PRF_DrawAsCube.Auto())
            {
                DrawInternal(
                    drawCommandBuilder,
                    color,
                    () =>
                    {
                        using (drawCommandBuilder.WithLineWidth(thickness))
                        {
                            drawCommandBuilder.Line(BottomLeft,  TopLeft);
                            drawCommandBuilder.Line(TopLeft,     TopRight);
                            drawCommandBuilder.Line(TopRight,    BottomRight);
                            drawCommandBuilder.Line(BottomRight, BottomLeft);

                            if (isSelected)
                            {
                                drawCommandBuilder.Line(BottomLeft, TopRight);
                                drawCommandBuilder.Line(TopLeft,    BottomRight);
                            }

                            if (drawLocatorLine)
                            {
                                var center = .5f * (BottomLeft + TopRight);
                                var screenCenter = Constants.SCREEN_CENTER;

                                drawCommandBuilder.Line(center, screenCenter);
                            }
                        }
                    }
                );
            }
        }

        public void Grow(float amount = 1f)
        {
            using (_PRF_Grow.Auto())
            {
                rect = rect.Expand(amount);
            }
        }

        public void Shrink(float amount = 1f)
        {
            using (_PRF_Shrink.Auto())
            {
                rect = rect.Expand(-amount);
            }
        }

        private static void DrawInternal(CommandBuilder drawCommandBuilder, Color color, Action drawAction)
        {
            using (_PRF_DrawInternal.Auto())
            {
                using (drawCommandBuilder.WithColor(color))
                {
                    drawAction();
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_CopyRect =
            new ProfilerMarker(_PRF_PFX + nameof(CopyRect));

        private static readonly ProfilerMarker _PRF_DrawAsCube =
            new ProfilerMarker(_PRF_PFX + nameof(DrawAsCube));

        private static readonly ProfilerMarker _PRF_DrawInternal =
            new ProfilerMarker(_PRF_PFX + nameof(DrawInternal));

        private static readonly ProfilerMarker _PRF_CopyCorners =
            new ProfilerMarker(_PRF_PFX + nameof(CopyRect));

        private static readonly ProfilerMarker _PRF_Grow = new ProfilerMarker(_PRF_PFX + nameof(Grow));

        private static readonly ProfilerMarker _PRF_Shrink = new ProfilerMarker(_PRF_PFX + nameof(Shrink));

        #endregion
    }
}
