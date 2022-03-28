using Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Components;
using Appalachia.UI.ControlModel.ComponentGroups.Default;
using Appalachia.UI.ControlModel.Controls.Default.Contracts;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.Images.Groups.Outline;
using Appalachia.UI.Functionality.Text.Groups;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Control.Contracts
{
    public interface ITooltipControl : IAppaUIControl
    {
        AppaTooltipTarget Target { get; }
        BasicUIComponentGroup Triangle { get; }
        GameObject BackgroundParent { get; }
        GameObject TriangleBackgroundParent { get; }
        GameObject TriangleForegroundParent { get; }
        GameObject TriangleParent { get; }
        GameObject TextParent { get; }
        ImageComponentGroup TriangleBackground { get; }
        ImageComponentGroup TriangleForeground { get; }
        OutlinedImageComponentGroup Background { get; }
        TextMeshProUGUIComponentGroup Text { get; }
    }
}
