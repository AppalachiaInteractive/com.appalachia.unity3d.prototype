using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Models;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Services.Audio;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Services.FPS;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Services.Memory;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
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

        private const string STRING_FPS = "{0} fps";
        private const string STRING_MILLISECONDS = "{0:0.00} ms";
        private const string STRING_MEGABYTES = "{0} MB";
        private const string STRING_DECIBALS = "{0} dB";

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

        private static readonly Utf16PreparedFormat<int, int, int, string> FORMAT_WINDOW_SIZE = new(STRING_WINDOW_SIZE);
        private static readonly Utf16PreparedFormat<string, int> FORMAT_PROCESSOR_TYPE = new(STRING_PROCESSOR_TYPE);
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

        private static readonly Utf16PreparedFormat<ushort> FORMAT_FPS = new(STRING_FPS);

        private static readonly Utf16PreparedFormat<float> FORMAT_MILLISECONDS = new(STRING_MILLISECONDS);
        private static readonly Utf16PreparedFormat<float> FORMAT_MEGABYTES = new(STRING_MEGABYTES);
        private static readonly Utf16PreparedFormat<float> FORMAT_DECIBALS = new(STRING_DECIBALS);
        
            
        
        #endregion

        static DeveloperInfoProviderService()
        {
            When.Behaviour<MemoryProfilerService>().IsAvailableThen(i => _memoryProfilerService = i);
            When.Behaviour<FPSProfilerService>().IsAvailableThen(i => _fpsProfilerService = i);
            When.Behaviour<AudioProfilerService>().IsAvailableThen(i => _audioProfilerService = i);
        }

        #region Static Fields and Autoproperties

        private static AudioProfilerService _audioProfilerService;
        private static FPSProfilerService _fpsProfilerService;

        private static MemoryProfilerService _memoryProfilerService;

        #endregion

        public string GetLabel(DeveloperInfoType val)
        {
            using (_PRF_GetLabel.Auto())
            {
                return val switch
                {
                    DeveloperInfoType.None                      => "None",
                    DeveloperInfoType.ActiveSceneName           => "Active Scene",
                    DeveloperInfoType.ActiveScenePath           => "Scene Path",
                    DeveloperInfoType.SceneCount                => "Scene Count",
                    DeveloperInfoType.ActiveSceneNameAndCount   => "Active Scene",
                    DeveloperInfoType.ApplicationPlatform       => "Platform",
                    DeveloperInfoType.ApplicationVersion        => "Version",
                    DeveloperInfoType.ApplicationBuildGuid      => "Build Guid",
                    DeveloperInfoType.ApplicationProductName    => "Product Name",
                    DeveloperInfoType.ApplicationCompanyName    => "Company Name",
                    DeveloperInfoType.ApplicationInstallerName  => "Installer",
                    DeveloperInfoType.ApplicationSystemLanguage => "Sys. Language",
                    DeveloperInfoType.ApplicationUnityVersion   => "Unity Version",
                    DeveloperInfoType.FrameCount                => "Frame Count",
                    DeveloperInfoType.TimeScale                 => "Time Scale",
                    DeveloperInfoType.WindowSize                => "Window",
                    DeveloperInfoType.ScreenResolution          => "Screen",
                    DeveloperInfoType.ProcessorType             => "Processor",
                    DeveloperInfoType.SystemMemory              => "Memory",
                    DeveloperInfoType.GraphicsDeviceVersion     => "GPU API",
                    DeveloperInfoType.GraphicsDeviceName        => "GPU Name",
                    DeveloperInfoType.GraphicsMemorySize        => "GPU Memory",
                    DeveloperInfoType.OperatingSystem           => "OS",
                    DeveloperInfoType.MachineName               => "Machine",
                    DeveloperInfoType.UserName                  => "User",
                    DeveloperInfoType.SceneList                 => "Scene List",
                    DeveloperInfoType.ProfilingFPSAverage       => "Avg",
                    DeveloperInfoType.ProfilingFPSMin           => "Min",
                    DeveloperInfoType.ProfilingFPSMax           => "Max",
                    DeveloperInfoType.ProfilingFPSCurrent       => null,
                    DeveloperInfoType.ProfilingFPSFrame         => null,
                    DeveloperInfoType.ProfilingRAMMono          => "Mono",
                    DeveloperInfoType.ProfilingRAMAllocated     => "Allocated",
                    DeveloperInfoType.ProfilingRAMReserved      => "Reserved",
                    DeveloperInfoType.ProfilingAudioDecibals    => null,
                    _                                           => throw new ArgumentOutOfRangeException()
                };
            }
        }

        public string GetUpdatedText(DeveloperInfoType val)
        {
            using (_PRF_GetUpdatedText.Auto())
            {
                var result = val switch
                {
                    DeveloperInfoType.None                      => string.Empty,
                    DeveloperInfoType.ActiveSceneName           => SceneManager.GetActiveScene().name,
                    DeveloperInfoType.ActiveScenePath           => SceneManager.GetActiveScene().path,
                    DeveloperInfoType.SceneCount                => SceneManager.sceneCount.ToString(),
                    DeveloperInfoType.ApplicationPlatform       => AppalachiaApplication.Platform.ToString(),
                    DeveloperInfoType.ApplicationVersion        => AppalachiaApplication.Version,
                    DeveloperInfoType.ApplicationBuildGuid      => AppalachiaApplication.BuildGuid,
                    DeveloperInfoType.ApplicationProductName    => AppalachiaApplication.ProductName,
                    DeveloperInfoType.ApplicationCompanyName    => AppalachiaApplication.CompanyName,
                    DeveloperInfoType.ApplicationInstallerName  => AppalachiaApplication.InstallerName,
                    DeveloperInfoType.ApplicationSystemLanguage => AppalachiaApplication.SystemLanguage.ToString(),
                    DeveloperInfoType.ApplicationUnityVersion   => AppalachiaApplication.UnityVersion,
                    DeveloperInfoType.FrameCount                => CoreClock.Instance.FrameCount.ToString(),
                    DeveloperInfoType.TimeScale                 => CoreClock.Instance.TimeScale.ToStringNonAlloc(3),
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
                    DeveloperInfoType.SystemMemory => FORMAT_SYSTEM_MEMORY.Format(SystemInfo.systemMemorySize),
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
                    DeveloperInfoType.MachineName            => Environment.MachineName,
                    DeveloperInfoType.UserName               => Environment.UserName,
                    DeveloperInfoType.SceneList              => string.Join('\n', GetLoadedSceneNames()),
                    DeveloperInfoType.ProfilingFPSAverage    => _fpsProfilerService.AverageFPS.ToString(),
                    DeveloperInfoType.ProfilingFPSMin        => _fpsProfilerService.MinimumFPS.ToString(),
                    DeveloperInfoType.ProfilingFPSMax        => _fpsProfilerService.MaximumFPS.ToString(),
                    DeveloperInfoType.ProfilingFPSCurrent    => FORMAT_FPS.Format(_fpsProfilerService.CurrentFPS),
                    DeveloperInfoType.ProfilingFPSFrame      => FORMAT_MILLISECONDS.Format(_fpsProfilerService.LastFrame*1000f),
                    DeveloperInfoType.ProfilingRAMMono       => FORMAT_MEGABYTES.Format(_memoryProfilerService.MonoRAM),
                    DeveloperInfoType.ProfilingRAMAllocated  => FORMAT_MEGABYTES.Format(_memoryProfilerService.AllocatedRam),
                    DeveloperInfoType.ProfilingRAMReserved   => FORMAT_MEGABYTES.Format(_memoryProfilerService.ReservedRam),
                    DeveloperInfoType.ProfilingAudioDecibals => FORMAT_DECIBALS.Format(_audioProfilerService.MaxDB),
                    _                                        => throw new ArgumentOutOfRangeException()
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
                    case DeveloperInfoType.ActiveSceneName:
                    case DeveloperInfoType.ActiveScenePath:
                    case DeveloperInfoType.SceneCount:
                    case DeveloperInfoType.SceneList:
                    case DeveloperInfoType.ActiveSceneNameAndCount:
                    case DeveloperInfoType.FrameCount:
                    case DeveloperInfoType.TimeScale:
                    case DeveloperInfoType.WindowSize:
                    case DeveloperInfoType.ScreenResolution:
                    case DeveloperInfoType.ProfilingFPSAverage:
                    case DeveloperInfoType.ProfilingFPSMin:
                    case DeveloperInfoType.ProfilingFPSMax:
                    case DeveloperInfoType.ProfilingFPSCurrent:
                    case DeveloperInfoType.ProfilingFPSFrame:
                    case DeveloperInfoType.ProfilingRAMMono:
                    case DeveloperInfoType.ProfilingRAMAllocated:
                    case DeveloperInfoType.ProfilingRAMReserved:
                    case DeveloperInfoType.ProfilingAudioDecibals:
                    default:
                        return false;
                }
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
            }
        }

        private IEnumerable<string> GetLoadedSceneNames()
        {
            var sceneCount = SceneManager.sceneCount;
            var activeScene = SceneManager.GetActiveScene();

            for (var sceneIndex = 0; sceneIndex < sceneCount; sceneIndex++)
            {
                var scene = SceneManager.GetSceneAt(sceneIndex);
                if (scene == activeScene)
                {
                    yield return scene.name.Bold();
                }
                else
                {
                    yield return scene.name;
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetLabel = new ProfilerMarker(_PRF_PFX + nameof(GetLabel));

        private static readonly ProfilerMarker _PRF_GetUpdatedText =
            new ProfilerMarker(_PRF_PFX + nameof(GetUpdatedText));

        private static readonly ProfilerMarker _PRF_IsImmutableValue =
            new ProfilerMarker(_PRF_PFX + nameof(IsImmutableValue));

        #endregion
    }
}
