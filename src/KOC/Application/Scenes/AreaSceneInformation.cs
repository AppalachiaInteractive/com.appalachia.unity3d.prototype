using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Core;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Prototype.KOC.Application.Collections;
using Appalachia.Prototype.KOC.Application.Scriptables;
using Appalachia.Prototype.KOC.Application.State;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Application.Scenes
{
    public class AreaSceneInformation : AppalachiaApplicationObject
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("area")]
        [SerializeField]
        private ApplicationArea _area;

        [PropertyOrder(150)]
        [NonSerialized, ShowInInspector]
        public AppaList_BootloadedScene scenes;

        [PropertyOrder(140)]
        [NonSerialized, ShowInInspector]
        public BootloadedScene entryScene;

        [FormerlySerializedAs("entryScene")]
        [PropertyOrder(80)]
        public SceneReference entrySceneReference;

        [FormerlySerializedAs("_scenes")]
        [PropertyOrder(90)]
        [SerializeField]
        private AppaList_SceneReference _sceneReferences;

        #endregion

        public ApplicationArea Area => _area;

        #region Event Functions

        protected override void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                base.Awake();
                AppaLog.Context.Bootload.Info(nameof(Awake));

                if (scenes == null)
                {
                    scenes = new AppaList_BootloadedScene();
                }

                if (_sceneReferences == null)
                {
                    _sceneReferences = new AppaList_SceneReference();
#if UNITY_EDITOR
                    this.MarkAsModified();
#endif
                }
            }
        }

        #endregion

        public void CheckAreaLoadState(ApplicationAreaState state)
        {
            using (_PRF_CheckAreaLoadState.Auto())
            {
                if ((_area == ApplicationArea.None) || (state == null) || state.IsAtRest)
                {
                    return;
                }

                AppaLog.Context.Bootload.Trace($"{nameof(CheckAreaLoadState)}: {state.Area}");

                EvaluateAreaState(state, HandleBootloadTransition);
            }
        }

        public IEnumerable<SceneReference> GetScenesToLoad()
        {
            AppaLog.Context.Bootload.Info(nameof(GetScenesToLoad));

            foreach (var scene in _sceneReferences)
            {
                yield return scene;
            }
        }

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();
#if UNITY_EDITOR
                if (name == "None")
                {
                    return;
                }

                if (_area == ApplicationArea.None)
                {
                    try
                    {
                        _area = Enum.Parse<ApplicationArea>(name, false);
                        this.MarkAsModified();
                    }
                    catch (Exception e)
                    {
                        AppaLog.Exception(
                            $"The name [{name}] of {nameof(AreaSceneInformation)} is not in {nameof(ApplicationArea)} Enum.",
                            e,
                            this
                        );
                    }
                }

                if (entrySceneReference == null)
                {
                    entrySceneReference =
                        AppalachiaObjectFactory.LoadExistingOrCreateNewAsset<SceneReference>(name);
                    this.MarkAsModified();
                    if (entrySceneReference == null)
                    {
                        AppaLog.Error(
                            $"The name [{name}] of {nameof(AreaSceneInformation)} does not have a matching {nameof(SceneReference)}.",
                            this
                        );
                    }
                }

                if ((entrySceneReference != null) && (entrySceneReference.sceneAsset == null))
                {
                    entrySceneReference.sceneAsset =
                        AssetDatabaseManager.FindFirstAssetMatch<UnityEditor.SceneAsset>(name);

                    if (entrySceneReference.sceneAsset == null)
                    {
                        AppaLog.Error(
                            $"The name [{name}] of {nameof(AreaSceneInformation)} does not have a matching {nameof(UnityEditor.SceneAsset)}.",
                            this
                        );
                    }
                }

                if ((entrySceneReference != null) &&
                    (entrySceneReference.sceneAsset != null) &&
                    (entrySceneReference.reference?.Asset == null))
                {
                    entrySceneReference.SetSelection(entrySceneReference.sceneAsset);
                }
