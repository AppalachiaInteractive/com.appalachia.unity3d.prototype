using System;
using Appalachia.Core.Objects.Components.Contracts;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton
{
    public abstract class ApplicationSingletonSubwidgetMetadata<TSubwidget, TISubwidget, TSubwidgetMetadata,
                                                                TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
                                                                TFeatureMetadata, TFunctionalitySet, TIService,
                                                                TIWidget, TManager> :
        ApplicationFunctionalityMetadata<TSubwidget, TSubwidgetMetadata, TManager>,
        ISingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TSubwidget : ApplicationSingletonSubwidget<TSubwidget, TISubwidget, TSubwidgetMetadata,
            TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>, TISubwidget
        where TISubwidget : class, ISingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TSubwidgetMetadata : ApplicationSingletonSubwidgetMetadata<TSubwidget, TISubwidget, TSubwidgetMetadata,
            TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>, TISubwidgetMetadata
        where TISubwidgetMetadata : class, ISingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TWidget : ApplicationWidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>, TIWidget
        where TWidgetMetadata : ApplicationWidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata,
            TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
            TManager>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>
        where TFunctionalitySet : FeatureFunctionalitySet<TIService, TIWidget>, new()
        where TIService : IApplicationService
        where TIWidget : IApplicationWidget
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>, IApplicationFunctionalityManager
    {
        protected override void SubscribeResponsiveComponents(TSubwidget functionality)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                Changed.Event += functionality.UpdateSubwidget;
            }
        }

        protected override void UpdateFunctionalityInternal(TSubwidget functionality)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                functionality.UpdateSubwidget();
            }
        }

        #region ISingletonSubwidgetMetadata<TISubwidget,TISubwidgetMetadata> Members

        void ISingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>.SubscribeResponsiveComponents(
            TISubwidget functionality)
        {
            SubscribeResponsiveComponents(functionality as TSubwidget);
        }

        #endregion
    }
}
