using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Util;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Audio
{
    public class G_AudioText : RuntimeGraphInstanceText<G_AudioGraph, G_AudioManager, G_AudioMonitor,
        G_AudioText, RuntimeGraphAudioSettings>
    {
        #region Fields and Autoproperties

        [SerializeField] private Text m_DBText;

        protected override RuntimeGraphAudioSettings settings => allSettings.audio;

        #endregion

        protected override bool ShouldUpdate => monitor.SpectrumDataAvailable;

        protected override void AfterInitialize()
        {
            using (_PRF_AfterInitialize.Auto())
            {
                G_IntString.Init(-80, 0); // dB range
            }
        }

        protected override void UpdateText()
        {
            using (_PRF_UpdateText.Auto())
            {
                m_DBText.text = Mathf.Clamp((int)monitor.MaxDB, -80, 0).ToStringNonAlloc();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(G_AudioText) + ".";

        private static readonly ProfilerMarker _PRF_UpdateParameters =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeParameters));

        private static readonly ProfilerMarker _PRF_AfterInitialize =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialize));

        private static readonly ProfilerMarker
            _PRF_UpdateText = new ProfilerMarker(_PRF_PFX + nameof(Update));

        #endregion
    }
}
