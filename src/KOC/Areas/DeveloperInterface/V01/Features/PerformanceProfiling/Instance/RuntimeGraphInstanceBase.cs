using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Settings;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Instance
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
            When.Behaviour(instance)
                .AndBehaviour<RuntimeGraphManager>()
                .AreAvailableThen(
                     (thisInstance, runtimeGraphManager) =>
                     {
                         _runtimeGraphManager = runtimeGraphManager;

                         thisInstance.InitializeParameters();
                     }
                 );
        }

        #region Static Fields and Autoproperties

        private static RuntimeGraphManager _runtimeGraphManager;

        #endregion

        #region Fields and Autoproperties

        protected RectTransform rectTransform;

        #endregion

        protected abstract TSettings settings { get; }

        /// <inheritdoc />
        public override bool FullyInitialized => base.FullyInitialized && (_runtimeGraphManager != null);

        protected RuntimeGraphManager RuntimeGraphManager => _runtimeGraphManager;

        protected RuntimeGraphSettings allSettings => RuntimeGraphManager.settings;

        public virtual void InitializeParameters()
        {
        }

        public virtual void UpdateParameters()
        {
        }
    }
}
