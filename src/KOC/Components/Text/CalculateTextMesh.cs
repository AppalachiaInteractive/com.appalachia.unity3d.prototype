using System;
using System.Collections;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable FormatStringProblem

namespace Appalachia.Prototype.KOC.Components.Text
{
    [SmartLabelChildren]
    public class CalculateTextMesh : DynamicTextMesh<CalculateTextMesh>
    {
        public enum Calculation
        {
            None = 0,
            ActiveSceneName = 100,
            ActiveScenePath = 101,
            SceneCount = 102,
            ActiveSceneNameAndCount = 110,

            ApplicationPlatform = 200,
            ApplicationVersion = 201,
            ApplicationBuildGuid = 202,
            ApplicationProductName = 203,
            ApplicationCompanyName = 204,
            ApplicationInstallerName = 205,
            ApplicationSystemLanguage = 206,
            ApplicationUnityVersion = 207,

            FrameCount = 300,
            TimeScale = 301,

            WindowSize = 400,
            ScreenResolution = 401,

            ProcessorType = 500,

            SystemMemory = 600,

            GraphicsDeviceVersion = 700,
            GraphicsDeviceName = 701,
            GraphicsMemorySize = 702,

            OperatingSystem = 800
        }

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

        #region Fields and Autoproperties

        [FoldoutGroup("Settings")]
        [PropertyRange(1, 10)]
        public float updateRate;

        [FoldoutGroup("Settings")]
        [OnValueChanged(nameof(InitializeSynchronous))]
        public Calculation calculation;

        private SafeCoroutineWrapper _wrapper;

        private EnumNamesCollection<Calculation> _enumNames;

        private bool _shouldUpdate;

        #endregion

        #region Event Functions

        protected void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                if (IsImmutableValue(calculation))
                {
                    return;
                }

                if (_wrapper is { IsExecuting: true })
                {
                    return;
                }

