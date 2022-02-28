using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Settings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Instance
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
        static RuntimeGraphInstanceMonitor()
        {
            When.Behaviour<TManager>().IsAvailableThen(i => manager = i);
        }

        #region Static Fields and Autoproperties

        protected static TManager manager;

        #endregion
    }
}
