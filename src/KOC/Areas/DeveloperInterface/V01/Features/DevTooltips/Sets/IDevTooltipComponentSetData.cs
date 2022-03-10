using Appalachia.UI.Core.Components.Sets.Contracts;
using Appalachia.UI.Core.Components.Subsets;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets
{
    public interface IDevTooltipComponentSetData : IUIComponentSetData
    {
        public ImageSubsetData TriangleBackground { get; }
        public ImageSubsetData TriangleForeground { get; }
        public OutlinedImageSubsetData Background { get; }
        public TextMeshProUGUISubsetData TooltipText { get; }
    }
}
