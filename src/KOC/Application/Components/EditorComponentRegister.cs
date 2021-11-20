#if UNITY_EDITOR

using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Attributes;
using Doozy.Engine.Nody.Models;

namespace Appalachia.Prototype.KOC.Application.Components
{
    [InitializeOnLoadAlways]
    public static class EditorComponentRegister
    {
        static EditorComponentRegister()
        {
            AssetDatabaseManager.RegisterAdditionalAssetTypeFolders<Graph>((_, _) => "Doozy/Graphs");
        }
    }
}

#endif
