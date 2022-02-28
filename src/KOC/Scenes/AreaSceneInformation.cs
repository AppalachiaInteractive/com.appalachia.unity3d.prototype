using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Core;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Attributes;
using Appalachia.Core.Collections.Extensions;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.State;
using Appalachia.Prototype.KOC.Areas;
using Appalachia.Prototype.KOC.Collections;
using Appalachia.Prototype.KOC.Scenes.Collections;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Scenes
{
    [CallStaticConstructorInEditor]
    public class AreaSceneInformation : AppalachiaObject<AreaSceneInformation>
    {
        private delegate void BootloadTransitionHandler(BootloadTransitionArgs args);

        #region Constants and Static Readonly

        private const ApplicationArea NONE_AREA = ApplicationArea.None;
        private const string ASI_PREFIX = nameof(AreaSceneInformation) + "_";

        private const string NONE_AREA_NAME = "None";

        private const string SEARCH_FORMAT_STRING = "{0}_v";

        #endregion

        static AreaSceneInformation()
        {
            When.Object<MainAreaSceneInformationCollection>()
                .IsAvailableThen(i => _mainAreaSceneInformationCollection = i);
        }

        #region Static Fields and Autoproperties

        private static MainAreaSceneInformationCollection _mainAreaSceneInformationCollection;

        #endregion

        #region Fields and Autoproperties

        [FormerlySerializedAs("area")]
        [SerializeField]
        private ApplicationArea _area;

        [PropertyOrder(150)]
        [NonSerialized, ShowInInspector, ReadOnly]
        public BootloadedSceneList scenes;

        [PropertyOrder(140)]
        [NonSerialized, ShowInInspector, ReadOnly]
        public BootloadedScene entryScene;

        [FormerlySerializedAs("entryScene")]
        [PropertyOrder(80)]
        public SceneReference entrySceneReference;

        [FormerlySerializedAs("_scenes")]
        [PropertyOrder(90)]
        [SerializeField]
        private SceneReferenceList _sceneReferences;

        #endregion

        public ApplicationArea Area => _area;

        public void CheckAreaLoadState(ApplicationAreaState state)
        {
            using (_PRF_CheckAreaLoadState.Auto())
            {
                if ((_area == ApplicationArea.None) || (state == null) || state.IsAtRest)
                {
                    return;
                }

                /*AppaLog.Context.Bootload.Trace(
                    ZString.Format("{0}: {1}", nameof(CheckAreaLoadState), state.Area),
                    this
                );*/

                EvaluateAreaState(state, HandleBootloadTransition);
            }
        }

        public IEnumerable<SceneReference> GetScenesToLoad()
        {
            AppaLog.Context.Bootload.Info(nameof(GetScenesToLoad), this);

            foreach (var scene in _sceneReferences)
            {
                yield return scene;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);
#if UNITY_EDITOR
            if (AppalachiaApplication.IsPlayingOrWillPlay)
            {
                return;
            }

            if (scenes == null)
            {
                scenes = new BootloadedSceneList();
            }

            if (_sceneReferences == null)
            {
                _sceneReferences = new SceneReferenceList();

                MarkAsModified();
            }

            var areaName = name.Replace(ASI_PREFIX, string.Empty);

            if ((areaName == NONE_AREA_NAME) || areaName.IsNullOrWhiteSpace())
            {
                return;
            }

            if ((_area == ApplicationArea.None) && (areaName != NONE_AREA_NAME))
            {
                try
                {
                    _area = Enum.Parse<ApplicationArea>(areaName, false);
                    MarkAsModified();
                }
                catch (Exception e)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "The name [{0}] of {1} is not in {2} Enum.",
                            areaName,
                            nameof(AreaSceneInformation),
                            nameof(ApplicationArea)
                        ),
                        this,
                        e
                    );
                }
            }

            if (entrySceneReference == null)
            {
                entrySceneReference =
                    AppalachiaObjectFactory.LoadExistingOrCreateNewAsset<SceneReference>(
                        ZString.Format("{0}_{1}", nameof(SceneReference), areaName)
                    );

                MarkAsModified();
                if (entrySceneReference == null)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "The name [{0}] of {1} does not have a matching {2}.",
                            areaName,
                            nameof(AreaSceneInformation),
                            nameof(SceneReference)
                        ),
                        this
                    );
                }
            }

            if (entrySceneReference != null)
            {
                if (entrySceneReference.elements == null)
                {
                    entrySceneReference.elements = new SceneReferenceElementLookup();
                    entrySceneReference.MarkAsModified();
                }

                entrySceneReference.elements.Changed.Event += entrySceneReference.OnChanged;

                var searchString = ZString.Format(SEARCH_FORMAT_STRING, areaName);
                var sceneAssets = AssetDatabaseManager.FindAssets<UnityEditor.SceneAsset>(searchString)
                                                      .Where(sa => !sa.name.Contains("AutoSave"))
                                                      .Where(
                                                           sa => sa.name.Length == (searchString.Length + 2)
                                                       )
                                                      .ToList();

                if (entrySceneReference.elements.Count != sceneAssets.Count)
                {
                    entrySceneReference.elements.Clear();

                    var values = sceneAssets.Select(sa => sa.ToSceneReference());
                    entrySceneReference.elements.AddOrUpdateRange(values, v => v.version);
                }

                foreach (var sceneReferenceElement in entrySceneReference.elements.Values)
                {
                    if (sceneReferenceElement.sceneAsset == null)
                    {
                        if (sceneReferenceElement.sceneAsset == null)
                        {
                            Context.Log.Error(
                                ZString.Format(
                                    "The name [{0}] of {1} does not have a matching {2}.",
                                    areaName,
                                    nameof(AreaSceneInformation),
                                    nameof(UnityEditor.SceneAsset)
                                ),
                                this
                            );
                        }
                    }

                    if ((sceneReferenceElement.sceneAsset != null) &&
                        (sceneReferenceElement.reference?.editorAsset == null))
                    {
                        sceneReferenceElement.SetSelection(sceneReferenceElement.sceneAsset);
                    }
                }
            }

