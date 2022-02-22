using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Screenshot.Services;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Screenshot.Widgets;
using Appalachia.Utility.Constants;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Screenshot
{
    [CallStaticConstructorInEditor]
    public class ScreenshotFeature :
        DeveloperInterfaceManager_V01.Feature<ScreenshotFeature, ScreenshotFeatureMetadata>,
        Screenshotter.IFeature
    {
        static ScreenshotFeature()
        {
            FunctionalitySet.RegisterWidget<ScreenshotWidget>(_dependencyTracker, i => _screenshotWidget = i);
            FunctionalitySet.RegisterService<ScreenshotService>(
                _dependencyTracker,
                i => _screenshotService = i
            );
        }

        #region Static Fields and Autoproperties

        private static ScreenshotService _screenshotService;

        private static ScreenshotWidget _screenshotWidget;

        #endregion

        private void OnScreenshotCompleted(Screenshotter.Args args)
        {
            using (_PRF_OnScreenshotCompleted.Auto())
            {
                Context.Log.Info(
                    $"Screen shot completed.  Saved at {args.screenshotFilePath.FormatForLogging()}."
                );
            }
        }

        #region IFeature Members

        public void InitiateServiceTask(Screenshotter.CompletedHandler notificationDelegate)
        {
            using (_PRF_InitiateServiceTask.Auto())
            {
                _screenshotService.InitiateServiceTask(notificationDelegate);
            }
        }

        public void RequestScreenshot(Screenshotter.CompletedHandler handler, Camera targetCamera)
        {
            using (_PRF_RequestScreenshot.Auto())
            {
                _screenshotService.RequestScreenshot(handler, targetCamera);
            }
        }

        public void RequestScreenshot(Screenshotter.CompletedHandler handler)
        {
            using (_PRF_RequestScreenshot.Auto())
            {
                _screenshotService.RequestScreenshot(handler);
            }
        }

        public void RequestScreenshot()
        {
            using (_PRF_RequestScreenshot.Auto())
            {
                _screenshotService.RequestScreenshot(OnScreenshotCompleted);
            }
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_InitiateServiceTask =
            new ProfilerMarker(_PRF_PFX + nameof(InitiateServiceTask));

        protected static readonly ProfilerMarker _PRF_OnScreenshotCompleted =
            new ProfilerMarker(_PRF_PFX + nameof(OnScreenshotCompleted));

        protected static readonly ProfilerMarker _PRF_RequestScreenshot =
            new ProfilerMarker(_PRF_PFX + nameof(RequestScreenshot));

        #endregion
    }
}
