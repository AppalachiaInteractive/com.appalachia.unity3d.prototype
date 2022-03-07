using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets
{
    public abstract class ApplicationSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata,
                                                       TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
                                                       TIWidget, TManager> : AppalachiaObject<TSubwidgetMetadata>
        where TWidget : ApplicationWidgetWithSubwidgets<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata,
            TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>, TIWidget
        where TWidgetMetadata : ApplicationWidgetWithSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
        where TSubwidget :
        ApplicationSubwidget<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TFunctionalitySet, TIService, TIWidget, TManager>, IEnableNotifier
        where TSubwidgetMetadata : ApplicationSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata
            , TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
            TManager>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>
        where TFunctionalitySet : FeatureFunctionalitySet<TIService, TIWidget>, new()
        where TIService : IApplicationService
        where TIWidget : IApplicationWidget
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>, IApplicationFunctionalityManager
    {
        #region Fields and Autoproperties

        [PropertyOrder(-500)]
        [SerializeField]
        protected bool showAll;

        #endregion

        public virtual void SubscribeResponsiveComponents(TSubwidget functionality)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                Changed.Event += functionality.UpdateSubwidget;
            }
        }

        #region Profiling

        protected static readonly ProfilerMarker _PRF_SubscribeResponsiveComponents =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeResponsiveComponents));

        #endregion
    }
}
