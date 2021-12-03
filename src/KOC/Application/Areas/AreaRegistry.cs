using System.Collections.Generic;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Areas
{
    public static class AreaRegistry
    {
        #region Static Fields and Autoproperties

        private static ApplicationManager _applicationManager;

        private static Dictionary<ApplicationArea, IAreaManager> _managerLookup =
            new Dictionary<ApplicationArea, IAreaManager>();

        private static Dictionary<ApplicationArea, IAreaMetadata> _metadataLookup =
            new Dictionary<ApplicationArea, IAreaMetadata>();

        #endregion

        public static ApplicationManager GetApplicationManager()
        {
            using (_PRF_GetApplicationManager.Auto())
            {
                return _applicationManager;
            }
        }

        public static IAreaManager GetManager(ApplicationArea area)
        {
            using (_PRF_GetManager.Auto())
            {
                Initialize();

                if (_managerLookup.ContainsKey(area))
                {
                    return _managerLookup[area];
                }

                return null;
            }
        }

        public static IAreaMetadata GetMetadata(ApplicationArea area)
        {
            using (_PRF_GetMetadata.Auto())
            {
                Initialize();

                if (_metadataLookup.ContainsKey(area))
                {
                    return _metadataLookup[area];
                }

                return null;
            }
        }

        public static void RegisterApplicationManager(ApplicationManager manager)
        {
            using (_PRF_RegisterApplicationManager.Auto())
            {
                if (manager == null)
                {
                    return;
                }

                _applicationManager = manager;
            }
        }

        public static void RegisterManager<T, TM>(AreaManager<T, TM> manager)
            where T : AreaManager<T, TM>
            where TM : AreaMetadata<T, TM>
        {
            using (_PRF_RegisterManager.Auto())
            {
                Initialize();

                if (_managerLookup.ContainsKey(manager.Area))
                {
                    _managerLookup[manager.Area] = manager;
                }
                else
                {
                    _managerLookup.Add(manager.Area, manager);
                }
            }
        }

        public static void RegisterMetadata<T, TM>(AreaMetadata<T, TM> metadata)
            where T : AreaManager<T, TM>
            where TM : AreaMetadata<T, TM>
        {
            using (_PRF_RegisterMetadata.Auto())
            {
                Initialize();

                if (_metadataLookup.ContainsKey(metadata.Area))
                {
                    _metadataLookup[metadata.Area] = metadata;
                }
                else
                {
                    _metadataLookup.Add(metadata.Area, metadata);
                }
            }
        }

        private static void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                _managerLookup ??= new Dictionary<ApplicationArea, IAreaManager>();
                _metadataLookup ??= new Dictionary<ApplicationArea, IAreaMetadata>();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AreaRegistry) + ".";

        private static readonly ProfilerMarker _PRF_GetMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(GetMetadata));

        private static readonly ProfilerMarker _PRF_RegisterManager =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterManager));

        private static readonly ProfilerMarker _PRF_RegisterMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterMetadata));

        private static readonly ProfilerMarker _PRF_RegisterApplicationManager =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterApplicationManager));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_GetManager =
            new ProfilerMarker(_PRF_PFX + nameof(GetManager));

        private static readonly ProfilerMarker _PRF_GetApplicationManager =
            new ProfilerMarker(_PRF_PFX + nameof(GetApplicationManager));

        #endregion
    }
}
