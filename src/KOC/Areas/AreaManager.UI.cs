using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Controls.Sets2.Canvases.RootCanvas;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas
{
    public abstract partial class AreaManager<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField] protected RootCanvasComponentSet rootCanvas;

        #endregion

        public RootCanvasComponentSet RootCanvas => rootCanvas;

        public async AppaTask HideAreaInterface()
        {
            using (_PRF_HideAreaInterface.Auto())
            {
                rootCanvas.CanvasFadeManager.EnsureFadeOut();

                await OnHideAreaInterface();

                AreaInterfaceHidden?.Invoke(Area, this);
            }
        }

        public async AppaTask ShowAreaInterface()
        {
            using (_PRF_ShowAreaInterface.Auto())
            {
                rootCanvas.CanvasFadeManager.EnsureFadeIn();

                await OnShowAreaInterface();

                AreaInterfaceShown?.Invoke(Area, this);
            }
        }

        public async AppaTask ToggleAreaInterface()
        {
            using (_PRF_ToggleAreaInterface.Auto())
            {
                if (rootCanvas.CanvasGroup.IsHidden())
                {
                    if (rootCanvas.CanvasFadeManager.IsFadingIn)
                    {
                        return;
                    }

                    await ShowAreaInterface();
                }
                else
                {
                    if (rootCanvas.CanvasFadeManager.IsFadingOut)
                    {
                        return;
                    }

                    await HideAreaInterface();
                }
            }
        }

        protected virtual async AppaTask OnHideAreaInterface()
        {
            await AppaTask.CompletedTask;
        }

        protected virtual async AppaTask OnShowAreaInterface()
        {
            await AppaTask.CompletedTask;
        }

        protected async AppaTask InitializeUI(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                areaMetadata.UpdateComponentSet(
                    ref areaMetadata.rootCanvas,
                    ref rootCanvas,
                    gameObject,
                    areaObjectName
                );
            }

            await AppaTask.CompletedTask;
        }

        #region Profiling

        protected static readonly ProfilerMarker _PRF_HideAreaInterface =
            new ProfilerMarker(_PRF_PFX + nameof(HideAreaInterface));

        protected static readonly ProfilerMarker _PRF_OnHideAreaInterface =
            new ProfilerMarker(_PRF_PFX + nameof(OnHideAreaInterface));

        protected static readonly ProfilerMarker _PRF_OnShowAreaInterface =
            new ProfilerMarker(_PRF_PFX + nameof(OnShowAreaInterface));

        protected static readonly ProfilerMarker _PRF_ShowAreaInterface =
            new ProfilerMarker(_PRF_PFX + nameof(ShowAreaInterface));

        protected static readonly ProfilerMarker _PRF_ToggleAreaInterface =
            new ProfilerMarker(_PRF_PFX + nameof(ToggleAreaInterface));

        #endregion
    }
}
