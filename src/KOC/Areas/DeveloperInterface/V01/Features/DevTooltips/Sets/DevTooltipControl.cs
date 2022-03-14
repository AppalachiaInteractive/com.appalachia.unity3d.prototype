using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.ControlModel.Controls.Model;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class DevTooltipControl : BaseDevTooltipControl<DevTooltipControl,
        DevTooltipControlConfig>
    {
        /// <inheritdoc />
        public override ControlSorting DesiredComponentOrder => ControlSorting.NotFirst;

        
    }
}
