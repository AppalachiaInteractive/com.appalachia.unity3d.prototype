using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Appalachia.UI.Controls.Extensions;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01
{
    public partial class DeveloperInterfaceManager_V01
    {
        #region Nested type: Feature

        public abstract class Feature<TFeature, TFeatureMetadata> : AreaFeature<TFeature, TFeatureMetadata,
            DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
            where TFeature : Feature<TFeature, TFeatureMetadata>
            where TFeatureMetadata :
            DeveloperInterfaceMetadata_V01.FeatureMetadata<TFeature, TFeatureMetadata>
        {
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
            static Widget()
            {
                When.Behaviour<DeveloperInterfaceManager_V01>()
                    .IsAvailableThen(
                         i => { i.InitializationComplete += _ => { instance.MoveWidgetToCorrectView(i); }; }
                     );
            }

            private void MoveWidgetToCorrectView(DeveloperInterfaceManager_V01 manager)
            {
                using (_PRF_MoveWidgetToCorrectView.Auto())
                {
                    var containerView = metadata.inUnscaledView
                        ? manager.UnscaledCanvas.GameObject
                        : manager.View.GameObject;

                    if (transform.IsChildOf(containerView.transform))
                    {
                        return;
                    }

                    GameObject widgetContainerObject = null;

                    containerView.GetOrAddChild(ref widgetContainerObject, APPASTR.ObjectNames.Widgets, true);

                    var widgetRectTransform = widgetContainerObject.GetComponent<RectTransform>();
                    widgetRectTransform.Reset(RectResetOptions.All);

                    gameObject.SetParentTo(widgetContainerObject);
                }
            }

            #region Profiling

            private static readonly ProfilerMarker _PRF_MoveWidgetToCorrectView =
                new ProfilerMarker(_PRF_PFX + nameof(MoveWidgetToCorrectView));

            #endregion
        }

        #endregion
    }
}
