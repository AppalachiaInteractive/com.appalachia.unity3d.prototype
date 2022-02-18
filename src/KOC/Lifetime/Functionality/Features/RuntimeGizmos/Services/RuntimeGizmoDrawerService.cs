using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Services;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Drawing;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.RuntimeGizmos.Services
{
    [ExecutionOrder(ExecutionOrders.GizmoDrawerService)]
    [RequireComponent(typeof(Camera))]
    public class RuntimeGizmoDrawerService : LifetimeService<RuntimeGizmoDrawerService,
                                                 RuntimeGizmoDrawerServiceMetadata, RuntimeGizmoDrawerFeature,
                                                 RuntimeGizmoDrawerFeatureMetadata>,
                                             GizmoDrawer.IService
    {
        #region Fields and Autoproperties

        private GizmoDrawer.Service _service;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                Camera drawCamera = null;
                this.GetOrAddComponent(ref drawCamera);
                _service ??= new GizmoDrawer.Service(drawCamera);
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
