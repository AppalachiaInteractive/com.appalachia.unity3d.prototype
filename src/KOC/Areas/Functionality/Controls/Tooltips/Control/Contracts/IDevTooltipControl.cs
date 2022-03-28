using Appalachia.Prototype.KOC.Areas.Functionality.Controls.Default.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Components;
using Appalachia.UI.ControlModel.ComponentGroups.Default;
using Appalachia.UI.ControlModel.ComponentGroups.Fadeable;
using Appalachia.UI.ControlModel.Controls.Default.Contracts;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.Images.Groups.Outline;
using Appalachia.UI.Functionality.Text.Groups;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Control.Contracts
{
    public interface ITooltipControl : IAppaUIControl
    {
        BasicUIComponentGroup Triangle { get; }
        FadeableComponentGroup Tooltip { get; }
        GameObject BackgroundParent { get; }
        GameObject TooltipParent { get; }
        GameObject TooltipTextParent { get; }
        GameObject TriangleBackgroundParent { get; }
        GameObject TriangleForegroundParent { get; }
        GameObject TriangleParent { get; }
        ImageComponentGroup TriangleBackground { get; }
        ImageComponentGroup TriangleForeground { get; }
        OutlinedImageComponentGroup Background { get; }
        TextMeshProUGUIComponentGroup TooltipText { get; }
        AppaTooltipTarget Target { get; }
    }
}
