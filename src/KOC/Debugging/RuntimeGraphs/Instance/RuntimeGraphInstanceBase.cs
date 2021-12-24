using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance
{
    public abstract class
        RuntimeGraphInstanceBase<TGraph, TManager, TMonitor, TText, TSettings> : AppalachiaBehaviour<>
        where TGraph : RuntimeGraphInstance<TGraph, TManager, TMonitor, TText, TSettings>
        where TManager : RuntimeGraphInstanceManager<TGraph, TManager, TMonitor, TText, TSettings>
        where TMonitor : RuntimeGraphInstanceMonitor<TGraph, TManager, TMonitor, TText, TSettings>
        where TText : RuntimeGraphInstanceText<TGraph, TManager, TMonitor, TText, TSettings>
        where TSettings : IRuntimeGraphSettings
    {
        #region Fields and Autoproperties

        protected RectTransform rectTransform;

        #endregion

        protected abstract TSettings settings { get; }

        protected GraphyManager graphyManager => GraphyManager.instance;

        protected RuntimeGraphSettings allSettings => graphyManager.settings;

        public virtual void InitializeParameters()
        {
        }

        public virtual void UpdateParameters()
        {
        }
    }
}
