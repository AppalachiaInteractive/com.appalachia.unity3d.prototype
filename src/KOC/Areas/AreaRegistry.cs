using System.Collections.Generic;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas
{
    public static class AreaRegistry
    {
        #region Static Fields and Autoproperties

        private static Dictionary<ApplicationArea, IAreaManager> _managerLookup =
            new Dictionary<ApplicationArea, IAreaManager>();

        private static Dictionary<ApplicationArea, IAreaMetadata> _metadataLookup =
            new Dictionary<ApplicationArea, IAreaMetadata>();

        #endregion

        public static IAreaManager GetManager(ApplicationArea area)
        {
            using (_PRF_GetManager.Auto())
            {
                Initialize();

                if (_managerLookup.TryGetValue(area, out var result)) return result;

                return null;
            }
        }

        public static IAreaMetadata GetMetadata(ApplicationArea area)
        {
            using (_PRF_GetMetadata.Auto())
            {
                Initialize();

                if (_metadataLookup.TryGetValue(area, out var result)) return result;

                return null;
            }
        }

        public static IAreaManager GetParentManager(IAreaManager manager)
        {
            var parentArea = manager.ParentArea;

            if (parentArea == ApplicationArea.None)
            {
                return null;
            }

            var parentManager = GetManager(parentArea);

            return parentManager;
        }

        public static void RegisterManager<TManager, TMetadata>(AreaManager<TManager, TMetadata> manager)
            where TManager : AreaManager<TManager, TMetadata>
            where TMetadata : AreaMetadata<TManager, TMetadata>
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

        public static void RegisterMetadata<TManager, TMetadata>(AreaMetadata<TManager, TMetadata> metadata)
            where TManager : AreaManager<TManager, TMetadata>
            where TMetadata : AreaMetadata<TManager, TMetadata>
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

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_GetManager =
            new ProfilerMarker(_PRF_PFX + nameof(GetManager));

        #endregion
    }
}
