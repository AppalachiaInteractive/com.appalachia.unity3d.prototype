using Appalachia.UI.ControlModel.Controls.Contracts;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.Images.Groups.Outline;
using Appalachia.UI.Functionality.Text.Groups;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets
{
    public interface IDevTooltipControl : IAppaUIControl
    {
        public ImageComponentGroup TriangleBackground { get; }
        public ImageComponentGroup TriangleForeground { get; }
        public OutlineImageComponentGroup Background { get; }
        public TextMeshProUGUIComponentGroup TooltipText { get; }
    }
}
