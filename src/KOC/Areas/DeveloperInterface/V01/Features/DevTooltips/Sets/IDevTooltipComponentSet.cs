using Appalachia.UI.Core.Components.Sets.Contracts;
using Appalachia.UI.Core.Components.Subsets;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets
{
    public interface IDevTooltipComponentSet : IUIComponentSet
    {
        public ImageSubset TriangleBackground {get;}
        public ImageSubset TriangleForeground {get;}
        public OutlinedImageSubset Background {get;}
        public TextMeshProUGUISubset TooltipText {get;}
    }
}
