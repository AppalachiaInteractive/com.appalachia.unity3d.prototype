using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Utility.Async;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Screenshot.Services
{
    public sealed class ScreenshotServiceMetadata : DeveloperInterfaceMetadata_V01.
                                                    ServiceMetadata<ScreenshotService,
                                                        ScreenshotServiceMetadata, ScreenshotFeature,
                                                        ScreenshotFeatureMetadata>,
                                                    Screenshotter.IServiceMetadata

    {
        #region Fields and Autoproperties

        [SerializeField] private Screenshotter.Metadata _metadata;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
                    this,
                    nameof(Screenshotter.Metadata),
                    _metadata == null,
                    () => _metadata = new Screenshotter.Metadata()
                );

                _metadata.Initialize(initializer, this);
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(ScreenshotService target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(ScreenshotService functionality)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
            }
        }

        #region IServiceMetadata Members

        public Screenshotter.Metadata Metadata => _metadata;

        #endregion
    }
}
