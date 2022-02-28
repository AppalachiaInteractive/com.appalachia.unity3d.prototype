using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features
{
    public abstract class ApplicationFeatureMetadata<TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
                                                     TIWidget, TManager> :
        ApplicationFunctionalityMetadata<TFeature, TFeatureMetadata, TManager>,
        IApplicationFeatureMetadata<TFeature, TFeatureMetadata>
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
        #region Fields and Autoproperties

        [BoxGroup("Startup")] 
        [OnValueChanged(nameof(OnChanged))]
        public bool startsEnabled;

        #endregion
    }
}
