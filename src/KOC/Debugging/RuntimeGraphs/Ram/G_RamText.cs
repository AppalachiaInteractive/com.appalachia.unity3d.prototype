using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Util;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Ram
{
    public class G_RamText : RuntimeGraphInstanceText<G_RamGraph, G_RamManager, G_RamMonitor, G_RamText,
        RuntimeGraphRamSettings>
    {
        #region Fields and Autoproperties

        [SerializeField] private Text m_allocatedSystemMemorySizeText;
        [SerializeField] private Text m_reservedSystemMemorySizeText;
        [SerializeField] private Text m_monoSystemMemorySizeText;

        #endregion

        protected override bool ShouldUpdate => true;

        protected override RuntimeGraphRamSettings settings => allSettings.ram;

        public override void InitializeParameters()
        {
            using (_PRF_InitializeParameters.Auto())
            {
                m_allocatedSystemMemorySizeText.color = settings.allocatedRamColor;
                m_reservedSystemMemorySizeText.color = settings.reservedRamColor;
                m_monoSystemMemorySizeText.color = settings.monoRamColor;
            }
        }

        protected override void AfterInitialize()
        {
            using (_PRF_AfterInitialize.Auto())
            {
                G_IntString.Init(0, 16386);
            }
        }

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

        private const string _PRF_PFX = nameof(G_RamText) + ".";

        private static readonly ProfilerMarker _PRF_AfterInitialize =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialize));

        private static readonly ProfilerMarker
            _PRF_UpdateText = new ProfilerMarker(_PRF_PFX + nameof(Update));

        private static readonly ProfilerMarker _PRF_InitializeParameters =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeParameters));

        #endregion
    }
}
