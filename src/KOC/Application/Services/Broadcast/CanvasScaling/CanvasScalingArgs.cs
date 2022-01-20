using System;
using Appalachia.UI.Controls.Components.Layout.Models;

namespace Appalachia.Prototype.KOC.Application.Services.Broadcast.CanvasScaling
{
    [Serializable]
    public class CanvasScalingArgs : BroadcastArgs
    {
        public CanvasScalingArgs(CanvasDimensionData dimensionData)
        {
            this.dimensionData = dimensionData;
        }

        #region Fields and Autoproperties

        public CanvasDimensionData dimensionData;

        #endregion
    }
}
