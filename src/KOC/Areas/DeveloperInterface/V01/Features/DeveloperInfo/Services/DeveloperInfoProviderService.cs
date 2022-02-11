using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Models;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Appalachia.Utility.Timing;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable FormatStringProblem

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Services
{
    [CallStaticConstructorInEditor]
    public sealed class DeveloperInfoProviderService : DeveloperInterfaceManager_V01.Service<
        DeveloperInfoProviderService, DeveloperInfoProviderServiceMetadata, DeveloperInfoFeature,
        DeveloperInfoFeatureMetadata>
    {
        #region Constants and Static Readonly

        private const string STRING_GRAPHICS_DEVICE_NAME = "{0}";
        private const string STRING_GRAPHICS_DEVICE_VERSION = "Graphics API: {0}";

        private const string STRING_GRAPHICS_MEMORY_SIZE = "{0}MB. Max texture: {1}px. Shader level: {2}";

        private const string STRING_OPERATING_SYSTEM = "{0} [{1}]";
        private const string STRING_PROCESSOR_TYPE = "{0} [{1} cores]";

        private const string STRING_SCENE_NAME_AND_COUNT = "{0} ({1})";
        private const string STRING_SCREEN_RESOLUTION = "{0}x{1}@{2}Hz";
        private const string STRING_SYSTEM_MEMORY = "{0} MB";
        private const string STRING_WINDOW_SIZE = "{0}x{1} @ {2}Hz [{3}dpi]";

        private static readonly Utf16PreparedFormat<string, int> FORMAT_SCENE_NAME_AND_COUNT =
            new(STRING_SCENE_NAME_AND_COUNT);

        private static readonly Utf16PreparedFormat<int, int, int, string> FORMAT_WINDOW_SIZE =
            new(STRING_WINDOW_SIZE);

        private static readonly Utf16PreparedFormat<string, int> FORMAT_PROCESSOR_TYPE =
            new(STRING_PROCESSOR_TYPE);

        private static readonly Utf16PreparedFormat<int> FORMAT_SYSTEM_MEMORY = new(STRING_SYSTEM_MEMORY);

        private static readonly Utf16PreparedFormat<string> FORMAT_GRAPHICS_DEVICE_VERSION =
            new(STRING_GRAPHICS_DEVICE_VERSION);

        private static readonly Utf16PreparedFormat<string> FORMAT_GRAPHICS_DEVICE_NAME =
            new(STRING_GRAPHICS_DEVICE_NAME);

        private static readonly Utf16PreparedFormat<int, int, int> FORMAT_GRAPHICS_MEMORY_SIZE =
            new(STRING_GRAPHICS_MEMORY_SIZE);

        private static readonly Utf16PreparedFormat<int, int, int> FORMAT_SCREEN_RESOLUTION =
            new(STRING_SCREEN_RESOLUTION);

        private static readonly Utf16PreparedFormat<string, DeviceType> FORMAT_OPERATING_SYSTEM =
            new(STRING_OPERATING_SYSTEM);

        #endregion

        public string GetLabel(DeveloperInfoType val)
        {
            using (_PRF_GetLabel.Auto())
            {
                switch (val)
                {
                    case DeveloperInfoType.None:
                        return "None";
                    case DeveloperInfoType.ActiveSceneName:
                        return "Active Scene";
                    case DeveloperInfoType.ActiveScenePath:
                        return "Scene Path";
                    case DeveloperInfoType.SceneCount:
                        return "Scene Count";
                    case DeveloperInfoType.ActiveSceneNameAndCount:
                        return "Active Scene";
                    case DeveloperInfoType.ApplicationPlatform:
                        return "Platform";
                    case DeveloperInfoType.ApplicationVersion:
                        return "Version";
                    case DeveloperInfoType.ApplicationBuildGuid:
                        return "Build Guid";
                    case DeveloperInfoType.ApplicationProductName:
                        return "Product Name";
                    case DeveloperInfoType.ApplicationCompanyName:
                        return "Company Name";
                    case DeveloperInfoType.ApplicationInstallerName:
                        return "Installer";
                    case DeveloperInfoType.ApplicationSystemLanguage:
                        return "Sys. Language";
                    case DeveloperInfoType.ApplicationUnityVersion:
                        return "Unity Version";
                    case DeveloperInfoType.FrameCount:
                        return "Frame Count";
                    case DeveloperInfoType.TimeScale:
                        return "Time Scale";
                    case DeveloperInfoType.WindowSize:
                        return "Window";
                    case DeveloperInfoType.ScreenResolution:
                        return "Screen";
                    case DeveloperInfoType.ProcessorType:
                        return "Processor";
                    case DeveloperInfoType.SystemMemory:
                        return "Memory";
                    case DeveloperInfoType.GraphicsDeviceVersion:
                        return "GPU API";
                    case DeveloperInfoType.GraphicsDeviceName:
                        return "GPU Name";
                    case DeveloperInfoType.GraphicsMemorySize:
                        return "GPU Memory";
                    case DeveloperInfoType.OperatingSystem:
                        return "OS";
                    case DeveloperInfoType.MachineName:
                        return "Machine";
                    case DeveloperInfoType.UserName:
                        return "User";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public string GetUpdatedText(DeveloperInfoType val)
        {
            using (_PRF_GetUpdatedText.Auto())
            {
                var result = val switch
                {
                    DeveloperInfoType.None                     => string.Empty,
                    DeveloperInfoType.ActiveSceneName          => SceneManager.GetActiveScene().name,
                    DeveloperInfoType.ActiveScenePath          => SceneManager.GetActiveScene().path,
                    DeveloperInfoType.SceneCount               => SceneManager.sceneCount.ToString(),
                    DeveloperInfoType.ApplicationPlatform      => AppalachiaApplication.Platform.ToString(),
                    DeveloperInfoType.ApplicationVersion       => AppalachiaApplication.Version,
                    DeveloperInfoType.ApplicationBuildGuid     => AppalachiaApplication.BuildGuid,
                    DeveloperInfoType.ApplicationProductName   => AppalachiaApplication.ProductName,
                    DeveloperInfoType.ApplicationCompanyName   => AppalachiaApplication.CompanyName,
                    DeveloperInfoType.ApplicationInstallerName => AppalachiaApplication.InstallerName,
                    DeveloperInfoType.ApplicationSystemLanguage => AppalachiaApplication.SystemLanguage
                       .ToString(),
                    DeveloperInfoType.ApplicationUnityVersion => AppalachiaApplication.UnityVersion,
                    DeveloperInfoType.FrameCount => CoreClock.Instance.FrameCount.ToString(),
                    DeveloperInfoType.TimeScale => CoreClock.Instance.TimeScale.ToStringNonAlloc(3),
                    DeveloperInfoType.ActiveSceneNameAndCount => FORMAT_SCENE_NAME_AND_COUNT.Format(
                        SceneManager.GetActiveScene().name,
                        SceneManager.sceneCount
                    ),
                    DeveloperInfoType.WindowSize => FORMAT_WINDOW_SIZE.Format(
                        Screen.width,
                        Screen.height,
                        Screen.currentResolution.refreshRate,
                        Screen.dpi.ToStringNonAlloc(0)
                    ),
                    DeveloperInfoType.ScreenResolution => FORMAT_SCREEN_RESOLUTION.Format(
                        Screen.currentResolution.width,
                        Screen.currentResolution.height,
                        Screen.currentResolution.refreshRate
                    ),
                    DeveloperInfoType.ProcessorType => FORMAT_PROCESSOR_TYPE.Format(
                        SystemInfo.processorType,
                        SystemInfo.processorCount
                    ),
                    DeveloperInfoType.SystemMemory =>
                        FORMAT_SYSTEM_MEMORY.Format(SystemInfo.systemMemorySize),
                    DeveloperInfoType.GraphicsDeviceVersion => FORMAT_GRAPHICS_DEVICE_VERSION.Format(
                        SystemInfo.graphicsDeviceVersion
                    ),
                    DeveloperInfoType.GraphicsDeviceName => FORMAT_GRAPHICS_DEVICE_NAME.Format(
                        SystemInfo.graphicsDeviceName
                    ),
                    DeveloperInfoType.GraphicsMemorySize => FORMAT_GRAPHICS_MEMORY_SIZE.Format(
                        SystemInfo.graphicsMemorySize,
                        SystemInfo.maxTextureSize,
                        SystemInfo.graphicsShaderLevel
                    ),
                    DeveloperInfoType.OperatingSystem => FORMAT_OPERATING_SYSTEM.Format(
                        SystemInfo.operatingSystem,
                        SystemInfo.deviceType
                    ),
                    DeveloperInfoType.MachineName => Environment.MachineName,
                    DeveloperInfoType.UserName    => Environment.UserName,

                    _ => throw new ArgumentOutOfRangeException()
                };

                return result;
            }
        }

        public bool IsImmutableValue(DeveloperInfoType x)
        {
            using (_PRF_IsImmutableValue.Auto())
            {
                switch (x)
                {
                    case DeveloperInfoType.None:
                    case DeveloperInfoType.ApplicationPlatform:
                    case DeveloperInfoType.ApplicationVersion:
                    case DeveloperInfoType.ApplicationBuildGuid:
                    case DeveloperInfoType.ApplicationProductName:
                    case DeveloperInfoType.ApplicationCompanyName:
                    case DeveloperInfoType.ApplicationInstallerName:
                    case DeveloperInfoType.ApplicationSystemLanguage:
                    case DeveloperInfoType.ApplicationUnityVersion:
                    case DeveloperInfoType.ProcessorType:
                    case DeveloperInfoType.SystemMemory:
                    case DeveloperInfoType.GraphicsDeviceVersion:
                    case DeveloperInfoType.GraphicsDeviceName:
                    case DeveloperInfoType.GraphicsMemorySize:
                    case DeveloperInfoType.OperatingSystem:
                    case DeveloperInfoType.MachineName:
                    case DeveloperInfoType.UserName:
                        return true;
                    default:
                        return false;
                }
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetLabel =
            new ProfilerMarker(_PRF_PFX + nameof(GetLabel));

        private static readonly ProfilerMarker _PRF_GetUpdatedText =
            new ProfilerMarker(_PRF_PFX + nameof(GetUpdatedText));

        private static readonly ProfilerMarker _PRF_IsImmutableValue =
            new ProfilerMarker(_PRF_PFX + nameof(IsImmutableValue));

        #endregion
    }
}
