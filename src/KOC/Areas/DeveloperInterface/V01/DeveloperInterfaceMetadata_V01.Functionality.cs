using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Styling;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Instanced;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Instanced.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;
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

        #region Nested type: InstancedSubwidgetMetadata

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
            #region Fields and Autoproperties

            [SerializeField]
            [OnValueChanged(nameof(OnChanged))]
            [HideIf("@HideAllFields || !ShowsTooltip")]
            public DevTooltipSubwidgetMetadata devTooltipSubwidget;

            #endregion

            protected virtual bool ShowsTooltip => true;

            protected override void AfterUpdateFunctionality(TSubwidget subwidget)
            {
                using (_PRF_AfterUpdateFunctionality.Auto())
                {
                    base.AfterUpdateFunctionality(subwidget);

                    if (subwidget != null)
                    {
                        subwidget.OnDevTooltipUpdateRequested();
                    }
                }
            }

            /// <inheritdoc />
            protected override async AppaTask Initialize(Initializer initializer)
            {
                await base.Initialize(initializer);

                using (_PRF_Initialize.Auto())
                {
                    if (ShowsTooltip)
                    {
                        initializer.Do(
                            this,
                            nameof(devTooltipSubwidget),
                            devTooltipSubwidget == null,
                            () =>
                            {
                                if (devTooltipSubwidget == null)
                                {
                                    var metadataName = typeof(TSubwidget).Name + nameof(DevTooltipSubwidgetMetadata);

                                    devTooltipSubwidget = AppalachiaObject.LoadOrCreateNew<DevTooltipSubwidgetMetadata>(
                                        metadataName,
                                        ownerType: AppalachiaRepository.PrimaryOwnerType
                                    );
                                }
                            }
                        );
                    }
                }
            }

            protected override void SubscribeResponsiveComponents(TSubwidget target)
            {
                using (_PRF_SubscribeResponsiveComponents.Auto())
                {
                    base.SubscribeResponsiveComponents(target);

                    devTooltipSubwidget.Changed.Event += OnChanged;
                }
            }
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
            #region Fields and Autoproperties

            [SerializeField]
            [OnValueChanged(nameof(OnChanged))]
            [HideIf("@HideAllFields || !ShowsTooltip")]
            public DevTooltipSubwidgetMetadata devTooltipSubwidget;

            #endregion

            protected virtual bool ShowsTooltip => true;

            protected override void AfterUpdateFunctionality(TSubwidget subwidget)
            {
                using (_PRF_AfterUpdateFunctionality.Auto())
                {
                    base.AfterUpdateFunctionality(subwidget);

                    if (subwidget != null)
                    {
                        subwidget.OnDevTooltipUpdateRequested();
                    }
                }
            }

            /// <inheritdoc />
            protected override async AppaTask Initialize(Initializer initializer)
            {
                await base.Initialize(initializer);

                using (_PRF_Initialize.Auto())
                {
                    if (ShowsTooltip)
                    {
                        initializer.Do(
                            this,
                            nameof(devTooltipSubwidget),
                            devTooltipSubwidget == null,
                            () =>
                            {
                                if (devTooltipSubwidget == null)
                                {
                                    var metadataName = typeof(TSubwidget).Name + nameof(DevTooltipSubwidgetMetadata);

                                    devTooltipSubwidget = AppalachiaObject.LoadOrCreateNew<DevTooltipSubwidgetMetadata>(
                                        metadataName,
                                        ownerType: AppalachiaRepository.PrimaryOwnerType
                                    );
                                }
                            }
                        );
                    }
                }
            }

            protected override void SubscribeResponsiveComponents(TSubwidget target)
            {
                using (_PRF_SubscribeResponsiveComponents.Auto())
                {
                    base.SubscribeResponsiveComponents(target);

                    devTooltipSubwidget.Changed.Event += OnChanged;
                }
            }
        }

        #endregion

        #region Nested type: WidgetMetadata

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

            [OnValueChanged(nameof(OnChanged))]
            [SerializeField]
            public DevTooltipStyleOverride devTooltipStyle;

            #endregion

            protected virtual bool ShowsTooltip => true;

            /// <inheritdoc />
            protected override async AppaTask Initialize(Initializer initializer)
            {
                await base.Initialize(initializer);

                using (_PRF_Initialize.Auto())
                {
                    if (ShowsTooltip)
                    {
                        initializer.Do(
                            this,
                            nameof(devTooltipStyle),
                            devTooltipStyle == null,
                            () =>
                            {
                                devTooltipStyle = LoadOrCreateNew<DevTooltipStyleOverride>(
                                    GetAssetName<DevTooltipStyleOverride>(),
                                    ownerType: typeof(ApplicationManager)
                                );
                            }
                        );
                    }
                }
            }
        }

        #endregion

        #region Nested type: WidgetWithSingletonSubwidgetsMetadata

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

            [OnValueChanged(nameof(OnChanged))]
            [SerializeField]
            [HideIf(nameof(HideAllFields))]
            public DevTooltipStyleOverride devTooltipStyle;

            [FoldoutGroup(APPASTR.GroupNames.Common)]
            [OnValueChanged(nameof(OnChanged))]
            public bool inUnscaledView;

            #endregion

            protected virtual bool ShowsTooltip => true;

            /// <inheritdoc />
            protected override async AppaTask Initialize(Initializer initializer)
            {
                await base.Initialize(initializer);

                using (_PRF_Initialize.Auto())
                {
                    if (ShowsTooltip)
                    {
                        initializer.Do(
                            this,
                            nameof(devTooltipStyle),
                            devTooltipStyle == null,
                            () =>
                            {
                                devTooltipStyle = LoadOrCreateNew<DevTooltipStyleOverride>(
                                    GetAssetName<DevTooltipStyleOverride>(),
                                    ownerType: typeof(ApplicationManager)
                                );
                            }
                        );
                    }
                }
            }
        }

        #endregion
    }
}
