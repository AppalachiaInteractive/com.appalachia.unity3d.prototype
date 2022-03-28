using Appalachia.UI.ControlModel.Controls.Default.Contracts;
using Appalachia.UI.Functionality.Layout.Groups.Horizontal;
using Appalachia.UI.Functionality.Tooltips.Controls.Base.Contracts;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Main.Contracts
{
    public interface IStatusBarControl : IAppaUIControl
    {
        HorizontalLayoutGroupComponentGroup LeftStatusBar { get; }
        HorizontalLayoutGroupComponentGroup RightStatusBar { get; }
        GameObject LeftStatusBarParent { get; set; }
        GameObject RightStatusBarParent { get; set; }
    }
}
