using System;
using Appalachia.Core.Attributes.Editing;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren][CreateAssetMenu(
        fileName = "New " + nameof(DevTooltipComponentSetData),
        menuName = PKG.Prefix + nameof(DevTooltipComponentSetData),
        order = PKG.Menu.Assets.Priority
    )]

    public sealed class DevTooltipComponentSetData : BaseDevTooltipComponentSetData<DevTooltipComponentSet,
        DevTooltipComponentSetData, IDevTooltipComponentSetData>
    {
    }
}
