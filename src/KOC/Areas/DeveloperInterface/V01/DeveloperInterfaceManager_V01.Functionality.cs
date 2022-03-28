using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Instanced;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Instanced.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Appalachia.Utility.Extensions;
using UnityEngine;
using MANAGER = Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.DeveloperInterfaceManager_V01;
using METADATA = Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.DeveloperInterfaceMetadata_V01;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01
{
    public partial class DeveloperInterfaceManager_V01
    {
        #region Nested type: Feature

        public abstract class Feature<TFeature, TFeatureMetadata> : AreaFeature<TFeature, TFeatureMetadata,
            MANAGER, METADATA>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : METADATA.FeatureMetadata<TFeature, TFeatureMetadata>
        {
            public override void SortWidgets()
            {
                using (_PRF_SortWidgets.Auto())
                {
                    base.SortWidgets();
                    Manager.UnscaledWidgetObject.transform.SortChildren();
                }
            }
        }

        #endregion

        #region Nested type: InstancedSubwidget

        public abstract class InstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata,
                                                 TWidget, TWidgetMetadata, TFeature, TFeatureMetadata> :
            AreaInstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TSubwidget : InstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata,
                TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidget, IEnableNotifier
            where TISubwidget : class, IAreaInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
            where TISubwidgetMetadata : class, IAreaInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TSubwidgetMetadata :
            METADATA.InstancedSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata,
                TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidgetMetadata,
            IAreaInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TWidget : WidgetWithInstancedSubwidgets<TSubwidget, TSubwidgetMetadata, TISubwidget,
                TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : METADATA.WidgetWithInstancedSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata,
                TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : METADATA.FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: Service

        public abstract class Service<TService, TServiceMetadata, TFeature, TFeatureMetadata> : AreaService<TService,
            TServiceMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TService : Service<TService, TServiceMetadata, TFeature, TFeatureMetadata>
            where TServiceMetadata : METADATA.ServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : METADATA.FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: SingletonSubwidget

        [CallStaticConstructorInEditor]
        public abstract class SingletonSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata,
                                                 TWidget, TWidgetMetadata, TFeature, TFeatureMetadata> :
            AreaSingletonSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TSubwidget :
            SingletonSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidget,
            IAreaSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
            where TSubwidgetMetadata :
            METADATA.SingletonSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata,
                TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidgetMetadata,
            IAreaSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TISubwidget : class, IAreaSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
            where TISubwidgetMetadata : class, IAreaSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TWidget : WidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata>
            where TWidgetMetadata : METADATA.WidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata,
                TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : METADATA.FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: Widget

        [CallStaticConstructorInEditor]
        public abstract class Widget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata> : AreaWidget<TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TWidget : Widget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : METADATA.WidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : METADATA.FeatureMetadata<TFeature, TFeatureMetadata>
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

        #region Nested type: WidgetWithInstancedSubwidgets

        public abstract class WidgetWithInstancedSubwidgets<TSubwidget, TSubwidgetMetadata, TISubwidget,
                                                            TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
                                                            TFeatureMetadata> : AreaWidgetWithInstancedSubwidgets<
            TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
            TFeatureMetadata, MANAGER, METADATA>
            where TSubwidget : InstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata,
                TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidget, IEnableNotifier
            where TISubwidget : class, IAreaInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
            where TISubwidgetMetadata : class, IAreaInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TSubwidgetMetadata :
            METADATA.InstancedSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata,
                TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidgetMetadata,
            IAreaInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TWidget : WidgetWithInstancedSubwidgets<TSubwidget, TSubwidgetMetadata, TISubwidget,
                TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : METADATA.WidgetWithInstancedSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata,
                TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : METADATA.FeatureMetadata<TFeature, TFeatureMetadata>
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

        #region Nested type: WidgetWithSingletonSubwidgets

        public abstract class WidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata,
                                                            TFeature, TFeatureMetadata> :
            AreaWidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
                TFeatureMetadata, MANAGER, METADATA>
            where TISubwidget : class, IAreaSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
            where TISubwidgetMetadata : class, IAreaSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TWidget : WidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata>
            where TWidgetMetadata : METADATA.WidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata,
                TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : METADATA.FeatureMetadata<TFeature, TFeatureMetadata>
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
