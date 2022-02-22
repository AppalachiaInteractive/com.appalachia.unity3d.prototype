using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Appalachia.Utility.Extensions;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01
{
    public partial class DeveloperInterfaceManager_V01
    {
        #region Nested type: Feature

        public abstract class Feature<TFeature, TFeatureMetadata> :
            AreaFeature<TFeature, TFeatureMetadata, DeveloperInterfaceManager_V01,
                DeveloperInterfaceMetadata_V01>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata :
            DeveloperInterfaceMetadata_V01.FeatureMetadata<TFeature, TFeatureMetadata>
        {
            public override void SortWidgets()
            {
                using (_PRF_SortWidgets.Auto())
                {
                    base.SortWidgets();
                    Manager.unscaledWidgetObject.transform.SortChildren();
                }
            }
        }

        #endregion

        #region Nested type: Service

        public abstract class Service<TService, TServiceMetadata, TFeature, TFeatureMetadata> : AreaService<
            TService, TServiceMetadata, TFeature, TFeatureMetadata, DeveloperInterfaceManager_V01,
            DeveloperInterfaceMetadata_V01>
            where TService : Service<TService, TServiceMetadata, TFeature, TFeatureMetadata>
            where TServiceMetadata : DeveloperInterfaceMetadata_V01.ServiceMetadata<TService, TServiceMetadata
                , TFeature, TFeatureMetadata>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata :
            DeveloperInterfaceMetadata_V01.FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: Widget

        [CallStaticConstructorInEditor]
        public abstract class Widget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata> : AreaWidget<
            TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, DeveloperInterfaceManager_V01,
            DeveloperInterfaceMetadata_V01>
            where TWidget : Widget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata :
            DeveloperInterfaceMetadata_V01.FeatureMetadata<TFeature, TFeatureMetadata>
        {
            protected override GameObject GetWidgetParentObject()
            {
                using (_PRF_GetWidgetParentObject.Auto())
                {
                    return Manager.GetWidgetParentObject(metadata.inUnscaledView);
                }
            }
        }

        #endregion
    }
}
