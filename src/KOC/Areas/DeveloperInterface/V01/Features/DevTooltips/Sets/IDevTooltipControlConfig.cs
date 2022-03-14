using Appalachia.UI.ControlModel.Controls.Contracts;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.Images.Groups.Outline;
using Appalachia.UI.Functionality.Text.Groups;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets
{
    public interface IDevTooltipControlConfig : IAppaUIControlConfig
    {
        public ImageComponentGroupConfig TriangleBackground { get; }
        public ImageComponentGroupConfig TriangleForeground { get; }
        public OutlineImageComponentGroupConfig Background { get; }
        public TextMeshProUGUIComponentGroupConfig TooltipText { get; }
    }
}
