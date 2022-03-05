using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Settings;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Util;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Audio
{
    public class RuntimeGraphAudioText : RuntimeGraphInstanceText<RuntimeGraphAudioGraph,
        RuntimeGraphAudioManager, RuntimeGraphAudioMonitor, RuntimeGraphAudioText, RuntimeGraphAudioSettings>
    {
        #region Fields and Autoproperties

        [SerializeField] private Text m_DBText;

        #endregion

        /// <inheritdoc />
        protected override bool ShouldUpdate => monitor.SpectrumDataAvailable;

        /// <inheritdoc />
        protected override RuntimeGraphAudioSettings settings => allSettings.audio;

        /// <inheritdoc />
        protected override void AfterInitialization()
        {
            using (_PRF_AfterInitialization.Auto())
            {
                G_IntString.Init(-80, 0); // dB range

                base.AfterInitialization();
            }
        }

        /// <inheritdoc />
        protected override void UpdateText()
        {
            using (_PRF_UpdateText.Auto())
            {
                m_DBText.text = Mathf.Clamp((int)monitor.MaxDB, -80, 0).ToStringNonAlloc();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_AfterInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialization));

        private static readonly ProfilerMarker _PRF_UpdateParameters =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeParameters));

        private static readonly ProfilerMarker
            _PRF_UpdateText = new ProfilerMarker(_PRF_PFX + nameof(Update));

        #endregion
    }
}
