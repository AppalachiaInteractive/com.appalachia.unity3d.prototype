using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Widgets;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.RuntimeGizmos.Services;
using Appalachia.UI.Controls.Sets.Images.RawImage;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.RuntimeGizmos.Widgets
{
    [CallStaticConstructorInEditor]
    public class RuntimeGizmoDrawerWidget : LifetimeWidget<RuntimeGizmoDrawerWidget,
        RuntimeGizmoDrawerWidgetMetadata, RuntimeGizmoDrawerFeature, RuntimeGizmoDrawerFeatureMetadata>
    {
        static RuntimeGizmoDrawerWidget()
        {
            When.Widget(instance)
                .AndService<RuntimeGizmoDrawerService>()
                .AreAvailableThen(
                     (widget, service) =>
                     {
                         _runtimeGizmoDrawerService = service;
                         widget.rawImageSet.RawImage.texture = service.GetRenderTexture();
                     }
                 );
        }

        #region Static Fields and Autoproperties

        private static RuntimeGizmoDrawerService _runtimeGizmoDrawerService;

        #endregion

        #region Fields and Autoproperties

        public RawImageComponentSet rawImageSet;

        #endregion
    }
}
