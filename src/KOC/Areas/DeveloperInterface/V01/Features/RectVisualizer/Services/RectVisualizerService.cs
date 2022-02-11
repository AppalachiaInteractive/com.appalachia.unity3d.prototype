using System.Linq;
using Appalachia.Core.Attributes;
using Appalachia.Core.Collections.NonSerialized;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Models;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Drawing;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Services
{
    [ExecutionOrder(ExecutionOrders.GizmoDrawerService)]
    [RequireComponent(typeof(Camera))]
    public class RectVisualizerService : DeveloperInterfaceManager_V01.Service<RectVisualizerService,
                                             RectVisualizerServiceMetadata, RectVisualizerFeature,
                                             RectVisualizerFeatureMetadata>,
                                         GizmoDrawer.IService
    {
        #region Fields and Autoproperties

        private GizmoDrawer.Service _service;

        private RectTransform[] _rectTransforms;
        private NonSerializedList<RectTransformVisualizationData> _rectDatas;

        #endregion

        [ReadOnly, ShowInInspector]
        public int RectCount => _rectTransforms?.Length ?? 0;

        public NonSerializedList<RectTransformVisualizationData> RectDatas
        {
            get
            {
                if (_rectDatas == null)
                {
                    _rectDatas ??= new NonSerializedList<RectTransformVisualizationData>();
                }

                return _rectDatas;
            }
        }

        [ButtonGroup(GROUP_NAME)]
        public void DiscoverTargets()
        {
            using (_PRF_DiscoverTargets.Auto())
            {
                _rectTransforms = FindObjectsOfType<RectTransform>();

                for (var index = 0; index < RectDatas.Count; index++)
                {
                    var rectData = RectDatas[index];
                    rectData.Return();
                }

                RectDatas.ClearFast();

                UpdateTargets().Forget();
            }
        }

        [ButtonGroup(GROUP_NAME)]
        public async AppaTask UpdateTargets()
        {
            using (_PRF_UpdateTargets.Auto())
            {
                if (_rectTransforms == null)
                {
                    DiscoverTargets();
                }

                var iterations = 0;
                var rebuildArray = false;

                for (var rectTransformIndex = 0;
                     rectTransformIndex < _rectTransforms.Length;
                     rectTransformIndex++)
                {
                    if (!enabled || ShouldSkipUpdate)
                    {
                        return;
                    }

                    var rt = _rectTransforms[rectTransformIndex];

                    if (rt == null)
                    {
                        rebuildArray = true;
                        continue;
                    }

                    RectTransformVisualizationData data;

                    if (RectDatas.Count > rectTransformIndex)
                    {
                        data = RectDatas[rectTransformIndex];
                    }
                    else
                    {
                        data = RectTransformVisualizationData.Get();
                        RectDatas.Add(data);
                    }

                    data.Create(rt, Feature.Metadata);

                    if (iterations > metadata.updateSteps)
                    {
                        iterations = 0;
                        await AppaTask.Yield();
                    }
                    else
                    {
                        iterations += 1;
                    }
                }

                if (rebuildArray)
                {
                    _rectTransforms = _rectTransforms.Where(rt => rt != null).ToArray();
                }
            }
        }

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

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                Run().Forget();
            }
        }

        private async AppaTask Run()
        {
            while (enabled)
            {
                await UpdateTargets();
                await metadata.updates.Delay();
            }
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
                if (!FullyInitialized)
                {
                    return;
                }

                _service.OnPreCull();
            }
        }

        async AppaTask GizmoDrawer.IService.WhenEnabled()
        {
            await WhenEnabled();
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_DiscoverTargets =
            new ProfilerMarker(_PRF_PFX + nameof(DiscoverTargets));

        protected static readonly ProfilerMarker _PRF_GetRenderTexture =
            new ProfilerMarker(_PRF_PFX + nameof(GetRenderTexture));

        protected static readonly ProfilerMarker _PRF_OnPreCull =
            new ProfilerMarker(_PRF_PFX + nameof(OnPreCull));

        private static readonly ProfilerMarker _PRF_Run = new ProfilerMarker(_PRF_PFX + nameof(Run));

        private static readonly ProfilerMarker _PRF_UpdateTargets =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateTargets));

        #endregion
    }
}
