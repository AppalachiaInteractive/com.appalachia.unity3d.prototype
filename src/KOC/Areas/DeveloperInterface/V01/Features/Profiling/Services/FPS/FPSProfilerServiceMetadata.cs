using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Services.FPS
{
    public sealed class FPSProfilerServiceMetadata : DeveloperInterfaceMetadata_V01.ServiceMetadata<
        FPSProfilerService, FPSProfilerServiceMetadata, PerformanceProfilingFeature,
        PerformanceProfilingFeatureMetadata>
    {
        #region Fields and Autoproperties

        [BoxGroup("Good")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public Color goodFpsColor;

        [BoxGroup("Good")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [PropertyRange(45f, 200f)]
        public int goodFpsThreshold;

        [BoxGroup("Caution")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public Color cautionFpsColor;

        [BoxGroup("Caution")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [PropertyRange(30f, 60f)]
        public int cautionFpsThreshold;

        [BoxGroup("Critical")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public Color criticalFpsColor;

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
                    nameof(goodFpsColor),
                    () => goodFpsColor = new Color32(118, 212, 58, 255)
                );
                initializer.Do(this, nameof(goodFpsThreshold), () => goodFpsThreshold = 60);
                initializer.Do(
                    this,
                    nameof(cautionFpsColor),
                    () => cautionFpsColor = new Color32(243, 232, 0, 255)
                );
                initializer.Do(this, nameof(cautionFpsThreshold), () => cautionFpsThreshold = 30);
                initializer.Do(
                    this,
                    nameof(criticalFpsColor),
                    () => criticalFpsColor = new Color32(220, 41, 30, 255)
                );
                initializer.Do(this, nameof(graphResolution), () => graphResolution = 150);
                initializer.Do(this, nameof(textUpdateRate),  () => textUpdateRate = 3);
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(FPSProfilerService target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(FPSProfilerService functionality)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
            }
        }
    }
}
