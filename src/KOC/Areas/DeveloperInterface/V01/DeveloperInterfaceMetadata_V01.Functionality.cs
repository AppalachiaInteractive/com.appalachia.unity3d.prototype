using Appalachia.CI.Constants;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01
{
    public partial class DeveloperInterfaceMetadata_V01
    {
        #region Nested type: FeatureMetadata

        public abstract class FeatureMetadata<TFeature, TFeatureMetadata> : AreaFeatureMetadata<TFeature,
            TFeatureMetadata, DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
            where TFeature : DeveloperInterfaceManager_V01.Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: ServiceMetadata

        public abstract class ServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata> :
            AreaServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata,
                DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
            where TService : DeveloperInterfaceManager_V01.Service<TService, TServiceMetadata, TFeature,
                TFeatureMetadata>
            where TServiceMetadata : ServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata>
            where TFeature : DeveloperInterfaceManager_V01.Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: WidgetMetadata

        public abstract class WidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata> :
            AreaWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
                DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
            where TWidget : DeveloperInterfaceManager_V01.Widget<TWidget, TWidgetMetadata, TFeature,
                TFeatureMetadata>
            where TWidgetMetadata : WidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : DeveloperInterfaceManager_V01.Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : FeatureMetadata<TFeature, TFeatureMetadata>
        {
            #region Fields and Autoproperties

            [FoldoutGroup(APPASTR.GroupNames.Common)]
            [OnValueChanged(nameof(OnChanged))]
            public bool inUnscaledView;

            #endregion
        }

        #endregion
    }
}
