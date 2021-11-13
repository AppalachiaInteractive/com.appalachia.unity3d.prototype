using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application.Collections;
using Appalachia.Utility.Logging;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Scenes
{
    public class SceneBootloadData : CategorizableAutonamedIdentifiableAppalachiaObject<SceneBootloadData>
    {
        #region Profiling

        private const string _PRF_PFX = nameof(SceneBootloadData) + ".";

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));

        #endregion

        #region Fields

        public ApplicationArea area;

        [PropertyOrder(140)]
        [NonSerialized, ShowInInspector]
        public SceneBootloadProgress entrySceneBootloadProgress;

        [PropertyOrder(150)]
        [NonSerialized, ShowInInspector]
        public List<SceneBootloadProgress> bootloadProgress;

        [PropertyOrder(80)] public SceneReference entryScene;

        [PropertyOrder(90)]
        [SerializeField]
        private AppaList_SceneReference _scenes;

        #endregion

        #region Event Functions

        private void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                AppaLog.Context.Bootload.Info(nameof(Awake));

                if (bootloadProgress == null)
                {
                    bootloadProgress = new List<SceneBootloadProgress>();
                }

                if (_scenes == null)
                {
                    _scenes = new AppaList_SceneReference();
#if UNITY_EDITOR
                    SetDirty();
#endif
                }
            }
        }

        #endregion

        public IEnumerable<SceneReference> GetScenesToLoad()
        {
            AppaLog.Context.Bootload.Info(nameof(GetScenesToLoad));

            foreach (var scene in _scenes)
            {
                yield return scene;
            }
        }
#if UNITY_EDITOR

        private static readonly ProfilerMarker _PRF_CreateScene =
            new ProfilerMarker(_PRF_PFX + nameof(CreateScene));

        [Button]
        [PropertyOrder(101)]
        [SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
        private void CreateScene()
        {
            using (_PRF_CreateScene.Auto())
            {
                AppaLog.Context.Bootload.Info(nameof(CreateScene));

                var otherScene = GetScenesToLoad().FirstOrDefault();

                if (otherScene == null)
                {
                    var candidateScenes =
                        AreaSceneBootloadDataCollection.instance.all.SelectMany(s => s._scenes);

                    otherScene = candidateScenes.FirstOrDefault(s => s != null);
                }

                var otherPath = otherScene.AssetPath;
                var otherDirectory = AppaPath.GetDirectoryName(otherPath);

                var sceneName = $"{name}_{_scenes.Count}";
                var outputPath = AppaPath.Combine(otherDirectory, $"{sceneName}.unity");

                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);

                EditorSceneManager.SaveScene(scene, outputPath);
                AssetDatabaseManager.Refresh();

                EditorSceneManager.CloseScene(scene, true);

                var asset = AssetDatabaseManager.LoadAssetAtPath<SceneAsset>(outputPath);
                var reference = SceneReference.CreateNew(sceneName);

                reference.SetSelection(asset);

                /*if (_specifyFirst && (_first == null))
            {
                _first = reference;
            }
            else if (_specifyLast && (_last == null))
            {
                _last = reference;
            }
            else
            {*/
                if (_scenes == null)
                {
                    _scenes = new AppaList_SceneReference();
                }

                _scenes.Add(reference);

                SetDirty();

                //}
            }
        }

        private static readonly ProfilerMarker _PRF_CreateAsset =
            new ProfilerMarker(_PRF_PFX + nameof(CreateAsset));

        [MenuItem(PKG.Menu.Assets.Base + nameof(SceneBootloadData), priority = PKG.Menu.Assets.Priority)]
        public static void CreateAsset()
        {
            using (_PRF_CreateAsset.Auto())
            {
                CreateNew();
            }
        }
#endif
    }
}
