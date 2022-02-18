using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;

namespace Appalachia.Prototype.KOC.Application.Features.Services
{
    public abstract class ApplicationServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata,
                                                     TFunctionalitySet, TIService, TIWidget, TManager> :
        ApplicationFunctionalityMetadata<TService, TServiceMetadata, TManager>,
        IApplicationServiceMetadata<TService>
        where TService : ApplicationService<TService, TServiceMetadata, TFeature, TFeatureMetadata,
            TFunctionalitySet, TIService, TIWidget, TManager>
        where TServiceMetadata : ApplicationServiceMetadata<TService, TServiceMetadata, TFeature,
            TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget
            , TManager>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata, TFunctionalitySet,
            TIService, TIWidget, TManager>
        where TFunctionalitySet : FeatureFunctionalitySet<TIService, TIWidget>, new()
        where TIService : IApplicationService
        where TIWidget : IApplicationWidget
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>,
        IApplicationFunctionalityManager
    {
    }
}
