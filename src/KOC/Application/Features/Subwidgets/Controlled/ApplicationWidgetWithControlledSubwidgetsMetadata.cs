using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Controlled
{
    public abstract class ApplicationWidgetWithControlledSubwidgetsMetadata<
        TSubwidget, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
        TManager> : ApplicationWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet,
        TIService, TIWidget, TManager>
        where TSubwidget :
        ApplicationControlledSubwidget<TSubwidget, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TFunctionalitySet, TIService, TIWidget, TManager>, IEnableNotifier
        where TWidget : ApplicationWidgetWithControlledSubwidgets<TSubwidget, TWidget, TWidgetMetadata, TFeature,
            TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>, TIWidget
        where TWidgetMetadata : ApplicationWidgetWithControlledSubwidgetsMetadata<TSubwidget, TWidget, TWidgetMetadata,
            TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
            TManager>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>
        where TFunctionalitySet : FeatureFunctionalitySet<TIService, TIWidget>, new()
        where TIService : IApplicationService
        where TIWidget : IApplicationWidget
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>, IApplicationFunctionalityManager
    {
        protected override void UpdateFunctionalityInternal(TWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);

                widget.DiscardNullSubwidgets();

                for (var index = 0; index < widget.Subwidgets.Count; index++)
                {
                    var subwidget = widget.Subwidgets[index];

                    subwidget.UpdateSubwidget();
                }
            }
        }
    }
}
