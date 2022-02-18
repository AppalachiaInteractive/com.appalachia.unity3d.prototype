using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Application.Features.Services
{
    [CallStaticConstructorInEditor]
    public abstract partial class ApplicationService<TService, TServiceMetadata, TFeature, TFeatureMetadata,
                                                     TFunctionalitySet, TIService, TIWidget, TManager> :
        ApplicationFunctionality<TService, TServiceMetadata, TManager>,
        IApplicationService
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
        #region Constants and Static Readonly

        protected const string GROUP_NAME = "Service";

        #endregion

        static ApplicationService()
        {
            /*
             * intentional use of base class ApplicationService<> to ensure that this callback
             * runs before other TService callbacks.
             */
            RegisterInstanceCallbacks
               .For<ApplicationService<TService, TServiceMetadata, TFeature, TFeatureMetadata,
                    TFunctionalitySet, TIService, TIWidget, TManager>>()
               .When.Behaviour(instance)
               .AndBehaviour<TFeature>()
               .AndBehaviour<TManager>()
               .AreAvailableThen(
                    (thisInstance, feature, _) =>
                    {
                        _feature = feature;

                        var parentObject = _feature.GetServiceParentObject();

                        thisInstance.transform.SetParent(parentObject.transform);
                    }
                );
        }

        #region Static Fields and Autoproperties

        private static TFeature _feature;

        #endregion

        public static TFeature Feature => _feature;

        /// <inheritdoc />
        protected override async AppaTask DelayEnabling()
        {
            await base.DelayEnabling();

            await AppaTask.WaitUntil(() => Manager != null);
            await AppaTask.WaitUntil(() => Manager.FullyInitialized);
        }
    }
}
