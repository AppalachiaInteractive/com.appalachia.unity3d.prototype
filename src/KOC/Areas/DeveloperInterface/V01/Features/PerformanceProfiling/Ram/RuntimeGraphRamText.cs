using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Settings;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Util;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Ram
{
    public class RuntimeGraphRamText : RuntimeGraphInstanceText<RuntimeGraphRamGraph, RuntimeGraphRamManager,
        RuntimeGraphRamMonitor, RuntimeGraphRamText, RuntimeGraphRamSettings>
    {
        #region Fields and Autoproperties

        [SerializeField] private Text m_allocatedSystemMemorySizeText;
        [SerializeField] private Text m_reservedSystemMemorySizeText;
        [SerializeField] private Text m_monoSystemMemorySizeText;

        #endregion

        /// <inheritdoc />
        protected override bool ShouldUpdate => true;

        /// <inheritdoc />
        protected override RuntimeGraphRamSettings settings => allSettings.ram;

        /// <inheritdoc />
        public override void InitializeParameters()
        {
            using (_PRF_InitializeParameters.Auto())
            {
                m_allocatedSystemMemorySizeText.color = settings.allocatedRamColor;
                m_reservedSystemMemorySizeText.color = settings.reservedRamColor;
                m_monoSystemMemorySizeText.color = settings.monoRamColor;
            }
        }

        /// <inheritdoc />
        protected override void AfterInitialization()
        {
            using (_PRF_AfterInitialization.Auto())
            {
                G_IntString.Init(0, 16386);

                base.AfterInitialization();
            }
        }

        /// <inheritdoc />
        protected override void UpdateText()
        {
            using (_PRF_UpdateText.Auto())
            {
                // Update allocated, mono and reserved memory
                m_allocatedSystemMemorySizeText.text = ((int)monitor.AllocatedRam).ToStringNonAlloc();
                m_reservedSystemMemorySizeText.text = ((int)monitor.ReservedRam).ToStringNonAlloc();
                m_monoSystemMemorySizeText.text = ((int)monitor.MonoRam).ToStringNonAlloc();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_AfterInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialization));

        private static readonly ProfilerMarker _PRF_InitializeParameters =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeParameters));

        private static readonly ProfilerMarker
            _PRF_UpdateText = new ProfilerMarker(_PRF_PFX + nameof(Update));

        #endregion
    }
}