                if (_wrapper == null)
                {
                    Execute();
                }
                else if (!_wrapper.IsExecuting)
                {
                    if (IsImmutableValue(calculation))
                    {
                        UpdateText();
                        return;
                    }

                    Execute();
                }
            }
        }

        #endregion

        protected override string GetLabel()
        {
            using (_PRF_GetLabel.Auto())
            {
                switch (calculation)
                {
                    case Calculation.None:
                        return "None";
                    case Calculation.ActiveSceneName:
                        return "Active Scene";
                    case Calculation.ActiveScenePath:
                        return "Scene Path";
                    case Calculation.SceneCount:
                        return "Scene Count";
                    case Calculation.ActiveSceneNameAndCount:
                        return "Active Scene";
                    case Calculation.ApplicationPlatform:
                        return "Platform";
                    case Calculation.ApplicationVersion:
                        return "Version";
                    case Calculation.ApplicationBuildGuid:
                        return "Build Guid";
                    case Calculation.ApplicationProductName:
                        return "Product Name";
                    case Calculation.ApplicationCompanyName:
                        return "Company Name";
                    case Calculation.ApplicationInstallerName:
                        return "Installer";
                    case Calculation.ApplicationSystemLanguage:
                        return "Sys. Language";
                    case Calculation.ApplicationUnityVersion:
                        return "Unity Version";
                    case Calculation.FrameCount:
                        return "Frame Count";
                    case Calculation.TimeScale:
                        return "Time Scale";
                    case Calculation.WindowSize:
                        return "Window";
                    case Calculation.ScreenResolution:
                        return "Screen";
                    case Calculation.ProcessorType:
                        return "Processor";
                    case Calculation.SystemMemory:
                        return "Memory";
                    case Calculation.GraphicsDeviceVersion:
                        return "GPU API";
                    case Calculation.GraphicsDeviceName:
                        return "GPU Name";
                    case Calculation.GraphicsMemorySize:
                        return "GPU Memory";
                    case Calculation.OperatingSystem:
                        return "OS";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected override string GetName()
        {
            _enumNames ??= new EnumNamesCollection<Calculation>();

            return _enumNames[calculation];
        }

        protected override string GetUpdatedText()
        {
            using (_PRF_GetUpdatedText.Auto())
            {
                _lastText = calculation switch
                {
                    Calculation.None                      => string.Empty,
                    Calculation.ActiveSceneName           => SceneManager.GetActiveScene().name,
                    Calculation.ActiveScenePath           => SceneManager.GetActiveScene().path,
                    Calculation.SceneCount                => SceneManager.sceneCount.ToString(),
                    Calculation.ApplicationPlatform       => AppalachiaApplication.Platform.ToString(),
                    Calculation.ApplicationVersion        => AppalachiaApplication.Version,
                    Calculation.ApplicationBuildGuid      => AppalachiaApplication.BuildGuid,
                    Calculation.ApplicationProductName    => AppalachiaApplication.ProductName,
                    Calculation.ApplicationCompanyName    => AppalachiaApplication.CompanyName,
                    Calculation.ApplicationInstallerName  => AppalachiaApplication.InstallerName,
                    Calculation.ApplicationSystemLanguage => AppalachiaApplication.SystemLanguage.ToString(),
                    Calculation.ApplicationUnityVersion   => AppalachiaApplication.UnityVersion,
                    Calculation.FrameCount                => Time.frameCount.ToString(),
                    Calculation.TimeScale                 => Time.timeScale.ToStringNonAlloc(3),
                    Calculation.ActiveSceneNameAndCount => FORMAT_SCENE_NAME_AND_COUNT.Format(
                        SceneManager.GetActiveScene().name,
                        SceneManager.sceneCount
                    ),
                    Calculation.WindowSize => FORMAT_WINDOW_SIZE.Format(
                        Screen.width,
                        Screen.height,
                        Screen.currentResolution.refreshRate,
                        Screen.dpi.ToStringNonAlloc(0)
                    ),
                    Calculation.ScreenResolution => FORMAT_SCREEN_RESOLUTION.Format(
                        Screen.currentResolution.width,
                        Screen.currentResolution.height,
                        Screen.currentResolution.refreshRate
                    ),
                    Calculation.ProcessorType => FORMAT_PROCESSOR_TYPE.Format(
                        SystemInfo.processorType,
                        SystemInfo.processorCount
                    ),
                    Calculation.SystemMemory => FORMAT_SYSTEM_MEMORY.Format(SystemInfo.systemMemorySize),
                    Calculation.GraphicsDeviceVersion => FORMAT_GRAPHICS_DEVICE_VERSION.Format(
                        SystemInfo.graphicsDeviceVersion
                    ),
                    Calculation.GraphicsDeviceName => FORMAT_GRAPHICS_DEVICE_NAME.Format(
                        SystemInfo.graphicsDeviceName
                    ),
                    Calculation.GraphicsMemorySize => FORMAT_GRAPHICS_MEMORY_SIZE.Format(
                        SystemInfo.graphicsMemorySize,
                        SystemInfo.maxTextureSize,
                        SystemInfo.graphicsShaderLevel
                    ),
                    Calculation.OperatingSystem => FORMAT_OPERATING_SYSTEM.Format(
                        SystemInfo.operatingSystem,
                        SystemInfo.deviceType
                    ),
                    _ => throw new ArgumentOutOfRangeException()
                };

                return _lastText;
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            UpdateText();
        }

        protected override async AppaTask WhenDisabled()
        {
            await base.WhenDisabled();

            using (_PRF_WhenDisabled.Auto())
            {
                if (_wrapper?.IsExecuting ?? false)
                {
                    _wrapper?.Cancel();
                }
            }
        }

        private static bool IsImmutableValue(Calculation x)
        {
            using (_PRF_IsImmutableValue.Auto())
            {
                switch (x)
                {
                    case Calculation.None:
                    case Calculation.ApplicationPlatform:
                    case Calculation.ApplicationVersion:
                    case Calculation.ApplicationBuildGuid:
                    case Calculation.ApplicationProductName:
                    case Calculation.ApplicationCompanyName:
                    case Calculation.ApplicationInstallerName:
                    case Calculation.ApplicationSystemLanguage:
                    case Calculation.ApplicationUnityVersion:
                    case Calculation.ProcessorType:
                    case Calculation.SystemMemory:
                    case Calculation.GraphicsDeviceVersion:
                    case Calculation.GraphicsDeviceName:
                    case Calculation.GraphicsMemorySize:
                    case Calculation.OperatingSystem:
                        return true;
                    default:
                        return false;
                }
            }
        }

        private void Execute()
        {
            using (_PRF_Execute.Auto())
            {
                var enumerator = ExecuteEnumerator();

                _wrapper = new SafeCoroutineWrapper(
                    enumerator,
                    ZString.Format("{0}_{1}_{2}", nameof(CalculateTextMesh), name, GetHashCode())
                );

                _wrapper.ExecuteAsCoroutine();
            }
        }

        private IEnumerator ExecuteEnumerator()
        {
            _shouldUpdate = true;

            while (_shouldUpdate)
            {
                if (!DependenciesAreReady || !FullyInitialized)
                {
                    yield return null;
                    continue;
                }

                if (calculation == Calculation.None)
                {
                    _shouldUpdate = false;

                    yield break;
                }

                if (IsImmutableValue(calculation))
                {
                    _shouldUpdate = false;

                    UpdateText();

                    yield break;
                }

                if (!_shouldUpdate)
                {
                    _shouldUpdate = true;
                }

                UpdateText();

                yield return new WaitForSecondsRealtime(1f / updateRate);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_Execute = new ProfilerMarker(_PRF_PFX + nameof(Execute));

        private static readonly ProfilerMarker
            _PRF_GetLabel = new ProfilerMarker(_PRF_PFX + nameof(GetLabel));

        private static readonly ProfilerMarker _PRF_IsImmutableValue =
            new ProfilerMarker(_PRF_PFX + nameof(IsImmutableValue));

        private static readonly ProfilerMarker _PRF_GetUpdatedText = new(_PRF_PFX + nameof(GetUpdatedText));

        #endregion
    }
}
