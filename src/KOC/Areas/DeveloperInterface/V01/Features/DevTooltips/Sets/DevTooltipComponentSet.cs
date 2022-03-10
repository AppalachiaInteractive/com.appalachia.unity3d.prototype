using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Components.Sets;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class DevTooltipComponentSet : BaseDevTooltipComponentSet<DevTooltipComponentSet,
        DevTooltipComponentSetData, IDevTooltipComponentSetData>
    {
        /// <inheritdoc />
        public override ComponentSetSorting DesiredComponentOrder => ComponentSetSorting.NotFirst;

        /// <summary>
        ///     Defines the name of the component set.
        /// </summary>
        public override string ComponentSetNamePrefix => APPASTR.Tooltip;

        protected override bool IsUI => true;
    }
}
