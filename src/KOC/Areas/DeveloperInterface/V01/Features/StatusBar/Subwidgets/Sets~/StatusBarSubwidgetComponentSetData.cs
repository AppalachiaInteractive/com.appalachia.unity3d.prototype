using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.UI.Controls.Sets2.Buttons.Button;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Sets
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren][CreateAssetMenu(
        fileName = "New " + nameof(StatusBarSubwidgetComponentSetData),
        menuName = PKG.Prefix + nameof(StatusBarSubwidgetComponentSetData),
        order = PKG.Menu.Assets.Priority
    )]

    public sealed class
        StatusBarSubwidgetComponentSetData : BaseButtonComponentSetData<StatusBarSubwidgetComponentSet,
            StatusBarSubwidgetComponentSetData>
    {
    }
}
