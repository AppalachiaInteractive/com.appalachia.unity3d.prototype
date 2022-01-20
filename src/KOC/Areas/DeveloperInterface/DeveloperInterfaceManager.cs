using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Input;
using Appalachia.UI.Controls.Sets.RootCanvas;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public abstract partial class DeveloperInterfaceManager<TManager, TMetadata> :
        AreaManager<TManager, TMetadata>,
        IDeveloperInterfaceManager,
        KOCInputActions.IDeveloperInterfaceActions
        where TManager : DeveloperInterfaceManager<TManager, TMetadata>
        where TMetadata : DeveloperInterfaceMetadata<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField]
        [FoldoutGroup(APPASTR.Components + "/" + APPASTR.Unscaled_Canvas)]
        protected RootCanvasComponentSet unscaledCanvas;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                areaMetadata.UnscaledCanvas.PrepareAndConfigure(ref unscaledCanvas, gameObject, "Unscaled");

                InitializeEditor(initializer, areaObjectName);
            }
        }

        protected override void OnActivation()
        {
            using (_PRF_OnActivation.Auto())
            {
                Context.Log.Info(nameof(OnActivation), this);
            }
        }

        protected override void OnDeactivation()
        {
            using (_PRF_OnDeactivation.Auto())
            {
                Context.Log.Info(nameof(OnDeactivation), this);
            }
        }

        #region IDeveloperInterfaceActions Members

        public abstract void OnOpenCommandPalette(InputAction.CallbackContext context);
        public abstract void OnToggleDeveloperInterface(InputAction.CallbackContext context);
        public abstract void OnToggleMenuBar(InputAction.CallbackContext context);
        public abstract void OnToggleStatusBar(InputAction.CallbackContext context);
        public abstract void OnToggleActivityBar(InputAction.CallbackContext context);
        public abstract void OnToggleSideBar(InputAction.CallbackContext context);
        public abstract void OnTogglePanel(InputAction.CallbackContext context);
        public abstract void OnScreenshot(InputAction.CallbackContext context);

        #endregion

        #region IDeveloperInterfaceManager Members

        public RootCanvasComponentSet UnscaledCanvas => unscaledCanvas;

        public override ApplicationArea Area => ApplicationArea.DeveloperInterface;
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_OnOpenCommandPalette =
            new ProfilerMarker(_PRF_PFX + nameof(OnOpenCommandPalette));

        protected static readonly ProfilerMarker _PRF_OnScreenshot =
            new ProfilerMarker(_PRF_PFX + nameof(OnScreenshot));

        protected static readonly ProfilerMarker _PRF_OnToggleActivityBar =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleActivityBar));

        protected static readonly ProfilerMarker _PRF_OnToggleDeveloperInterface =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleDeveloperInterface));

        protected static readonly ProfilerMarker _PRF_OnToggleMenuBar =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleMenuBar));

        protected static readonly ProfilerMarker _PRF_OnTogglePanel =
            new ProfilerMarker(_PRF_PFX + nameof(OnTogglePanel));

        protected static readonly ProfilerMarker _PRF_OnToggleSideBar =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleSideBar));

        protected static readonly ProfilerMarker _PRF_OnToggleStatusBar =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleStatusBar));

        #endregion
    }
}
