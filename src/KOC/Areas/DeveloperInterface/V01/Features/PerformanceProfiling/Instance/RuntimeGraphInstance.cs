using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Settings;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Instance
{
    public abstract class
        RuntimeGraphInstance<TGraph, TManager, TMonitor, TText, TSettings> : RuntimeGraphInstanceBase<TGraph,
            TManager, TMonitor, TText, TSettings, TGraph>
        where TGraph : RuntimeGraphInstance<TGraph, TManager, TMonitor, TText, TSettings>
        where TManager : RuntimeGraphInstanceManager<TGraph, TManager, TMonitor, TText, TSettings>
        where TMonitor : RuntimeGraphInstanceMonitor<TGraph, TManager, TMonitor, TText, TSettings>
        where TText : RuntimeGraphInstanceText<TGraph, TManager, TMonitor, TText, TSettings>
        where TSettings : IRuntimeGraphSettings
    {
        static RuntimeGraphInstance()
        {
            RegisterDependency<TMonitor>(i => monitor = i);

            When.Behaviour(instance)
                .AndBehaviour<RuntimeGraphManager>()
                .AndBehaviour<TManager>()
                .AreAvailableThen(
                     (thisInstance, graphManager, instanceManager) =>
                     {
                         manager = instanceManager;

                         thisInstance.CreatePoints();
                     }
                 );
        }

        #region Static Fields and Autoproperties

        protected static TManager manager;
        protected static TMonitor monitor;

        #endregion

        public abstract GameObject graphParent { get; }

        protected abstract bool ShouldUpdate { get; }

        #region Event Functions

        protected void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                if (ShouldUpdate)
                {
                    UpdateGraph();
                }
            }
        }

        #endregion

        /// <summary>
        ///     Creates the points for the graph/s.
        /// </summary>
        protected abstract void CreatePoints();

        /// <summary>
        ///     Updates the graph/s.
        /// </summary>
        protected abstract void UpdateGraph();
    }
}
