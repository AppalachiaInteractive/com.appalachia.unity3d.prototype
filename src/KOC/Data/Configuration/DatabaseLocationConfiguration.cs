using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Data.Core;
using Appalachia.Utility.Extensions;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Data.Configuration
{
    [Serializable, DoNotReorderFields]
    public class DatabaseLocationConfiguration
    {
        #region Constants and Static Readonly

        private const string PATH_PROTOTYPE_HARDCODED = "Appalachia.Prototype/asset/Database";

        #endregion

        public DatabaseLocationConfiguration(
            DataLocation location,
            bool includeCompanyName,
            bool includeProductName,
            string additional)
        {
            this.location = location;
            this.includeCompanyName = includeCompanyName;
            this.includeProductName = includeProductName;
            this.additional = additional;
        }

        #region Fields and Autoproperties

        public DataLocation location;
        public bool includeCompanyName;
        public bool includeProductName;
        public string additional;

        #endregion

        public string GetBasePath()
        {
            using (_PRF_GetBasePath.Auto())
            {
                var outputPath = GetLocationPath();

                if (includeCompanyName)
                {
                    outputPath = AppaPath.Combine(outputPath, PKG.AssemblyCompany);
                }

                if (includeProductName)
                {
                    outputPath = AppaPath.Combine(outputPath, PKG.AssemblyProduct);
                }

                if (additional.IsNotNullOrWhiteSpace())
                {
                    outputPath = AppaPath.Combine(outputPath, additional);
                }

                return outputPath;
            }
        }

        private string GetLocationPath()
        {
            using (_PRF_GetLocationPath.Auto())
            {
                switch (location)
                {
                    case DataLocation.Desktop:
                        return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    case DataLocation.MyDocuments:
                        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    case DataLocation.StreamingAssetsDirectory:
                        return UnityEngine.Application.streamingAssetsPath;
                    case DataLocation.PrototypeHardcoded:
                        return AppaPath.Combine(UnityEngine.Application.dataPath, PATH_PROTOTYPE_HARDCODED);
                    case DataLocation.ApplicationData:
                        return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    case DataLocation.LocalApplicationData:
                        return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    case DataLocation.CommonApplicationData:
                        return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(location), location, null);
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DatabaseLocationConfiguration) + ".";

        private static readonly ProfilerMarker _PRF_GetLocationPath =
            new ProfilerMarker(_PRF_PFX + nameof(GetLocationPath));

        private static readonly ProfilerMarker _PRF_GetBasePath =
            new ProfilerMarker(_PRF_PFX + nameof(GetBasePath));

        #endregion
    }
}
