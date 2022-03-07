using Appalachia.UI.Controls.Sets2.MultiPart.MultiPartSubsetWithCanvas;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Components.Subsets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets2.Complex
{
    public interface IComplexCursorComponentSetData : IMultiPartComponentSubsetWithCanvasSetData<
        FadeableImageSubset, FadeableImageSubset.List, FadeableImageSubsetData, FadeableImageSubsetData.List>
    {
        AnimatorData AnimatorData { get; }
    }
}
