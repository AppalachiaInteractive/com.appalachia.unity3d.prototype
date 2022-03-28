using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Instanced;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Instanced.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Sirenix.OdinInspector;
using MANAGER = Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.DeveloperInterfaceManager_V01;
using METADATA = Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.DeveloperInterfaceMetadata_V01;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01
{
    public partial class DeveloperInterfaceMetadata_V01
    {
        #region Nested type: FeatureMetadata

        [Serializable]
        public abstract class FeatureMetadata<TFeature, TFeatureMetadata> : AreaFeatureMetadata<TFeature,
            TFeatureMetadata, MANAGER, METADATA>
            where TFeature : MANAGER.Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: InstancedSubwidgetMetadata

        [Serializable]
        public abstract class InstancedSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget,
                                                         TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
                                                         TFeatureMetadata> : AreaInstancedSubwidgetMetadata<TSubwidget,
            TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            MANAGER, METADATA>
            where TSubwidget :
            MANAGER.InstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidget, IEnableNotifier
            where TISubwidget : class, IAreaInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
            where TISubwidgetMetadata : class, IAreaInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TSubwidgetMetadata :
            InstancedSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidgetMetadata,
            IAreaInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TWidget : MANAGER.WidgetWithInstancedSubwidgets<TSubwidget, TSubwidgetMetadata, TISubwidget,
                TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : WidgetWithInstancedSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget,
                TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : MANAGER.Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: ServiceMetadata

        [Serializable]
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

        [Serializable]
        public abstract class SingletonSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget,
                                                         TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
                                                         TFeatureMetadata> : AreaSingletonSubwidgetMetadata<TSubwidget,
            TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            MANAGER, METADATA>
            where TSubwidget :
            MANAGER.SingletonSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidget,
            IAreaSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
            where TSubwidgetMetadata :
            SingletonSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidgetMetadata,
            IAreaSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TISubwidget : class, IAreaSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
            where TISubwidgetMetadata : class, IAreaSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TWidget : MANAGER.WidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : WidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TFeature : MANAGER.Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata : FeatureMetadata<TFeature, TFeatureMetadata>
        {
        }

        #endregion

        #region Nested type: WidgetMetadata

        [Serializable]
        public abstract class WidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata> : AreaWidgetMetadata<
            TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
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

        #region Nested type: WidgetWithInstancedSubwidgetsMetadata

        [Serializable]
        public abstract class WidgetWithInstancedSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget,
                                                                    TISubwidgetMetadata, TWidget, TWidgetMetadata,
                                                                    TFeature, TFeatureMetadata> :
            AreaWidgetWithInstancedSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata,
                TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TSubwidget :
            MANAGER.InstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidget, IEnableNotifier
            where TISubwidget : class, IAreaInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
            where TISubwidgetMetadata : class, IAreaInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TSubwidgetMetadata :
            InstancedSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidgetMetadata,
            IAreaInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TWidget : MANAGER.WidgetWithInstancedSubwidgets<TSubwidget, TSubwidgetMetadata, TISubwidget,
                TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : WidgetWithInstancedSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget,
                TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
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

        [Serializable]
        public abstract class WidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata, TWidget,
                                                                    TWidgetMetadata, TFeature, TFeatureMetadata> :
            AreaWidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata, MANAGER, METADATA>
            where TISubwidget : class, IAreaSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
            where TISubwidgetMetadata : class, IAreaSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
            where TWidget : MANAGER.WidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata>
            where TWidgetMetadata : WidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata, TWidget,
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
