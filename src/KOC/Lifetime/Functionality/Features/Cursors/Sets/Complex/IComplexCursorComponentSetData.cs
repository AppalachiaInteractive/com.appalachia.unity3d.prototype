using Appalachia.UI.Controls.Sets.MultiPartSubsetWithCanvas;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Components.Subsets;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Sets.Complex
{
    public interface IComplexCursorComponentSetData : IMultiPartComponentSubsetWithCanvasSetData<
        FadeableImageSubset, FadeableImageSubset.List, FadeableImageSubsetData, FadeableImageSubsetData.List>
    {
        AnimatorData AnimatorData { get; }
    }
}
