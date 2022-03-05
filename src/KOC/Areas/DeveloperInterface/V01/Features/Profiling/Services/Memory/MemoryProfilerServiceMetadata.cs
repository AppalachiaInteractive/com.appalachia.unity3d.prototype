using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Services.Memory
{
    public sealed class MemoryProfilerServiceMetadata : DeveloperInterfaceMetadata_V01.ServiceMetadata<
        MemoryProfilerService, MemoryProfilerServiceMetadata, PerformanceProfilingFeature,
        PerformanceProfilingFeatureMetadata>
    {
        #region Fields and Autoproperties

        [BoxGroup("Colors")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public Color allocatedRamColor;

        [BoxGroup("Colors")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public Color reservedRamColor;

        [BoxGroup("Colors")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public Color monoRamColor;

        [BoxGroup("General")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [PropertyRange(10, 300)]
        public int graphResolution;

        [BoxGroup("General")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [PropertyRange(1, 10)]
        public int textUpdateRate;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
                    this,
                    nameof(allocatedRamColor),
                    () => allocatedRamColor = new Color32(255, 190, 60, 255)
                );
                initializer.Do(
                    this,
                    nameof(reservedRamColor),
                    () => reservedRamColor = new Color32(205, 84, 229, 255)
                );
                initializer.Do(this, nameof(monoRamColor),    () => monoRamColor = new(0.3f, 0.65f, 1f, 1));
                initializer.Do(this, nameof(graphResolution), () => graphResolution = 150);
                initializer.Do(this, nameof(textUpdateRate),  () => textUpdateRate = 3);
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(MemoryProfilerService target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(MemoryProfilerService functionality)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
            }
        }
    }
}
