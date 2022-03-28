using Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Styling;
using Appalachia.UI.ControlModel.ComponentGroups.Default;
using Appalachia.UI.ControlModel.ComponentGroups.Fadeable;
using Appalachia.UI.ControlModel.Controls.Default.Contracts;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.Images.Groups.Outline;
using Appalachia.UI.Functionality.Text.Groups;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Control.Contracts
{
    public interface ITooltipControlConfig : IAppaUIControlConfig
    {
        BasicUIComponentGroupConfig Triangle { get; }
        TooltipStyleOverride StyleOverride { get; }
        FadeableComponentGroupConfig Tooltip { get; }
        ImageComponentGroupConfig TriangleBackground { get; }
        ImageComponentGroupConfig TriangleForeground { get; }
        OutlinedImageComponentGroupConfig Background { get; }
        TextMeshProUGUIComponentGroupConfig TooltipText { get; }
    }
}
