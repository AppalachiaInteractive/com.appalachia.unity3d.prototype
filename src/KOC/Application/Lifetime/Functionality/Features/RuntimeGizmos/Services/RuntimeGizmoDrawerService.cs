using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Extensions;
using Drawing;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos.Services
{
    [ExecutionOrder(ExecutionOrders.GizmoDrawerService)]
    [RequireComponent(typeof(Camera))]
    public class RuntimeGizmoDrawerService : LifetimeService<RuntimeGizmoDrawerService,
                                                 RuntimeGizmoDrawerServiceMetadata, RuntimeGizmoDrawerFeature,
                                                 RuntimeGizmoDrawerFeatureMetadata>,
                                             GizmoDrawer.IService
    {
        
        static RuntimeGizmoDrawerService()
        {
            When.AnyInstance<IRuntimeGizmoDrawer>()
                .IsEnabledThen(
                     args =>
                     {
                         When.Behaviour<RuntimeGizmoDrawerService>()
                             .IsAvailableThen(svc => { svc.OnDraw.Event += args.value.DrawGizmos; });
                     }
                 );
        }
        
        #region Fields and Autoproperties

        public AppaEvent<CommandBuilder>.Data OnDraw;
        private GizmoDrawer.Service _service;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                Camera drawCamera = null;
                this.GetOrAddComponent(ref drawCamera);

                initializer.Do(
                    this,
                    nameof(_service),
                    _service == null,
                    () => _service = new GizmoDrawer.Service(drawCamera)
                );
            }

            await _service.Initialize();
        }

        #region IService Members

        public Camera DrawCamera => _service.DrawCamera;
        public CommandBuilder Draw => _service.Draw;

        public RenderTexture GetRenderTexture()
        {
            using (_PRF_GetRenderTexture.Auto())
            {
                return _service.GetRenderTexture();
            }
        }

        public void OnPreCull()
        {
            using (_PRF_OnPreCull.Auto())
            {
                if (!FullyInitialized || ShouldSkipUpdate)
                {
                    return;
                }

                OnDraw.RaiseEvent(Draw);
                _service.OnPreCull();
            }
        }

        async AppaTask GizmoDrawer.IService.WhenEnabled()
        {
            await WhenEnabled();
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_GetRenderTexture =
            new ProfilerMarker(_PRF_PFX + nameof(GetRenderTexture));

        protected static readonly ProfilerMarker _PRF_OnPreCull =
            new ProfilerMarker(_PRF_PFX + nameof(OnPreCull));

        #endregion
    }
}
