using System;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas
{
    public abstract partial class AreaManager<TManager, TMetadata>
    {
        public void Propogate(Action<IAreaFeature> forEachFeature)
        {
            using (_PRF_Propogate.Auto())
            {
                for (var featureIndex = 0; featureIndex < _featureManager.Functionality.Features.Count; featureIndex++)
                {
                    var feature = _featureManager.Functionality.Features[featureIndex];
                    
                    feature.p
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_Propogate = new ProfilerMarker(_PRF_PFX + nameof(Propogate));

        #endregion
    }
}
