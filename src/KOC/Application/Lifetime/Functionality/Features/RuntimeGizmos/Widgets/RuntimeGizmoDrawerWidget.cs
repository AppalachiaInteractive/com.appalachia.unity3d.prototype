using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos.Services;
using Appalachia.UI.Functionality.Images.Controls.Raw;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos.Widgets
{
    [CallStaticConstructorInEditor]
    public class RuntimeGizmoDrawerWidget : LifetimeWidget<RuntimeGizmoDrawerWidget,
        RuntimeGizmoDrawerWidgetMetadata, RuntimeGizmoDrawerFeature, RuntimeGizmoDrawerFeatureMetadata>
    {
        static RuntimeGizmoDrawerWidget()
        {
            When.Service<RuntimeGizmoDrawerService>()
                .IsAvailableThen(service => { _runtimeGizmoDrawerService = service; });
        }

        #region Static Fields and Autoproperties

        private static RuntimeGizmoDrawerService _runtimeGizmoDrawerService;

        #endregion

        #region Fields and Autoproperties

        public RawImageControl rawImageSet;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask AfterEnabled()
        {
            await base.AfterEnabled();

            await AppaTask.WaitUntil(() => (rawImageSet != null) && (_runtimeGizmoDrawerService != null));

            rawImageSet.RawImage.rawImage.texture = _runtimeGizmoDrawerService.GetRenderTexture();
        }
    }
}
