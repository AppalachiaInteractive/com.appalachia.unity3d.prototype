using System.Collections.Generic;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Instance.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Utility.Async;
using Appalachia.Utility.Timing;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Services
{
    public class CursorService : LifetimeService<CursorService, CursorServiceMetadata, CursorFeature,
        CursorFeatureMetadata>
    {
        #region Fields and Autoproperties

        private List<ICursorInstance> _cursorInstances;

        #endregion

        public IReadOnlyList<ICursorInstance> CursorInstances
        {
            get
            {
                _cursorInstances ??= new List<ICursorInstance>();
                return _cursorInstances;
            }
        }

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                var cursorIntances = CursorInstances;

                for (var index = 0; index < cursorIntances.Count; index++)
                {
                    var cursorInstance = cursorIntances[index];
                    var driver = cursorInstance.Driver;

                    var state = cursorInstance.StateData;

                    var time = CoreClock.Instance.Time;
                    var deltaTime = CoreClock.Instance.DeltaTime;
                    var bounds = Manager.ScaledCanvasBounds;

                    driver.DriveCursorInstance(state, time, deltaTime, bounds);

                    cursorInstance.ApplyDrivenData(deltaTime);

                    cursorInstance.UpdateForRendering();
                }
            }
        }

        #endregion

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                if (metadata.initialSoftwareCursorState.HasFlag(SoftwareCursorStateFlags.Locked))
                {
                    LockSoftwareCursor();
                }

                if (metadata.initialSoftwareCursorState.HasFlag(SoftwareCursorStateFlags.Confined))
                {
                    ConfineSoftwareCursor();
                }

                if (metadata.initialSoftwareCursorState.HasFlag(SoftwareCursorStateFlags.Hidden))
                {
                    HideSoftwareCursor();
                }
            }
        }

        [BoxGroup("Software Cursor")]
        [ButtonGroup("Software Cursor/ConfineRelease")]
        private void ConfineSoftwareCursor()
        {
            using (_PRF_ConfineSoftwareCursor.Auto())
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        [ButtonGroup("Software Cursor/ShowHide")]
        private void HideSoftwareCursor()
        {
            using (_PRF_HideSoftwareCursor.Auto())
            {
                Cursor.visible = false;
            }
        }

        [ButtonGroup("Software Cursor/LockUnlock")]
        private void LockSoftwareCursor()
        {
            using (_PRF_LockSoftwareCursor.Auto())
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        [ButtonGroup("Software Cursor/ConfineRelease")]
        private void ReleaseSoftwareCursor()
        {
            using (_PRF_ReleaseSoftwareCursor.Auto())
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        [ButtonGroup("Software Cursor/ShowHide")]
        private void ShowSoftwareCursor()
        {
            using (_PRF_ShowSoftwareCursor.Auto())
            {
                Cursor.visible = true;
            }
        }

        [ButtonGroup("Software Cursor/LockUnlock")]
        private void UnlockSoftwareCursor()
        {
            using (_PRF_UnlockSoftwareCursor.Auto())
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ConfineSoftwareCursor =
            new ProfilerMarker(_PRF_PFX + nameof(ConfineSoftwareCursor));

        private static readonly ProfilerMarker _PRF_HideSoftwareCursor =
            new ProfilerMarker(_PRF_PFX + nameof(HideSoftwareCursor));

        private static readonly ProfilerMarker _PRF_LockSoftwareCursor =
            new ProfilerMarker(_PRF_PFX + nameof(LockSoftwareCursor));

        private static readonly ProfilerMarker _PRF_ReleaseSoftwareCursor =
            new ProfilerMarker(_PRF_PFX + nameof(ReleaseSoftwareCursor));

        private static readonly ProfilerMarker _PRF_ShowSoftwareCursor =
            new ProfilerMarker(_PRF_PFX + nameof(ShowSoftwareCursor));

        private static readonly ProfilerMarker _PRF_UnlockSoftwareCursor =
            new ProfilerMarker(_PRF_PFX + nameof(UnlockSoftwareCursor));

        #endregion
    }
}
