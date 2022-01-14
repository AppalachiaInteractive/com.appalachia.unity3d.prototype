using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Settings;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Instance
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
        static RuntimeGraphInstanceText()
        {
            RegisterDependency<TMonitor>(i => monitor = i);
            RuntimeGraphInstanceManager<TGraph, TManager, TMonitor, TText, TSettings>.InstanceAvailable +=
                i => manager = i;
        }

        #region Static Fields and Autoproperties

        protected static TManager manager;
        protected static TMonitor monitor;

        #endregion

        #region Fields and Autoproperties

        protected int frameCount;
        protected float deltaTime;

        #endregion

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
    }
}
