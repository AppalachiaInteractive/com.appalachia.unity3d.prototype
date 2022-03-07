using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets
{
    public abstract class ApplicationWidgetWithSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TWidget,
                                                                  TWidgetMetadata, TFeature, TFeatureMetadata,
                                                                  TFunctionalitySet, TIService, TIWidget,
                                                                  TManager> : ApplicationWidgetMetadata<
        TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
        TManager>
        where TWidget : ApplicationWidgetWithSubwidgets<TSubwidget, TSubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>,
        TIWidget
        where TWidgetMetadata : ApplicationWidgetWithSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata,
            TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
            TManager>
        where TSubwidget : ApplicationSubwidget<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata,
            TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>, IEnableNotifier
        where TSubwidgetMetadata : ApplicationSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
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
        protected override void UpdateFunctionalityInternal(TWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);

                for (var index = 0; index < widget.Subwidgets.Count; index++)
                {
                    var subwidget = widget.Subwidgets[index];

                    subwidget.UpdateSubwidget();
                }
            }
        }

        protected override void SubscribeResponsiveComponents(TWidget widget)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(widget);
            
                for (var index = 0; index < widget.Subwidgets.Count; index++)
                {
                    var subwidget = widget.Subwidgets[index];
                    var data = subwidget.Metadata;

                    data.SubscribeResponsiveComponents(subwidget);
                }
            }
        }
    }
}
