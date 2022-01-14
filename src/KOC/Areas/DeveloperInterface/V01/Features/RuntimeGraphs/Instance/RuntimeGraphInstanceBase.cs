using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Settings;
using Appalachia.Utility.Async;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Instance
{
    [CallStaticConstructorInEditor]
    public abstract class
        RuntimeGraphInstanceBase<TGraph, TManager, TMonitor, TText, TSettings,
                                 T> : SingletonAppalachiaBehaviour<T>
        where TGraph : RuntimeGraphInstance<TGraph, TManager, TMonitor, TText, TSettings>
        where TManager : RuntimeGraphInstanceManager<TGraph, TManager, TMonitor, TText, TSettings>
        where TMonitor : RuntimeGraphInstanceMonitor<TGraph, TManager, TMonitor, TText, TSettings>
        where TText : RuntimeGraphInstanceText<TGraph, TManager, TMonitor, TText, TSettings>
        where TSettings : IRuntimeGraphSettings
        where T : RuntimeGraphInstanceBase<TGraph, TManager, TMonitor, TText, TSettings, T>
    {
        static RuntimeGraphInstanceBase()
        {
            RuntimeGraphManager.InstanceAvailable += i => { _runtimeGraphManager = i; };
        }

        #region Static Fields and Autoproperties

        private static RuntimeGraphManager _runtimeGraphManager;

        #endregion

        #region Fields and Autoproperties

        protected RectTransform rectTransform;

        #endregion

        protected abstract TSettings settings { get; }

        public override bool FullyInitialized => base.FullyInitialized && (_runtimeGraphManager != null);

        protected RuntimeGraphManager RuntimeGraphManager => _runtimeGraphManager;

        protected RuntimeGraphSettings allSettings => RuntimeGraphManager.settings;

        public virtual void InitializeParameters()
        {
        }

        public virtual void UpdateParameters()
        {
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            RuntimeGraphManager.InstanceAvailable += _ => InitializeParameters();
        }
    }
}
