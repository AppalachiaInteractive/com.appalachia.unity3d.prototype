using System;
using Appalachia.Core.Attributes.Editing;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class DevTooltipComponentSetData : BaseDevTooltipComponentSetData<DevTooltipComponentSet,
        DevTooltipComponentSetData, IDevTooltipComponentSetData>
    {
    }
}
