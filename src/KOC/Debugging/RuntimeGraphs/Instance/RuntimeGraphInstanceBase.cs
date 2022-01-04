using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance
{
    [CallStaticConstructorInEditor]
    public abstract class
        RuntimeGraphInstanceBase<TGraph, TManager, TMonitor, TText, TSettings, T> : AppalachiaBehaviour<T>
        where TGraph : RuntimeGraphInstance<TGraph, TManager, TMonitor, TText, TSettings>
        where TManager : RuntimeGraphInstanceManager<TGraph, TManager, TMonitor, TText, TSettings>
        where TMonitor : RuntimeGraphInstanceMonitor<TGraph, TManager, TMonitor, TText, TSettings>
        where TText : RuntimeGraphInstanceText<TGraph, TManager, TMonitor, TText, TSettings>
        where TSettings : IRuntimeGraphSettings
        where T : RuntimeGraphInstanceBase<TGraph, TManager, TMonitor, TText, TSettings, T>
    {
        static RuntimeGraphInstanceBase()
        {
            GraphyManager.InstanceAvailable += i => _graphyManager = i;
        }

        #region Static Fields and Autoproperties

        private static GraphyManager _graphyManager;

        #endregion

        #region Fields and Autoproperties

        protected RectTransform rectTransform;

        #endregion

        protected abstract TSettings settings { get; }

        protected GraphyManager graphyManager => _graphyManager;

        protected RuntimeGraphSettings allSettings => graphyManager.settings;

        public virtual void InitializeParameters()
        {
        }

        public virtual void UpdateParameters()
        {
        }
    }
}
