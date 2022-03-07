using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Controlled;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Appalachia.Utility.Extensions;
using UnityEngine;
using MANAGER = Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.DeveloperInterfaceManager_V01;
using METADATA = Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.DeveloperInterfaceMetadata_V01;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01
{
    public partial class DeveloperInterfaceManager_V01
    {
        #region Nested type: ControlledSubwidget

        public abstract class ControlledSubwidget<TSubwidget, TWidget, TWidgetMetadata,
                                                  TFeature, TFeatureMetadata> : AreaControlledSubwidget<
            TSubwidget, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TSubwidget : ControlledSubwidget<TSubwidget, TWidget, TWidgetMetadata, TFeature,
                TFeatureMetadata>
            where TWidget :
            WidgetWithControlledSubwidgets<TSubwidget, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>,
            IAreaWidget
            where TWidgetMetadata : METADATA.WidgetWithControlledSubwidgetsMetadata<TSubwidget, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : METADATA.FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

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
                    Manager.unscaledWidgetObject.transform.SortChildren();
                }
            }
        }

        #endregion

        #region Nested type: Service

        public abstract class Service<TService, TServiceMetadata, TFeature, TFeatureMetadata> : AreaService<
            TService, TServiceMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TService : Service<TService, TServiceMetadata, TFeature, TFeatureMetadata>
            where TServiceMetadata : METADATA.ServiceMetadata<TService, TServiceMetadata, TFeature,
                TFeatureMetadata>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : METADATA.FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: SingletonSubwidget

        [CallStaticConstructorInEditor]
        public abstract class SingletonSubwidget<TSubwidget, TISubwidget, TSubwidgetMetadata,
                                                 TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
                                                 TFeatureMetadata> : AreaSingletonSubwidget<TSubwidget,
            TISubwidget, TSubwidgetMetadata, TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
            TFeatureMetadata, MANAGER, METADATA>
            where TISubwidget : class, IAreaSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
            where TISubwidgetMetadata : class,
            IAreaSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TSubwidget :
            SingletonSubwidget<TSubwidget, TISubwidget, TSubwidgetMetadata, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidget
            where TSubwidgetMetadata :
            METADATA.SingletonSubwidgetMetadata<TSubwidget, TISubwidget, TSubwidgetMetadata,
                TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>,
            TISubwidgetMetadata
            where TWidget : WidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : METADATA.WidgetWithSingletonSubwidgetsMetadata<TISubwidget,
                TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : METADATA.FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: Subwidget

        public abstract class Subwidget<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
                                        TFeatureMetadata> : AreaSubwidget<TSubwidget, TSubwidgetMetadata,
            TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TSubwidget : Subwidget<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata>, IEnableNotifier
            where TSubwidgetMetadata : METADATA.SubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidget : WidgetWithSubwidgets<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata>
            where TWidgetMetadata : METADATA.WidgetWithSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata,
                TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : METADATA.FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: Widget

        [CallStaticConstructorInEditor]
        public abstract class Widget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata> : AreaWidget<
            TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TWidget : Widget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : METADATA.WidgetMetadata<TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata>
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

        #region Nested type: WidgetWithControlledSubwidgets

        public abstract class WidgetWithControlledSubwidgets<TSubwidget, TWidget,
                                                             TWidgetMetadata, TFeature, TFeatureMetadata> :
            AreaWidgetWithControlledSubwidgets<TSubwidget, TWidget, TWidgetMetadata, TFeature,
                TFeatureMetadata, MANAGER, METADATA>
            where TSubwidget : ControlledSubwidget<TSubwidget, TWidget, TWidgetMetadata, TFeature,
                TFeatureMetadata>
            where TWidget :
            WidgetWithControlledSubwidgets<TSubwidget, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>,
            IAreaWidget
            where TWidgetMetadata : METADATA.WidgetWithControlledSubwidgetsMetadata<TSubwidget, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
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

        public abstract class WidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
                                                            TWidgetMetadata, TFeature, TFeatureMetadata> :
            AreaWidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TISubwidget : class, IAreaSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
            where TISubwidgetMetadata : class,
            IAreaSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TWidget : WidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : METADATA.WidgetWithSingletonSubwidgetsMetadata<TISubwidget,
                TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
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

        #region Nested type: WidgetWithSubwidgets

        public abstract class WidgetWithSubwidgets<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata,
                                                   TFeature, TFeatureMetadata> : AreaWidgetWithSubwidgets<
            TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, MANAGER,
            METADATA>
            where TSubwidget : Subwidget<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata>, IEnableNotifier
            where TSubwidgetMetadata : METADATA.SubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidget : WidgetWithSubwidgets<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata>
            where TWidgetMetadata : METADATA.WidgetWithSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata,
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