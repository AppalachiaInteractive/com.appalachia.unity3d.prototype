using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Animation;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Model;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Metadata
{
    public partial class ComplexCursorMetadata
    {
        private bool CanCreateController => !HasAnimatorController && HasType;
        private bool HasAnimatorController => animatorController != null;
        private bool HasType => value != ComplexCursors.None;

        [Button]
        [EnableIf(nameof(CanCreateController))]
        private void CreateAnimatorController()
        {
            using (_PRF_CreateAnimatorController.Auto())
            {
                animatorController = CursorAnimationController.Default(value.ToString());
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_CreateAnimatorController =
            new ProfilerMarker(_PRF_PFX + nameof(CreateAnimatorController));

        #endregion
    }
}
