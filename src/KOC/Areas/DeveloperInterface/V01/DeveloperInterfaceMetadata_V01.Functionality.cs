using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Controlled;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Sirenix.OdinInspector;
using MANAGER = Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.DeveloperInterfaceManager_V01;
using METADATA = Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.DeveloperInterfaceMetadata_V01;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01
{
    public partial class DeveloperInterfaceMetadata_V01
    {
        #region Nested type: FeatureMetadata

        public abstract class FeatureMetadata<TFeature, TFeatureMetadata> : AreaFeatureMetadata<TFeature,
            TFeatureMetadata, MANAGER, METADATA>
            where TFeature : MANAGER.Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: ServiceMetadata

        public abstract class ServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata> :
            AreaServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TService : MANAGER.Service<TService, TServiceMetadata, TFeature, TFeatureMetadata>
            where TServiceMetadata : ServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata>
            where TFeature : MANAGER.Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: SingletonSubwidgetMetadata

        public abstract class SingletonSubwidgetMetadata<TSubwidget, TISubwidget, TSubwidgetMetadata,
                                                         TISubwidgetMetadata, TWidget, TWidgetMetadata,
                                                         TFeature, TFeatureMetadata> :
            AreaSingletonSubwidgetMetadata<TSubwidget, TISubwidget, TSubwidgetMetadata, TISubwidgetMetadata,
                TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TISubwidget : class, IAreaSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
            where TISubwidgetMetadata : class,
            IAreaSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TSubwidget :
            MANAGER.SingletonSubwidget<TSubwidget, TISubwidget, TSubwidgetMetadata, TISubwidgetMetadata,
                TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidget
            where TSubwidgetMetadata :
            SingletonSubwidgetMetadata<TSubwidget, TISubwidget, TSubwidgetMetadata, TISubwidgetMetadata,
                TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidgetMetadata
            where TWidget : MANAGER.WidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : WidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata,
                TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : MANAGER.Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: SubwidgetMetadata

        public abstract class SubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata,
                                                TFeature, TFeatureMetadata> : AreaSubwidgetMetadata<TSubwidget
            , TSubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TSubwidget :
            MANAGER.Subwidget<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
                TFeatureMetadata>, IEnableNotifier
            where TSubwidgetMetadata : SubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidget : MANAGER.WidgetWithSubwidgets<TSubwidget, TSubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : WidgetWithSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : MANAGER.Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: WidgetMetadata

        public abstract class WidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata> :
            AreaWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TWidget : MANAGER.Widget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : WidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : MANAGER.Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : FeatureMetadata<TFeature, TFeatureMetadata>
        {
            #region Fields and Autoproperties

            [FoldoutGroup(APPASTR.GroupNames.Common)]
            [OnValueChanged(nameof(OnChanged))]
            public bool inUnscaledView;

            #endregion
        }

        #endregion

        #region Nested type: WidgetWithControlledSubwidgetsMetadata

        public abstract class WidgetWithControlledSubwidgetsMetadata<
            TSubwidget, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata> :
            AreaWidgetWithControlledSubwidgetsMetadata<TSubwidget, TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TSubwidget : MANAGER.ControlledSubwidget<TSubwidget, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidget :
            MANAGER.WidgetWithControlledSubwidgets<TSubwidget, TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata>, IAreaWidget
            where TWidgetMetadata : WidgetWithControlledSubwidgetsMetadata<TSubwidget, TWidget
                , TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : MANAGER.Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : FeatureMetadata<TFeature, TFeatureMetadata>
        {
            #region Fields and Autoproperties

            [FoldoutGroup(APPASTR.GroupNames.Common)]
            [OnValueChanged(nameof(OnChanged))]
            public bool inUnscaledView;

            #endregion
        }

        #endregion

        #region Nested type: WidgetWithSingletonSubwidgetsMetadata

        public abstract class WidgetWithSingletonSubwidgetsMetadata<
            TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata> :
            AreaWidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TISubwidget : class, IAreaSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
            where TISubwidgetMetadata : class,
            IAreaSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TWidget : MANAGER.WidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : WidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata,
                TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : MANAGER.Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : FeatureMetadata<TFeature, TFeatureMetadata>
        {
            #region Fields and Autoproperties

            [FoldoutGroup(APPASTR.GroupNames.Common)]
            [OnValueChanged(nameof(OnChanged))]
            public bool inUnscaledView;

            #endregion
        }

        #endregion

        #region Nested type: WidgetWithSubwidgetsMetadata

        public abstract class WidgetWithSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TWidget,
                                                           TWidgetMetadata, TFeature, TFeatureMetadata> :
            AreaWidgetWithSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TSubwidget : MANAGER.Subwidget<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata>, IEnableNotifier
            where TSubwidgetMetadata : SubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidget : MANAGER.WidgetWithSubwidgets<TSubwidget, TSubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : WidgetWithSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : MANAGER.Feature<TFeature, TFeatureMetadata>
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
