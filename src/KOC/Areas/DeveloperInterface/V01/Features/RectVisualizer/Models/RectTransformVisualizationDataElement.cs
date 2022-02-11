using System;
using Appalachia.Core.ArrayPooling;
using Appalachia.Core.ObjectPooling;
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

        public Color color;
        public Vector3[] corners;

        #endregion

        public Vector3 bottomLeft => corners[0];
        public Vector3 bottomRight => corners[3];
        public Vector3 topLeft => corners[1];
        public Vector3 topRight => corners[2];

        public override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                corners = ArrayPool<Vector3>.Shared.Rent(4);
                color = Color.white;
            }
        }

        public override void Reset()
        {
            using (_PRF_Reset.Auto())
            {
                if (corners != null)
                {
                    ArrayPool<Vector3>.Shared.Return(corners);
                }

                corners = null;
                color = Color.clear;
            }
        }

        public void CopyCorners(Vector3[] cornersArray, float z)
        {
            using (_PRF_CopyCorners.Auto())
            {
                corners[0] = cornersArray[0];
                corners[1] = cornersArray[1];
                corners[2] = cornersArray[2];
                corners[3] = cornersArray[3];
                corners[0].z = z;
                corners[1].z = z;
                corners[2].z = z;
                corners[3].z = z;
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
                        drawCommandBuilder.Line(bottomLeft,  topLeft);
                        drawCommandBuilder.Line(topLeft,     topRight);
                        drawCommandBuilder.Line(topRight,    bottomRight);
                        drawCommandBuilder.Line(bottomRight, bottomLeft);
                    }
                );
            }
        }

        public void Grow(int amount = 1)
        {
            using (_PRF_Grow.Auto())
            {
                corners[0].x -= amount;
                corners[0].y -= amount;
                corners[1].x -= amount;
                corners[1].y += amount;
                corners[2].x += amount;
                corners[2].y += amount;
                corners[3].x += amount;
                corners[3].y -= amount;
            }
        }

        public void Shrink(int amount = 1)
        {
            using (_PRF_Shrink.Auto())
            {
                corners[0].x += amount;
                corners[0].y += amount;
                corners[1].x += amount;
                corners[1].y -= amount;
                corners[2].x -= amount;
                corners[2].y -= amount;
                corners[3].x -= amount;
                corners[3].y += amount;
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

        private static readonly ProfilerMarker _PRF_DrawAsCube =
            new ProfilerMarker(_PRF_PFX + nameof(DrawAsCube));

        private static readonly ProfilerMarker _PRF_DrawInternal =
            new ProfilerMarker(_PRF_PFX + nameof(DrawInternal));

        private static readonly ProfilerMarker _PRF_CopyCorners =
            new ProfilerMarker(_PRF_PFX + nameof(CopyCorners));

        private static readonly ProfilerMarker _PRF_Grow = new ProfilerMarker(_PRF_PFX + nameof(Grow));

        private static readonly ProfilerMarker _PRF_Shrink = new ProfilerMarker(_PRF_PFX + nameof(Shrink));

        #endregion
    }
}
