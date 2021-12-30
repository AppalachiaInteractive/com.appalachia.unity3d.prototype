using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance
{
    public abstract class
        RuntimeGraphInstanceText<TGraph, TManager, TMonitor, TText, TSettings> : RuntimeGraphInstanceBase<
            TGraph, TManager, TMonitor, TText, TSettings, TText>
        where TGraph : RuntimeGraphInstance<TGraph, TManager, TMonitor, TText, TSettings>
        where TManager : RuntimeGraphInstanceManager<TGraph, TManager, TMonitor, TText, TSettings>
        where TMonitor : RuntimeGraphInstanceMonitor<TGraph, TManager, TMonitor, TText, TSettings>
        where TText : RuntimeGraphInstanceText<TGraph, TManager, TMonitor, TText, TSettings>
        where TSettings : IRuntimeGraphSettings
    {
        #region Fields and Autoproperties

        protected TManager manager;
        protected TMonitor monitor;
        protected int frameCount;
        protected float deltaTime;

        #endregion

        protected abstract bool ShouldUpdate { get; }

        #region Event Functions

        protected void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (!DependenciesAreReady)
                {
                    return;
                }

                deltaTime += Time.unscaledDeltaTime;
                frameCount++;

                if (deltaTime > (1f / settings.TextUpdateRate))
                {
                    UpdateText();

                    frameCount = 0;
                    deltaTime = 0f;
                }
            }
        }

        #endregion

        protected abstract void UpdateText();

        protected virtual void AfterInitialize()
        {
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                if (monitor == null)
                {
                    monitor = GetComponent<TMonitor>();
                }

                if (manager == null)
                {
                    manager = GetComponent<TManager>();
                }

                InitializeParameters();
            }
        }

        #region Profiling

        private const string _PRF_PFX =
            nameof(RuntimeGraphInstanceText<TGraph, TManager, TMonitor, TText, TSettings>) + ".";

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