#endif
        }

        private void EvaluateAreaState(ApplicationAreaState state, BootloadTransitionHandler handler)
        {
            using (_PRF_CheckAreaLoadStateInternal.Auto())
            {
                /*AppaLog.Context.Bootload.Trace(
                    ZString.Format("{0}: {1}", nameof(EvaluateAreaState), state.Area),
                    this
                );*/

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
                    entryScene = new BootloadedScene(entrySceneReference, _area);
                }

                result.stateTransition = state.MarkStateTransitionCompleted;

                switch (state.NextState)
                {
                    case ApplicationAreaStates.Load:
                        result.action = entryScene.Load;

                        entryScene.OnLoadComplete += (s, a) => handler(completedResult);
                        entryScene.OnLoadFailed += (s, a) => handler(failedResult);

                        break;
                    case ApplicationAreaStates.Activate:
                        result.action = entryScene.Activate;

                        entryScene.OnActivateComplete += (s, a) => handler(completedResult);

                        //entryScene.OnActivateFailed += _ => handler(failedResult);

                        break;
                    case ApplicationAreaStates.Unload:
                        result.action = entryScene.Unload;

                        entryScene.OnUnloadComplete += (s, a) => handler(completedResult);
                        entryScene.OnUnloadFailed += (s, a) => handler(failedResult);

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
                var state = args.state;

                AppaLog.Context.Bootload.Info(
                    ZString.Format(
                        "Area [{0}] is transitioning from [{1}:{2}].",
                        state.Area.ToString().FormatNameForLogging(),
                        state.State,
                        state.Substate
                    ),
                    this
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
                    Context.Log.Error(
                        ZString.Format(
                            "Error executing state change on area [{0}] from [{1}:{2}].",
                            _area.ToString().FormatNameForLogging(),
                            args.state.State,
                            args.state.Substate
                        ),
                        ex
                    );

                    throw;
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

        #region Profiling

        private static readonly ProfilerMarker _PRF_CheckAreaLoadState =
            new ProfilerMarker(_PRF_PFX + nameof(CheckAreaLoadState));

        private static readonly ProfilerMarker _PRF_CheckAreaLoadStateInternal =
            new ProfilerMarker(_PRF_PFX + nameof(EvaluateAreaState));

        private static readonly ProfilerMarker _PRF_HandleBootloadTransition =
            new ProfilerMarker(_PRF_PFX + nameof(HandleBootloadTransition));

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
                AppaLog.Context.Bootload.Info(nameof(CreateScene), this);

                var otherScene = GetScenesToLoad().FirstOrDefault();

                if (otherScene == null)
                {
                    otherScene = _mainAreaSceneInformationCollection.Lookup.Items
                                                                    .SelectMany((k, v) => v._sceneReferences)
                                                                    .FirstOrDefault(s => s != null);
                }

                var otherPath = otherScene.AssetPath;
                var otherDirectory = AppaPath.GetDirectoryName(otherPath);

                var sceneName = ZString.Format("{0}_{1}", name, _sceneReferences.Count);
                var outputPath = AppaPath.Combine(otherDirectory, ZString.Format("{0}.unity", sceneName));

                var scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(
                    UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,
                    UnityEditor.SceneManagement.NewSceneMode.Additive
                );

                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene, outputPath);
                AssetDatabaseManager.Refresh();

                UnityEditor.SceneManagement.EditorSceneManager.CloseScene(scene, true);

                var asset = AssetDatabaseManager.LoadAssetAtPath<UnityEditor.SceneAsset>(outputPath);

                var reference = CreateNew<SceneReference>(
                    ZString.Format("{0}_{1}", nameof(SceneReference), sceneName)
                );

                reference.SetSelection(AreaVersion.V01, asset);

                if (_sceneReferences == null)
                {
                    _sceneReferences = new SceneReferenceList();
                }

                _sceneReferences.Add(reference);

                MarkAsModified();
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
