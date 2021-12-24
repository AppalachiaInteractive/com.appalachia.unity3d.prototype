using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings
{
    #region Nested type: AudioSettings

    #endregion

    [DoNotReorderFields]
    public class RuntimeGraphSettings : SingletonAppalachiaObject<RuntimeGraphSettings>
    {
        #region Fields and Autoproperties

        [SerializeField, HideInInspector]
        private bool _first;

        [FoldoutGroup("General")]
        public RuntimeGraphGeneralSettings general;

        [FoldoutGroup("Fps")] public RuntimeGraphFpsSettings fps;
        [FoldoutGroup("Memory")] public RuntimeGraphRamSettings ram;
        [FoldoutGroup("Audio")] public RuntimeGraphAudioSettings audio;

        #endregion

        #region Event Functions

        [Button]
        public void Reset()
        {
            general.Reset();
            fps.Reset();
            ram.Reset();
            audio.Reset();
        }

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                if (_first)
                {
                    general.Reset();
                    ram.Reset();
                    fps.Reset();
                    audio.Reset();
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(RuntimeGraphSettings) + ".";

        #endregion
    }
}
