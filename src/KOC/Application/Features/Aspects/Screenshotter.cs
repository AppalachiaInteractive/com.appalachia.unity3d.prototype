using System;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Extensions;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features.Aspects
{
    public static class Screenshotter
    {
        public delegate void CompletedHandler(Args args);

        public enum ScreenshotCaptureMethod
        {
            EntireView = 0,
            SingleCamera = 10,

            //RenderTexture
        }

        public enum ScreenshotFileType
        {
            PNG,
            JPG
        }

        #region Nested type: Args

        [Serializable]
        public class Args
        {
            public Args(
                Texture2D screenshot,
                string screenshotFilePath,
                Texture2D preview,
                string previewFilePath)
            {
                this.screenshot = screenshot;
                this.screenshotFilePath = screenshotFilePath;
                this.preview = preview;
                this.previewFilePath = previewFilePath;
            }

            #region Fields and Autoproperties

            public readonly string previewFilePath;
            public readonly string screenshotFilePath;
            public readonly Texture2D preview;

            public readonly Texture2D screenshot;

            #endregion
        }

        #endregion

        #region Nested type: Functionality

        public sealed class Functionality
        {
            #region Constants and Static Readonly

            public const string DEFAULT_DIRECTORY_NAME = "Screenshots";

            #endregion

            #region Fields and Autoproperties

            private Camera _currentCamera;

            private int _counter;

            #endregion

            public void Initialize(Initializer initializer, IUnitySerializable owner)
            {
                using (_PRF_Initialize.Auto())
                {
                    initializer.Do(
                        owner,
                        nameof(Camera),
                        _currentCamera == null,
                        () => _currentCamera = Camera.main
                    );
                }
            }

            public void InitiateServiceTask(Metadata metadata, CompletedHandler handler)
            {
                ExecuteServiceTask(metadata, handler).Forget();
            }

            public void RequestScreenshot(Metadata metadata, CompletedHandler handler, Camera targetCamera)
            {
                using (_PRF_RequestScreenshot.Auto())
                {
                    _currentCamera = targetCamera;

                    RequestScreenshot(metadata, handler);
                }
            }

            public void RequestScreenshot(Metadata metadata, CompletedHandler handler)
            {
                using (_PRF_RequestScreenshot.Auto())
                {
                    InitiateServiceTask(metadata, handler);
                }
            }

            public void RequestScreenshot(Metadata metadata)
            {
                using (_PRF_RequestScreenshot.Auto())
                {
                    InitiateServiceTask(metadata, _ => { });
                }
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

            private static Texture2D ExecuteScreenshot(
                Metadata metadata,
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

                UnityEngine.Object.DestroyImmediate(renderTexture);

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

            private async AppaTask ExecuteServiceTask(
                Metadata metadata,
                CompletedHandler notificationDelegate)
            {
                var (nativeWidth, nativeHeight) = GetScreenshotNativeSize(metadata.captureMethod);
                var nativeRatio = nativeWidth / (float)nativeHeight;

                var outputHeight = metadata.outputSize;
                var outputWidth = (int)(outputHeight / nativeRatio);

                var outputDirectoryPath = GetOrCreateDirectory(metadata);
                var (screenshotFileName, previewFileName) =
                    GetCaptureFileName(metadata, outputWidth, outputHeight);
                var screenFilePath = AppaPath.Combine(outputDirectoryPath, screenshotFileName);

                Texture2D screenshot;

                using (_PRF_ExecuteServiceTask_CreateScreenshot.Auto())
                {
                    screenshot = ExecuteScreenshot(
                        metadata,
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
                    var completionArgs = new Args(screenshot, screenFilePath, preview, previewFilePath);

                    notificationDelegate?.Invoke(completionArgs);
                }
            }

            private (string fileName, string previewName) GetCaptureFileName(
                Metadata metadata,
                int width,
                int height)
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
                    if (metadata.useCustomFileNamePrefix &&
                        metadata.customFileNamePrefix.IsNotNullOrWhiteSpace())
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

            private string GetOrCreateDirectory(Metadata metadata)
            {
                using (_PRF_GetOrCreateDirectory.Auto())
                {
                    var directoryPath = GetSaveDirectory(metadata);

                    if (!AppaDirectory.Exists(directoryPath))
                    {
                        AppaDirectory.CreateDirectory(directoryPath);
                    }

                    return directoryPath;
                }
            }

            private string GetSaveDirectory(Metadata metadata)
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

            private const string _PRF_PFX = nameof(Functionality) + ".";

            private static readonly ProfilerMarker _PRF_Initialize =
                new ProfilerMarker(_PRF_PFX + nameof(Initialize));

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

        #endregion

        #region Nested type: IFeature

        public interface IFeature : IService
        {
        }

        #endregion

        #region Nested type: IService

        public interface IService : Notifier.IService<CompletedHandler>
        {
            void RequestScreenshot(CompletedHandler handler, Camera targetCamera);
            void RequestScreenshot(CompletedHandler handler);
            void RequestScreenshot();
        }

        #endregion

        #region Nested type: IServiceMetadata

        public interface IServiceMetadata
        {
            public Metadata Metadata { get; }
        }

        #endregion

        #region Nested type: Metadata

        [Serializable]
        public sealed class Metadata
        {
            #region Fields and Autoproperties

            [BoxGroup("Naming")] public bool includeCamera;

            [BoxGroup("Naming")] public bool includeCounter = true;

            [BoxGroup("Naming")] public bool includeDate = true;

            [BoxGroup("Naming")] public bool includeProject = true;

            [BoxGroup("Naming")] public bool includeResolution = true;

            [BoxGroup("Capture")] public bool linear;

            [BoxGroup("Output")] public bool saveToApplicationPath;

            [BoxGroup("Naming")] public bool useCustomFileNamePrefix;

            [BoxGroup("Output")] public bool useCustomSubDirectoryName;

            [BoxGroup("Capture")]
            [ValueDropdown(nameof(_textureWidths))]
            public int outputSize;

            [BoxGroup("Capture")]
            [ValueDropdown(nameof(_previewTextureWidths))]
            public int previewSize;

            [BoxGroup("Capture")] public RenderTextureFormat renderTextureFormat = RenderTextureFormat.ARGB32;

            [BoxGroup("Capture")] public ScreenshotCaptureMethod captureMethod;

            [BoxGroup("Output")] public ScreenshotFileType fileType;

            [BoxGroup("Output"), FolderPath, ShowIf(nameof(_showCustomDirectory))]
            public string customDirectoryName = "";

            [BoxGroup("Naming"), ShowIf(nameof(useCustomFileNamePrefix))]
            public string customFileNamePrefix = "";

            [BoxGroup("Capture")] public TextureFormat textureFormat = TextureFormat.ARGB32;

            #endregion

            private static int[] _previewTextureWidths => APPAINT.POWERS_OF.TWO.PreviewTextureSizes;

            private static int[] _textureWidths => APPAINT.POWERS_OF.TWO.TextureSizes;

            private bool _showCustomDirectory => useCustomSubDirectoryName;

            public void Initialize(Initializer initializer, IUnitySerializable owner)
            {
                using (_PRF_Initialize.Auto())
                {
                    initializer.Do(owner, nameof(outputSize),  outputSize == 0,  () => outputSize = 2048);
                    initializer.Do(owner, nameof(previewSize), previewSize == 0, () => previewSize = 256);
                    initializer.Do(owner, nameof(linear),      () => linear = true);
                    initializer.Do(
                        owner,
                        nameof(textureFormat),
                        textureFormat == default,
                        () => textureFormat = TextureFormat.ARGB32
                    );
                    initializer.Do(
                        owner,
                        nameof(renderTextureFormat),
                        renderTextureFormat == default,
                        () => renderTextureFormat = RenderTextureFormat.ARGB32
                    );
                }
            }

            #region Profiling

            private const string _PRF_PFX = nameof(Metadata) + ".";

            private static readonly ProfilerMarker _PRF_Initialize =
                new ProfilerMarker(_PRF_PFX + nameof(Initialize));

            #endregion
        }

        #endregion
    }
}
