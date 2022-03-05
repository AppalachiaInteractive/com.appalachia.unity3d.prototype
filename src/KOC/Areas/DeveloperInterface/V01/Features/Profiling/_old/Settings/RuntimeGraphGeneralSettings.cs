using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Objects.Root;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Settings
{
    [DoNotReorderFields, Serializable]
    public class RuntimeGraphGeneralSettings : AppalachiaSimpleBase
    {
        #region Fields and Autoproperties

        public bool enableOnStartup;
        public bool keepAlive;
        public bool background;
        public Color backgroundColor;
        public ModulePosition graphModulePosition;
        public ModulePreset modulePresetState;

        #endregion

        public void Reset()
        {
            enableOnStartup = true;
            keepAlive = true;
            background = true;
            backgroundColor = new(0, 0, 0, 0.3f);
            graphModulePosition = ModulePosition.TOP_RIGHT;
            modulePresetState = ModulePreset.FPS_FULL_RAM_FULL_AUDIO_FULL;
        }
    }
}
