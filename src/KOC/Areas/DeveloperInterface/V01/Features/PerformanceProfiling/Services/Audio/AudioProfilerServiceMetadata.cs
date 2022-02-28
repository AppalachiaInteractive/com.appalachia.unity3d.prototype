using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Services.Audio
{
    public sealed class AudioProfilerServiceMetadata : DeveloperInterfaceMetadata_V01.ServiceMetadata<
        AudioProfilerService, AudioProfilerServiceMetadata, PerformanceProfiingFeature,
        PerformanceProfiingFeatureMetadata>
    {
        #region Constants and Static Readonly

        private static readonly int[] spectrumSizes = { 64, 128, 256, 512, 1024, 2048, 4096, 8192 };

        #endregion

        #region Fields and Autoproperties

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public Color audioGraphColor;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [PropertyRange(10, 300)]
        public int graphResolution;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [PropertyRange(1, 10)]
        public int textUpdateRate;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public FFTWindow FFTWindow;

        [ValueDropdown(nameof(spectrumSizes))]
        [PropertyTooltip("Must be a power of 2 and between 64-8192")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public int spectrumSize;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(audioGraphColor), () => audioGraphColor = Color.white);
                initializer.Do(this, nameof(graphResolution), () => graphResolution = 81);
                initializer.Do(this, nameof(textUpdateRate),  () => textUpdateRate = 3);
                initializer.Do(this, nameof(FFTWindow),       () => FFTWindow = FFTWindow.Blackman);
                initializer.Do(this, nameof(spectrumSize),    () => spectrumSize = 512);
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(AudioProfilerService target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(AudioProfilerService functionality)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
            }
        }
    }
}
