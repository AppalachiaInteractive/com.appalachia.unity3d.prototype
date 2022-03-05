using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Drivers.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Instance.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.State;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.State.Contracts;
using Appalachia.UI.Core.Components.Sets2;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Instance
{
    [CallStaticConstructorInEditor]
    public abstract class
        CursorInstance<T, TState, TMetadata, TComponentSet, TComponentSetData> : AppalachiaBehaviour<T>,
            ICursorInstance
        where T : CursorInstance<T, TState, TMetadata, TComponentSet, TComponentSetData>
        where TState : CursorInstanceStateData<TState, TMetadata>, new()
        where TComponentSet : UIComponentSet<TComponentSet, TComponentSetData>, new()
        where TComponentSetData : UIComponentSetData<TComponentSet, TComponentSetData>, new()
        where TMetadata : CursorMetadata<TMetadata>
    {
        static CursorInstance()
        {
            When.Behaviour<LifetimeComponentManager>().IsAvailableThen(i => _lifetimeComponentManager = i);
        }

        #region Static Fields and Autoproperties

        private static LifetimeComponentManager _lifetimeComponentManager;

        #endregion

        #region Fields and Autoproperties

        public TComponentSet components;
        protected TState stateData;
        protected ICursorDriver driver;

        #endregion

        protected LifetimeComponentManager LifetimeComponentManager => _lifetimeComponentManager;

        protected abstract void BeforeRendering();

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(stateData), stateData == null, () => stateData = new());
            }
        }

        private bool ShouldExecute()
        {
            using (_PRF_ShouldExecute.Auto())
            {
                if (components == null)
                {
                    return false;
                }

                if (stateData == null)
                {
                    return false;
                }

                if (!stateData.ShouldExecute())
                {
                    return false;
                }

                if (stateData.Metadata == null)
                {
                    return false;
                }

                return true;
            }
        }

        #region ICursorInstance Members

        public void UpdateForRendering()
        {
            using (_PRF_UpdateForRendering.Auto())
            {
                if (!ShouldExecute())
                {
                    return;
                }

                if (!stateData.Metadata.IsSimple)
                {
                    return;
                }

                Rect rect;

                if (stateData.CurrentLocked)
                {
                    rect = stateData.LastRect;
                }
                else
                {
                    var targetPosition = stateData.CurrentPosition;

                    var screenSize = LifetimeComponentManager.REFERENCE_RESOLUTION;

                    var x = targetPosition.x;
                    var y = screenSize.y - targetPosition.y;

                    var cursorSize = stateData.Size;
                    var hotspotOffset = cursorSize * stateData.Metadata.hotspot;

                    x -= hotspotOffset.x;
                    y -= hotspotOffset.y;

                    rect = new Rect(x, y, stateData.Texture.width, stateData.Texture.height);
                }

                stateData.RecordRect(rect);

                BeforeRendering();
            }
        }

        public IReadLimitedWriteCursorInstanceStateData StateData => stateData;

        public void ApplyDrivenData(float deltaTime)
        {
            using (_PRF_ApplyDrivenData.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                if (!ShouldExecute())
                {
                    return;
                }

                stateData.CalculateCursorMovement(deltaTime);
                stateData.CalculateRenderingRect();

                components.RectTransform.anchoredPosition = stateData.CurrentPosition;
            }
        }

        public ICursorDriver Driver => driver;

        public void SetDriver(ICursorDriver newDriver)
        {
            using (_PRF_SetDriver.Auto())
            {
                driver = newDriver;
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_ApplyDrivenData =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyDrivenData));

        protected static readonly ProfilerMarker _PRF_BeforeRendering =
            new ProfilerMarker(_PRF_PFX + nameof(BeforeRendering));

        private static readonly ProfilerMarker _PRF_SetDriver =
            new ProfilerMarker(_PRF_PFX + nameof(SetDriver));

        private static readonly ProfilerMarker _PRF_ShouldExecute =
            new ProfilerMarker(_PRF_PFX + nameof(ShouldExecute));

        private static readonly ProfilerMarker _PRF_UpdateForRendering =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateForRendering));

        #endregion
    }
}
