using Appalachia.UI.Animations;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.MultiPartCanvas.Contracts;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Complex.Contracts
{
    public interface IComplexCursorControl : IMultiPartCanvasControl<ImageComponentGroup,
        ImageComponentGroup.List, ImageComponentGroupConfig, ImageComponentGroupConfig.List>
    {
#if UNITY_EDITOR
        AnimationRemapper AnimationRemapper { get; }
#endif
        Animator Animator { get; }
    }
}
