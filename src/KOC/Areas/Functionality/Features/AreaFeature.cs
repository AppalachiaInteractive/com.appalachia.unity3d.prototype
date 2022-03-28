using System;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Features
{
    [CallStaticConstructorInEditor]
    public abstract class AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata> :
        ApplicationFeature<TFeature, TFeatureMetadata, AreaFeatureFunctionalitySet, IAreaService, IAreaWidget,
            TAreaManager>,
        IAreaFeature
        where TFeature : AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        #region IAreaFeature Members

        public void ForEachService(Action<IAreaService> forEachAction)
        {
            using (_PRF_ForEachService.Auto())
            {
                foreach (var service in FunctionalitySet.Services)
                {
                    forEachAction(service);
                }
            }
        }

        public void ForEachWidget(Action<IAreaWidget> forEachAction)
        {
            using (_PRF_ForEachWidget.Auto())
            {
                foreach (var widget in FunctionalitySet.Widgets)
                {
                    forEachAction(widget);
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_ForEachService =
            new ProfilerMarker(_PRF_PFX + nameof(ForEachService));

        private static readonly ProfilerMarker _PRF_ForEachWidget =
            new ProfilerMarker(_PRF_PFX + nameof(ForEachWidget));

        #endregion
    }
}
