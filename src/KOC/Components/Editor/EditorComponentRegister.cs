#if UNITY_EDITOR

using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Attributes;
using Appalachia.Utility.Execution;
using Doozy.Engine.Nody.Models;

namespace Appalachia.Prototype.KOC.Components
{
    [CallStaticConstructorInEditor]
    public static class EditorComponentRegister
    {
        static EditorComponentRegister()
        {
            if (AppalachiaApplication.IsPlayingOrWillPlay)
            {
                return;
            }

            AssetDatabaseManager.RegisterAdditionalAssetTypeFolders<Graph>((_, _) => "Doozy/Graphs");
        }
    }
}

#endif
