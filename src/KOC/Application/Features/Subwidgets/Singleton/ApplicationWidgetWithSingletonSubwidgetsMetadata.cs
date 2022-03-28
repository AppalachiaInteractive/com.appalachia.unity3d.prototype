using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton
{
    public abstract class ApplicationWidgetWithSingletonSubwidgetsMetadata<
        TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet,
        TIService, TIWidget, TManager> : ApplicationWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
        TFunctionalitySet, TIService, TIWidget, TManager>
        where TISubwidget : class, IApplicationSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, IApplicationSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TWidgetMetadata : ApplicationWidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata,
            TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
        where TWidget : ApplicationWidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>, TIWidget
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
            TManager>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>
        where TFunctionalitySet : FeatureFunctionalitySet<TIService, TIWidget>, new()
        where TIService : IApplicationService
        where TIWidget : IApplicationWidget
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>, IApplicationFunctionalityManager
    {
        protected override void SubscribeResponsiveComponents(TWidget widget)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(widget);

                for (var index = 0; index < widget.Subwidgets.Count; index++)
                {
                    var subwidget = widget.Subwidgets[index];
                    subwidget.Metadata.SubscribeResponsiveComponents(subwidget);
                }
            }
        }

        protected override void SuspendResponsiveComponents(TWidget widget)
        {
            using (_PRF_SuspendResponsiveComponents.Auto())
            {
                base.SuspendResponsiveComponents(widget);

                for (var index = 0; index < widget.Subwidgets.Count; index++)
                {
                    var subwidget = widget.Subwidgets[index];
                    subwidget.Metadata.SuspendResponsiveComponents(subwidget);
                }
            }
        }

        protected override void UnsuspendResponsiveComponents(TWidget widget)
        {
            using (_PRF_UnsuspendResponsiveComponents.Auto())
            {
                base.UnsuspendResponsiveComponents(widget);

                for (var index = 0; index < widget.Subwidgets.Count; index++)
                {
                    var subwidget = widget.Subwidgets[index];
                    subwidget.Metadata.UnsuspendResponsiveComponents(subwidget);
                }
            }
        }

        protected override void OnApply(TWidget widget)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(widget);

                for (var index = 0; index < widget.Subwidgets.Count; index++)
                {
                    var subwidget = widget.Subwidgets[index];

                    subwidget.ApplyMetadata();
                }
            }
        }
    }
}
