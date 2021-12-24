using System;
using System.Linq;
using Appalachia.CI.Integration.Assets;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Utility.Strings;
using UnityEngine.AddressableAssets;

namespace Appalachia.Prototype.KOC.Application.Scenes
{
    public static class SceneExtensions
    {
#if UNITY_EDITOR

        public static AssetReference ToAssetReference(this UnityEditor.SceneAsset sceneAsset)
        {
            AssetDatabaseManager.TryGetGUIDAndLocalFileIdentifier(sceneAsset, out var guid, out _);

            var reference = new AssetReference(guid);

            return reference;
        }

        public static SceneReferenceElement ToSceneReference(this UnityEditor.SceneAsset asset)
        {
            var match = AssetDatabaseManager.FindAssets<SceneReferenceElement>();

            if ((match == null) || (match.Count == 0))
            {
                return null;
            }

            var result = match.FirstOrDefault(m => m.sceneAsset == asset);

            var extension = ".asset";
            var targetNameWithoutExtension = ZString.Format(
                "{0}_{1}",
                nameof(SceneReferenceElement),
                asset.name
            );
            var targetName = ZString.Format("{0}{1}", targetNameWithoutExtension, extension);

            if (result != null)
            {
                if (result.name != targetNameWithoutExtension)
                {
                    result.Rename(targetName);
                }
            }
            else
            {
                result = SceneReferenceElement.LoadOrCreateNew(targetNameWithoutExtension);
            }

            if (result.version == AreaVersion.None)
            {
                if (asset.name[^3] == 'V')
                {
                    result.version = Enum.Parse<AreaVersion>(asset.name[^3..]);
                }
            }

            return result;
        }
#endif
    }
}
