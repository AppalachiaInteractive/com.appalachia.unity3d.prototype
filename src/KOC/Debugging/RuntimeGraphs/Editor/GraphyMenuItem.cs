#if UNITY_EDITOR
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Strings;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Editor
{
    public class GraphyMenuItem : AppalachiaSimpleBase
    {
        #region Menu Items

        [MenuItem(PKG.Menu.Appalachia.Tools.Base + "Graphy/Create Prefab Variant")]
        private static void CreatePrefabVariant()
        {
            // Directory checking
            if (!AssetDatabaseManager.IsValidFolder("Assets/Graphy - Ultimate Stats Monitor"))
            {
                AssetDatabaseManager.CreateFolder("Assets", "Graphy - Ultimate Stats Monitor");
            }

            if (!AssetDatabaseManager.IsValidFolder("Assets/Graphy - Ultimate Stats Monitor/Prefab Variants"))
            {
                AssetDatabaseManager.CreateFolder(
                    "Assets/Graphy - Ultimate Stats Monitor",
                    "Prefab Variants"
                );
            }

            var graphyPrefabGuid = AssetDatabaseManager.FindAssets("[Graphy]")[0];

            Object originalPrefab = (GameObject)AssetDatabaseManager.LoadAssetAtPath(
                AssetDatabaseManager.GUIDToAssetPath(graphyPrefabGuid),
                typeof(GameObject)
            );
            var objectSource = PrefabUtility.InstantiatePrefab(originalPrefab) as GameObject;

            var prefabVariantCount = AssetDatabaseManager.FindAssets(
                                                              "Graphy_Variant",
                                                              new[]
                                                                  {
                                                                      "Assets/Graphy - Ultimate Stats Monitor/Prefab Variants"
                                                                  }
                                                          )
                                                         .Length;

            var prefabVariant = PrefabUtility.SaveAsPrefabAsset(
                objectSource,
                ZString.Format(
                    "Assets/Graphy - Ultimate Stats Monitor/Prefab Variants/Graphy_Variant_{0}.prefab",
                    prefabVariantCount
                )
            );

            Object.DestroyImmediate(objectSource);

            foreach (SceneView scene in SceneView.sceneViews)
            {
                scene.ShowNotification(
                    new GUIContent(
                        "Prefab Variant Created at \"Assets/Graphy - Ultimate Stats Monitor/Prefab\"!"
                    )
                );
            }
        }

        [MenuItem(PKG.Menu.Appalachia.Tools.Base + "Graphy/Import Graphy Customization Scene")]
        private static void ImportGraphyCustomizationScene()
        {
            var customizationSceneGuid = AssetDatabaseManager.FindAssets("Graphy_CustomizationScene")[0];

            AssetDatabaseManager.ImportPackage(
                AssetDatabaseManager.GUIDToAssetPath(customizationSceneGuid),
                true
            );
        }

        #endregion
    }
}

#endif
