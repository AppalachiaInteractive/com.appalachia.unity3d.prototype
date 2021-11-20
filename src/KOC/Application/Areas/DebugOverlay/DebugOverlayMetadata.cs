using Appalachia.Prototype.KOC.Application.References;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Application.Areas.DebugOverlay
{
    public sealed class DebugOverlayMetadata : AreaMetadata<DebugOverlayManager, DebugOverlayMetadata>
    {
        #region Fields and Autoproperties

        [FoldoutGroup("Editor Only")]
        [BoxGroup("Editor Only/Graphy")]
        public AppaAsset graphyPrefab;

        [BoxGroup("Editor Only/Debug Log")]
        public AppaAsset inGameConsolePrefab;

        [BoxGroup("Editor Only/Input")]
        public InputActionReference captureScreenshot;

        [BoxGroup("Editor Only/Input")]
        public InputActionReference debugLogToggle;

        [BoxGroup("Editor Only/Input")]
        public InputActionReference graphyToggleActive;

        [BoxGroup("Editor Only/Input")]
        public InputActionReference graphyToggleModes;

        #endregion

        public override ApplicationArea Area => ApplicationArea.DebugOverlay;
    }
}
