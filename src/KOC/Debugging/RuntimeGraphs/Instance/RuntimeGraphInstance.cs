using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance
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
        #region Fields and Autoproperties

        [NonSerialized] private bool _initialized;

        protected TManager manager;
        protected TMonitor monitor;

        #endregion

        public abstract GameObject graphParent { get; }

        protected abstract bool ShouldUpdate { get; }

        #region Event Functions

        protected void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (!DependenciesAreReady || !FullyInitialized)
                {
                    return;
                }

                if (!_initialized)
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

        protected virtual void AfterInitialize()
        {
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                monitor = await initializer.Get<TMonitor>(this);
                manager = await initializer.Get<TManager>(this);

                AfterInitialize();

                InitializeParameters();

                CreatePoints();

                _initialized = true;
            }
        }

        #region Profiling

        private const string _PRF_PFX =
            nameof(RuntimeGraphInstance<TGraph, TManager, TMonitor, TText, TSettings>) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        #endregion
    }
}
