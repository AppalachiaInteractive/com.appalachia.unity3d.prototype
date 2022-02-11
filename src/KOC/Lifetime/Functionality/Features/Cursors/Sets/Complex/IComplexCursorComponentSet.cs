using Appalachia.UI.Controls.Animation;
using Appalachia.UI.Controls.Sets.MultiPartSubsetWithCanvas;
using Appalachia.UI.Core.Components.Subsets;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Sets.Complex
{
    public interface IComplexCursorComponentSet : IMultiPartComponentSubsetWithCanvasSet<FadeableImageSubset,
        FadeableImageSubset.List, FadeableImageSubsetData, FadeableImageSubsetData.List>
    {
#if UNITY_EDITOR
        AnimationRemapper AnimationRemapper { get; }
#endif
        Animator Animator { get; }
    }
}