#endif
            }
        }

        /*
         
        private static readonly ProfilerMarker _PRF_BootloadAllScenes =
            new ProfilerMarker(_PRF_PFX + nameof(BootloadAllScenes));
            
         private static IEnumerator BootloadAllScenes(SceneBootloadData bootloadData)
        {
            AppaLog.Context.Bootload.Info(nameof(BootloadAllScenes));

            if (bootloadData.entrySceneReference == null)
            {
                throw new NotSupportedException();
            }

            var sceneReferences = bootloadData.GetScenesToLoad();

            foreach (var sceneReference in sceneReferences)
            {
                var assetReference = sceneReference.reference;

                var newOperation = assetReference.LoadSceneAsync(LoadSceneMode.Additive, false);

                bootloadProgress.Add(newOperation);

                yield return null;
            }
        }*/

        private void EvaluateAreaState(ApplicationAreaState state, BootloadTransitionHandler handler)
        {
            using (_PRF_CheckAreaLoadStateInternal.Auto())
            {
                AppaLog.Context.Bootload.Trace($"{nameof(EvaluateAreaState)}: {state.Area}");

                if (!state.HasStateChangedTriggered)
                {
                    return;
                }

                var result = new BootloadTransitionArgs { state = state };
                var completedResult = new BootloadTransitionArgs
                {
                    state = state, stateTransition = state.MarkComplete
                };
                var failedResult = new BootloadTransitionArgs
                {
                    state = state, stateTransition = state.MarkFailed
                };

                if (entryScene == null)
                {
                    entryScene = new BootloadedScene(entrySceneReference);
                }

                result.stateTransition = state.MarkStateTransitionCompleted;

                switch (state.NextState)
                {
                    case ApplicationAreaStates.Load:
                        result.action = entryScene.Load;

                        entryScene.OnLoadComplete += _ => handler(completedResult);
                        entryScene.OnLoadFailed += _ => handler(failedResult);

                        break;
                    case ApplicationAreaStates.Activate:
                        result.action = entryScene.Activate;

                        entryScene.OnActivateComplete += _ => handler(completedResult);

                        //entryScene.OnActivateFailed += _ => handler(failedResult);

                        break;
                    case ApplicationAreaStates.Unload:
                        result.action = entryScene.Unload;

                        entryScene.OnUnloadComplete += _ => handler(completedResult);
                        entryScene.OnUnloadFailed += _ => handler(failedResult);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                handler?.Invoke(result);
            }
        }

        private void HandleBootloadTransition(BootloadTransitionArgs args)
        {
            using (_PRF_HandleBootloadTransition.Auto())
            {
                AppaLog.Context.Bootload.Info(
                    $"Area [{args.state.Area}] is transitioning from [{args.state.State}:{args.state.Substate}]."
                );

                try
                {
                    if (args.action == null)
                    {
                        args.stateTransition?.Invoke();

                        return;
                    }

                    args.action();

                    args.stateTransition?.Invoke();
                }
                catch (Exception ex)
                {
                    AppaLog.Exception(
                        $"Error executing state change from [{args.state.State}:{args.state.Substate}].",
                        ex
                    );
                }
            }
        }

        #region Nested type: BootloadTransitionArgs

        private class BootloadTransitionArgs
        {
            #region Fields and Autoproperties

            public Action action;
            public Action stateTransition;
            public ApplicationAreaState state;

            #endregion
        }

        #endregion

        #region Nested type: BootloadTransitionHandler

        private delegate void BootloadTransitionHandler(BootloadTransitionArgs args);

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AreaSceneInformation) + ".";

        private static readonly ProfilerMarker _PRF_HandleBootloadTransition =
            new ProfilerMarker(_PRF_PFX + nameof(HandleBootloadTransition));

        private static readonly ProfilerMarker _PRF_CheckAreaLoadStateInternal =
            new ProfilerMarker(_PRF_PFX + nameof(EvaluateAreaState));

        private static readonly ProfilerMarker _PRF_CheckAreaLoadState =
            new ProfilerMarker(_PRF_PFX + nameof(CheckAreaLoadState));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));

        #endregion

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
                        AreaSceneInformationCollection.instance.all.SelectMany(s => s._sceneReferences);

                    otherScene = candidateScenes.FirstOrDefault(s => s != null);
                }

                var otherPath = otherScene.AssetPath;
                var otherDirectory = AppaPath.GetDirectoryName(otherPath);

                var sceneName = $"{name}_{_sceneReferences.Count}";
                var outputPath = AppaPath.Combine(otherDirectory, $"{sceneName}.unity");

                var scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(
                    UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,
                    UnityEditor.SceneManagement.NewSceneMode.Additive
                );

                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene, outputPath);
                AssetDatabaseManager.Refresh();

                UnityEditor.SceneManagement.EditorSceneManager.CloseScene(scene, true);

                var asset = AssetDatabaseManager.LoadAssetAtPath<UnityEditor.SceneAsset>(outputPath);
                var reference = CreateNew<SceneReference>(sceneName);

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
                if (_sceneReferences == null)
                {
                    _sceneReferences = new AppaList_SceneReference();
                }

                _sceneReferences.Add(reference);

                this.MarkAsModified();

                //}
            }
        }

        private static readonly ProfilerMarker _PRF_CreateAsset =
            new ProfilerMarker(_PRF_PFX + nameof(CreateAsset));

        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + nameof(AreaSceneInformation),
            priority = PKG.Menu.Assets.Priority
        )]
        public static void CreateAsset()
        {
            using (_PRF_CreateAsset.Auto())
            {
                CreateNew<AreaSceneInformation>();
            }
        }
#endif
    }
}
