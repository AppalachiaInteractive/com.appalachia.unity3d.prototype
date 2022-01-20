using System;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Extensions;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Services.NotificationOnComplete;
using Appalachia.Prototype.KOC.Application.Services.Screenshot.Model;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Services.Screenshot
{
    public abstract class ScreenshotService<TService, TServiceMetadata> : NotificationOnCompleteService<
        TService, TServiceMetadata, ScreenshotCompletedHandler>
        where TService : ScreenshotService<TService, TServiceMetadata>
        where TServiceMetadata : ScreenshotServiceMetadata<TService, TServiceMetadata>
    {
        #region Constants and Static Readonly

        public const string DEFAULT_DIRECTORY_NAME = "Screenshots";

        #endregion

        #region Fields and Autoproperties

        private int _counter;
        private Camera _currentCamera;

        #endregion

        public void RequestScreenshot(ScreenshotCompletedHandler handler, Camera targetCamera)
        {
            using (_PRF_RequestScreenshot.Auto())
            {
                _currentCamera = targetCamera;

                RequestScreenshot(handler);
            }
        }

        public void RequestScreenshot(ScreenshotCompletedHandler handler)
        {
            using (_PRF_RequestScreenshot.Auto())
            {
                InitiateServiceTask(handler);
            }
        }

        [ButtonGroup(GROUP_NAME)]
        public void RequestScreenshot()
        {
            using (_PRF_RequestScreenshot.Auto())
            {
                InitiateServiceTask(_ => { });
            }
        }

        protected override async AppaTask ExecuteServiceTask(ScreenshotCompletedHandler notificationDelegate)
        {
            var (nativeWidth, nativeHeight) = GetScreenshotNativeSize(metadata.captureMethod);
            var nativeRatio = nativeWidth / (float)nativeHeight;

            var outputHeight = metadata.outputSize;
            var outputWidth = (int)(outputHeight / nativeRatio);

            var outputDirectoryPath = GetOrCreateDirectory();
            var (screenshotFileName, previewFileName) = GetCaptureFileName(outputWidth, outputHeight);
            var screenFilePath = AppaPath.Combine(outputDirectoryPath, screenshotFileName);

            Texture2D screenshot;

            using (_PRF_ExecuteServiceTask_CreateScreenshot.Auto())
            {
                screenshot = ExecuteScreenshot(
                    outputWidth,
                    outputHeight,
                    metadata.captureMethod == ScreenshotCaptureMethod.EntireView
                        ? CaptureEntireView
                        : rt => CaptureSingleCamera(rt, _currentCamera),
                    metadata.captureMethod == ScreenshotCaptureMethod.EntireView
                        ? PostCaptureEntireView
                        : () => PostCaptureSingleCamera(_currentCamera)
                );
            }

            await AppaTask.Yield();

            using (_PRF_ExecuteServiceTask_SaveScreenshot.Auto())
            {
                screenshot.WriteToPNGFile(screenFilePath);
            }

            await AppaTask.Yield();

            Texture2D preview;

            using (_PRF_ExecuteServiceTask_CreatePreview.Auto())
            {
                var previewHeight = metadata.previewSize;
                var previewWidth = (int)(previewHeight / nativeRatio);

                preview = screenshot.Resize(
                    previewWidth,
                    previewHeight,
                    metadata.textureFormat,
                    metadata.renderTextureFormat,
                    metadata.linear
                );
            }

            var previewFilePath = AppaPath.Combine(outputDirectoryPath, previewFileName);

            using (_PRF_ExecuteServiceTask_SavePreview.Auto())
            {
                preview.WriteToPNGFile(previewFilePath);
            }

            await AppaTask.Yield();

            using (_PRF_ExecuteServiceTask_InvokeNotification.Auto())
            {
                var completionArgs = new ScreenshotCompletedArgs(
                    screenshot,
                    screenFilePath,
                    preview,
                    previewFilePath
                );

                notificationDelegate?.Invoke(completionArgs);
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
                    this,
                    nameof(Camera),
                    _currentCamera == null,
                    () => _currentCamera = Camera.main
                );
            }
        }

        protected override void OnApplyMetadataInternal()
        {
        }

        private static void CaptureEntireView(RenderTexture target)
        {
            using (_PRF_CaptureEntireView.Auto())
            {
                ScreenCapture.CaptureScreenshotIntoRenderTexture(target);
            }
        }

        private static void CaptureSingleCamera(RenderTexture target, Camera targetCamera)
        {
            using (_PRF_CaptureRenderTexture.Auto())
            {
                targetCamera.targetTexture = target;

                targetCamera.Render();

                RenderTexture.active = target;
            }
        }

        private static void CreateScreenshot()
        {
        }

        private static Texture2D ExecuteScreenshot(
            int width,
            int height,
            Action<RenderTexture> capture,
            Action postCapture)
        {
            var renderTexture = new RenderTexture(width, height, 24);
            var screenshot = new Texture2D(width, height, metadata.textureFormat, false);

            capture(renderTexture);

            screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

            postCapture?.Invoke();

            DestroyImmediate(renderTexture);

            return screenshot;
        }

        private static void PostCaptureEntireView()
        {
            using (_PRF_PostCaptureEntireView.Auto())
            {
            }
        }

        private static void PostCaptureSingleCamera(Camera targetCamera)
        {
            using (_PRF_PostCaptureSingleCamera.Auto())
            {
                targetCamera.targetTexture = null;
                RenderTexture.active = null;
            }
        }

        private void AfterScreenshotCapture(Texture2D primaryTexture)
        {
            using (_PRF_AfterScreenshotCapture.Auto())
            {
                _counter++;
            }
        }

        private (string fileName, string previewName) GetCaptureFileName(int width, int height)
        {
            using (_PRF_GetCaptureFileName.Auto())
            {
                const string UNDERSCORE = "_";
                const string DATE_FORMAT = "yyyy-MM-dd-hh-mm-ss-fff";
                const string EXT_JPG = ".jpg";
                const string EXT_PNG = ".png";

                var fileNameBuilder = new Utf8ValueStringBuilder(true);

                var hasOpenedValue = false;

                // custom name
                if (metadata.useCustomFileNamePrefix && metadata.customFileNamePrefix.IsNotNullOrWhiteSpace())
                {
                    fileNameBuilder.Append(metadata.customFileNamePrefix);
                    hasOpenedValue = true;
                }

                if (metadata.includeProject)
                {
                    if (hasOpenedValue)
                    {
                        fileNameBuilder.Append(UNDERSCORE);
                    }

                    fileNameBuilder.Append(AppalachiaApplication.ProductName);
                    hasOpenedValue = true;
                }

                if (metadata.includeCamera)
                {
                    if (hasOpenedValue)
                    {
                        fileNameBuilder.Append(UNDERSCORE);
                    }

                    fileNameBuilder.Append(_currentCamera.name);
                    hasOpenedValue = true;
                }

                // add date
                if (metadata.includeDate)
                {
                    if (hasOpenedValue)
                    {
                        fileNameBuilder.Append(UNDERSCORE);
                    }

                    fileNameBuilder.Append(DateTime.Now.ToString(DATE_FORMAT));
                    hasOpenedValue = true;
                }

                // add resolution
                if (metadata.includeResolution)
                {
                    if (hasOpenedValue)
                    {
                        fileNameBuilder.Append(UNDERSCORE);
                    }

                    var resolutionString = GetScreenResolutionString(width, height);
                    fileNameBuilder.Append(resolutionString);
                    hasOpenedValue = true;
                }

                // add counter
                if (metadata.includeCounter)
                {
                    if (hasOpenedValue)
                    {
                        fileNameBuilder.Append(UNDERSCORE);
                    }

                    fileNameBuilder.Append(_counter.ToString("D4"));
                    hasOpenedValue = true;
                }

                // if the filename is empty, add the date at least
                if (fileNameBuilder.Length == 0)
                {
                    fileNameBuilder.Append(DateTime.Now.ToString(DATE_FORMAT));
                }

                const string PREVIEW_POSTFIX = ".PREVIEW";
                fileNameBuilder.Append(PREVIEW_POSTFIX);

                fileNameBuilder.Append(metadata.fileType == ScreenshotFileType.JPG ? EXT_JPG : EXT_PNG);

                var previewFileName = fileNameBuilder.ToString();
                var outputFileName = previewFileName.Replace(PREVIEW_POSTFIX, string.Empty);

                return (outputFileName, previewFileName);
            }
        }

        private string GetOrCreateDirectory()
        {
            using (_PRF_GetOrCreateDirectory.Auto())
            {
                var directoryPath = GetSaveDirectory();

                if (!AppaDirectory.Exists(directoryPath))
                {
                    AppaDirectory.CreateDirectory(directoryPath);
                }

                return directoryPath;
            }
        }

        private string GetSaveDirectory()
        {
            using (_PRF_GetSaveDirectory.Auto())
            {
                var hasCustomDirectoryName = metadata.useCustomSubDirectoryName &&
                                             metadata.customDirectoryName.IsNotNullOrWhiteSpace();

                var baseDirectory = metadata.saveToApplicationPath
                    ? AppalachiaApplication.PersistentDataPath
                    : AppaDirectory.GetCurrentDirectory();

                var directoryName = hasCustomDirectoryName
                    ? metadata.customDirectoryName.Trim()
                    : DEFAULT_DIRECTORY_NAME;

                return AppaPath.Combine(baseDirectory, directoryName);
            }
        }

        private string GetScreenResolutionString(int width, int height)
        {
            using (_PRF_GetScreenResolutionString.Auto())
            {
                return ZString.Format("{0}x{1}", width, height);
            }
        }

        private (int width, int height) GetScreenshotNativeSize(ScreenshotCaptureMethod method)
        {
            using (_PRF_GetScreenshotNativeSize.Auto())
            {
                if (_currentCamera == null)
                {
                    _currentCamera = Camera.current;
                }

                switch (method)
                {
                    case ScreenshotCaptureMethod.EntireView:
                        return (Screen.width, Screen.height);

                    case ScreenshotCaptureMethod.SingleCamera:

                        return (_currentCamera.pixelWidth, _currentCamera.pixelHeight);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(method), method, null);
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ExecuteServiceTask_CreatePreview =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteServiceTask) + ".CreatePreview");

        private static readonly ProfilerMarker _PRF_ExecuteServiceTask_CreateScreenshot =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteServiceTask) + ".CreateScreenshot");

        private static readonly ProfilerMarker _PRF_ExecuteServiceTask_InvokeNotification =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteServiceTask) + ".InvokeNotification");

        private static readonly ProfilerMarker _PRF_ExecuteServiceTask_SavePreview =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteServiceTask) + ".SavePreview");

        private static readonly ProfilerMarker _PRF_ExecuteServiceTask_SaveScreenshot =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteServiceTask) + ".SaveScreenshot");

        private static readonly ProfilerMarker _PRF_PostCaptureEntireView =
            new ProfilerMarker(_PRF_PFX + nameof(PostCaptureEntireView));

        private static readonly ProfilerMarker _PRF_PostCaptureSingleCamera =
            new ProfilerMarker(_PRF_PFX + nameof(PostCaptureSingleCamera));

        private static readonly ProfilerMarker _PRF_AfterScreenshotCapture =
            new ProfilerMarker(_PRF_PFX + nameof(AfterScreenshotCapture));

        private static readonly ProfilerMarker _PRF_CaptureEntireView =
            new ProfilerMarker(_PRF_PFX + nameof(CaptureEntireView));

        private static readonly ProfilerMarker _PRF_CaptureRenderTexture =
            new ProfilerMarker(_PRF_PFX + nameof(CaptureSingleCamera));

        private static readonly ProfilerMarker _PRF_CaptureScreenshot =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteScreenshot));

        private static readonly ProfilerMarker _PRF_GetCaptureFileName =
            new ProfilerMarker(_PRF_PFX + nameof(GetCaptureFileName));

        private static readonly ProfilerMarker _PRF_GetOrCreateDirectory =
            new ProfilerMarker(_PRF_PFX + nameof(GetOrCreateDirectory));

        private static readonly ProfilerMarker _PRF_GetSaveDirectory =
            new ProfilerMarker(_PRF_PFX + nameof(GetSaveDirectory));

        private static readonly ProfilerMarker _PRF_GetScreenResolutionString =
            new ProfilerMarker(_PRF_PFX + nameof(GetScreenResolutionString));

        private static readonly ProfilerMarker _PRF_GetScreenshotNativeSize =
            new ProfilerMarker(_PRF_PFX + nameof(GetScreenshotNativeSize));

        private static readonly ProfilerMarker _PRF_RequestScreenshot =
            new ProfilerMarker(_PRF_PFX + nameof(RequestScreenshot));

        #endregion
    }
}
