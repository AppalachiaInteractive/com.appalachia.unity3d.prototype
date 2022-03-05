using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Settings;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling
{
    public class PerformanceProfilingFeatureMetadata : DeveloperInterfaceMetadata_V01.FeatureMetadata<
        PerformanceProfilingFeature, PerformanceProfilingFeatureMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public bool enableOnStartup;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public bool keepAlive;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public bool background;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public Color backgroundColor;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public ModulePosition graphModulePosition;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public ModulePreset modulePresetState;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(enableOnStartup),     () => enableOnStartup = true);
                initializer.Do(this, nameof(keepAlive),           () => keepAlive = true);
                initializer.Do(this, nameof(background),          () => background = true);
                initializer.Do(this, nameof(backgroundColor),     () => backgroundColor = new(0, 0, 0, 0.3f));
                initializer.Do(this, nameof(graphModulePosition), () => graphModulePosition = ModulePosition.TOP_RIGHT);
                initializer.Do(
                    this,
                    nameof(modulePresetState),
                    () => modulePresetState = ModulePreset.FPS_FULL_RAM_FULL_AUDIO_FULL
                );
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(PerformanceProfilingFeature target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(PerformanceProfilingFeature functionality)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
            }
        }
    }
}
