using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Screenshot.Services
{
    public sealed class ScreenshotService : DeveloperInterfaceManager_V01.Service<
                                                ScreenshotService, ScreenshotServiceMetadata,
                                                ScreenshotFeature, ScreenshotFeatureMetadata>,
                                            IAreaService,
                                            Screenshotter.IService
    {
        #region Fields and Autoproperties

        private Screenshotter.Functionality _functionality;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
                    this,
                    nameof(_functionality),
                    _functionality == null,
                    () => _functionality = new Screenshotter.Functionality()
                );

                _functionality.Initialize(initializer, this);
            }
        }

        #region IService Members

        public void InitiateServiceTask(Screenshotter.CompletedHandler notificationDelegate)
        {
            using (_PRF_InitiateServiceTask.Auto())
            {
                _functionality.InitiateServiceTask(metadata.Metadata, notificationDelegate);
            }
        }

        public void RequestScreenshot(Screenshotter.CompletedHandler handler, Camera targetCamera)
        {
            using (_PRF_RequestScreenshot.Auto())
            {
                _functionality.RequestScreenshot(metadata.Metadata, handler, targetCamera);
            }
        }

        public void RequestScreenshot(Screenshotter.CompletedHandler handler)
        {
            using (_PRF_RequestScreenshot.Auto())
            {
                _functionality.RequestScreenshot(metadata.Metadata, handler);
            }
        }

        public void RequestScreenshot()
        {
            using (_PRF_RequestScreenshot.Auto())
            {
                _functionality.RequestScreenshot(metadata.Metadata);
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitiateServiceTask =
            new ProfilerMarker(_PRF_PFX + nameof(InitiateServiceTask));

        private static readonly ProfilerMarker _PRF_RequestScreenshot =
            new ProfilerMarker(_PRF_PFX + nameof(RequestScreenshot));

        #endregion
    }
}
