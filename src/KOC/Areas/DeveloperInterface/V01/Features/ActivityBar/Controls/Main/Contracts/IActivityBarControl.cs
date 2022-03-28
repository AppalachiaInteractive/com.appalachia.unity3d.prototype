using Appalachia.UI.ControlModel.Controls.Default.Contracts;
using Appalachia.UI.Functionality.Layout.Groups.Vertical;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Controls.Main.Contracts
{
    public interface IActivityBarControl : IAppaUIControl
    {
        VerticalLayoutGroupComponentGroup TopActivityBar { get; }
        VerticalLayoutGroupComponentGroup BottomActivityBar { get; }
        GameObject TopActivityBarParent { get; set; }
        GameObject BottomActivityBarParent { get; set; }
    }
}
