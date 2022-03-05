namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Models
{
    public enum DeveloperInfoType
    {
        None = 0,
        ActiveSceneName = 100,
        ActiveScenePath = 101,
        SceneCount = 102,
        SceneList = 103,
        
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

        OperatingSystem = 800,
        MachineName = 801,
        UserName = 802,
        
        ProfilingFPSAverage = 1000,
        ProfilingFPSMin = 1001,
        ProfilingFPSMax = 1002,
        ProfilingFPSCurrent = 1003,
        ProfilingFPSFrame = 1004,
        
        ProfilingRAMMono = 1100,
        ProfilingRAMAllocated = 1101,
        ProfilingRAMReserved = 1102,
        
        ProfilingAudioDecibals = 1200,
    }
}
