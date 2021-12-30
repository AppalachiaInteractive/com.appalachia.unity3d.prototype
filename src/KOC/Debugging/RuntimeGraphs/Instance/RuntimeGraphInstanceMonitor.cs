using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;
using Appalachia.Utility.Async;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance
{
    public abstract class
        RuntimeGraphInstanceMonitor<TGraph, TManager, TMonitor, TText, TSettings> : RuntimeGraphInstanceBase<
            TGraph, TManager, TMonitor, TText, TSettings, TMonitor>
        where TGraph : RuntimeGraphInstance<TGraph, TManager, TMonitor, TText, TSettings>
        where TManager : RuntimeGraphInstanceManager<TGraph, TManager, TMonitor, TText, TSettings>
        where TMonitor : RuntimeGraphInstanceMonitor<TGraph, TManager, TMonitor, TText, TSettings>
        where TText : RuntimeGraphInstanceText<TGraph, TManager, TMonitor, TText, TSettings>
        where TSettings : IRuntimeGraphSettings
    {
        #region Fields and Autoproperties

        protected TManager manager;

        #endregion

        protected virtual void AfterInitialize()
        {
        }

        protected virtual void BeforeInitialize()
        {
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                if (manager == null)
                {
                    manager = GetComponent<TManager>();
                }

                BeforeInitialize();

                InitializeParameters();

                AfterInitialize();
            }
        }

        #region Profiling

        private const string _PRF_PFX =
            nameof(RuntimeGraphInstanceMonitor<TGraph, TManager, TMonitor, TText, TSettings>) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
