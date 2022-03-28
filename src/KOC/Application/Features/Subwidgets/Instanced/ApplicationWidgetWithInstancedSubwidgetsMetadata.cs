using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced
{
    public abstract class ApplicationWidgetWithInstancedSubwidgetsMetadata<
        TSubwidget, TSubwidgetMetadata, TISubwidget,
        TISubwidgetMetadata, TWidget, TWidgetMetadata,
        TFeature, TFeatureMetadata, TFunctionalitySet,
        TIService, TIWidget, TManager> : ApplicationWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
        TFunctionalitySet, TIService, TIWidget, TManager>
        where TSubwidget :
        ApplicationInstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>, TISubwidget,
        IApplicationInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TSubwidgetMetadata :
        ApplicationInstancedSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>,
        TISubwidgetMetadata, IApplicationInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TISubwidget : class, IApplicationInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, IApplicationInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TWidget : ApplicationWidgetWithInstancedSubwidgets<TSubwidget, TSubwidgetMetadata, TISubwidget,
            TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
            TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>, TIWidget
        where TWidgetMetadata : ApplicationWidgetWithInstancedSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget,
            TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
            TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
            TManager>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>
        where TFunctionalitySet : FeatureFunctionalitySet<TIService, TIWidget>, new()
        where TIService : IApplicationService
        where TIWidget : IApplicationWidget
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>, IApplicationFunctionalityManager
    {
        protected override void SuspendResponsiveComponents(TWidget widget)
        {
            using (_PRF_SuspendResponsiveComponents.Auto())
            {
                base.SuspendResponsiveComponents(widget);

                for (var index = 0; index < widget.Subwidgets.Count; index++)
                {
                    var subwidget = widget.Subwidgets[index];
                    subwidget.SuspendChanges();
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
                    subwidget.UnsuspendChanges();
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
                    subwidget.SubscribeToChanges(OnChanged);
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
