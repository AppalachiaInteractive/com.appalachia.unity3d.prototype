using System;
using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.UI.Controls.Components.Layout.Models;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.ViewScaling.Models
{
    [Serializable]
    public class ViewScalingArgs : Broadcaster.IArgs
    {
        public ViewScalingArgs(ViewDimensionData dimensionData)
        {
            this.dimensionData = dimensionData;
        }

        #region Fields and Autoproperties

        public ViewDimensionData dimensionData;

        #endregion
    }
}
