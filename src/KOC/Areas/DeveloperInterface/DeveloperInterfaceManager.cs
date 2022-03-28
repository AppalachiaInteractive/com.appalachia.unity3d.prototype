using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Input;
using Appalachia.UI.Core.Extensions;
using Appalachia.UI.Functionality.Canvas.Controls.Unscaled;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public abstract partial class DeveloperInterfaceManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>,
        IDeveloperInterfaceManager,
        KOCInputActions.IDeveloperInterfaceActions
        where TManager : DeveloperInterfaceManager<TManager, TMetadata>
        where TMetadata : DeveloperInterfaceMetadata<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField] protected UnscaledCanvasControl unscaledCanvas;

        [SerializeField] private GameObject _unscaledWidgetObject;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                UnscaledCanvasControl.Refresh(ref unscaledCanvas, gameObject, nameof(unscaledCanvas));
                unscaledCanvas.transform.SetSiblingIndex(1);
                areaMetadata.unscaledCanvas.Apply(unscaledCanvas);

#if UNITY_EDITOR
                InitializeEditor(initializer);
#endif
            }
        }

        /// <inheritdoc />
        protected override void OnActivation()
        {
            using (_PRF_OnActivation.Auto())
            {
                Context.Log.Info(nameof(OnActivation), this);
            }
        }

        /// <inheritdoc />
        protected override void OnDeactivation()
        {
            using (_PRF_OnDeactivation.Auto())
            {
                Context.Log.Info(nameof(OnDeactivation), this);
            }
        }

        protected GameObject UnscaledWidgetObject => GetWidgetParentObject(true);

        internal GameObject GetWidgetParentObject(bool isUnscaled)
        {
            using (_PRF_GetWidgetParentObject.Auto())
            {
                if (isUnscaled)
                {
                    UnscaledCanvasControl.Refresh(
                        ref unscaledCanvas,
                        gameObject,
                        nameof(unscaledCanvas)
                    );
                    
                    UnscaledCanvas.unscaledCanvas.GameObject.GetOrAddChild(
                        ref _unscaledWidgetObject,
                        APPASTR.ObjectNames.Widgets,
                        true
                    );

                    (_unscaledWidgetObject.transform as RectTransform).FullScreen(true);

                    return _unscaledWidgetObject;
                }

                return base.GetWidgetParentObject();
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

        public UnscaledCanvasControl UnscaledCanvas => unscaledCanvas;

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.DeveloperInterface;

        /// <inheritdoc />
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
