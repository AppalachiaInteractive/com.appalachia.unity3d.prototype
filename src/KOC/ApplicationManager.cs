using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Lifetime;
using Appalachia.Prototype.KOC.Application.State;
using Appalachia.Prototype.KOC.Areas;
using Appalachia.Prototype.KOC.Behaviours;
using Appalachia.Prototype.KOC.Scenes;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Appalachia.Prototype.KOC
{
    [CallStaticConstructorInEditor]
    [ExecutionOrder(ExecutionOrders.ApplicationManager)]
    public class ApplicationManager : GlobalSingletonAppalachiaBehaviour<ApplicationManager>
    {
        static ApplicationManager()
        {
            Core.Objects.Root.AppalachiaRepository.PrimaryOwnerType = typeof(ApplicationManager);

            RegisterDependency<MainAreaSceneInformationCollection>(
                i => _mainAreaSceneInformationCollection = i
            );

            RegisterDependency<LifetimeComponentManager>(i => _lifetimeComponentManager = i);
        }

        #region Static Fields and Autoproperties

        [NonSerialized] private static LifetimeComponentManager _lifetimeComponentManager;

        [Title("Area Scene Information")]
        [ShowInInspector]
        [NonSerialized]
        [InlineEditor(Expanded = true, ObjectFieldMode = InlineEditorObjectFieldModes.Boxed)]
        [HideLabel]
        [LabelWidth(0)]
        private static MainAreaSceneInformationCollection _mainAreaSceneInformationCollection;

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] public ApplicationArea PrimarySubSceneArea;

        [Title("Application State")]
        [ShowInInspector, HideReferenceObjectPicker, HideLabel]
        [NonSerialized]
        private ApplicationAreaStateCollection _areaStates;

        [NonSerialized] private bool _isApplicationFocused;

        [NonSerialized] private bool _hasStarted;

        #endregion

        public bool HasSubSceneManagerBeenIdentified => PrimarySubSceneArea != ApplicationArea.None;

        public bool IsApplicationFocused => _isApplicationFocused;

        #region Event Functions

        /// <inheritdoc />
        protected override void Update()
        {
            using (_PRF_Update.Auto())
            {
                APPASERIALIZE.FrameIncrement();

                base.Update();

                if (ShouldSkipUpdate)
                {
                    return;
                }

                if (!AppalachiaApplication.IsPlaying)
                {
                    return;
                }

                /*Context.Log.Trace(nameof(Update), this);*/

                foreach (var areaStateEntry in _areaStates.Areas)
                {
                    var area = areaStateEntry.Key;
                    var areaState = areaStateEntry.Value;

                    var areaSceneInfo = _mainAreaSceneInformationCollection.Lookup.Items.Get(area);

                    areaSceneInfo.CheckAreaLoadState(areaState);

                    if ((areaState.State == ApplicationAreaStates.Load) &&
                        (areaState.Substate == ApplicationAreaSubstates.Complete))
                    {
                        var areaMetadata = AreaRegistry.GetMetadata(area);

                        if (areaMetadata.SceneBehaviour.setEntrySceneActive)
                        {
                            areaSceneInfo.entryScene.OnActivateComplete -= SceneLoad_SetActive;
                            areaSceneInfo.entryScene.OnActivateComplete += SceneLoad_SetActive;
                        }
                    }
                }
            }
        }

        private void OnApplicationFocus(bool isFocused)
        {
            using (_PRF_OnApplicationFocus.Auto())
            {
                _isApplicationFocused = isFocused;
            }
        }

        #endregion

        public void ActivateManager(ApplicationArea area)
        {
            using (_PRF_ActivateManager.Auto())
            {
                var manager = AreaRegistry.GetManager(area);

                manager.Activate();
            }
        }

        public void ActivateScene(ApplicationArea area)
        {
            using (_PRF_ActivateManager.Auto())
            {
                Context.Log.Info(
                    ZString.Format(
                        "{0} instructed to {1} area [{2}].",
                        nameof(ApplicationManager),
                        nameof(ActivateScene),
                        area.FormatEnumForLogging()
                    ),
                    this
                );

                var areaState = _areaStates.Areas[area];

                areaState.QueueActivate();
            }
        }

        public void DeactivateManager(ApplicationArea area)
        {
            using (_PRF_DeactivateManager.Auto())
            {
                var manager = AreaRegistry.GetManager(area);

                manager.Deactivate();
            }
        }

        public void DestroyScene(ApplicationArea area)
        {
            using (_PRF_DestroyScene.Auto())
            {
                Context.Log.Info(
                    ZString.Format(
                        "{0} instructed to {1} area [{2}].",
                        nameof(ApplicationManager),
                        nameof(DestroyScene),
                        area.FormatEnumForLogging()
                    ),
                    this
                );

                var areaState = _areaStates.Areas[area];

                areaState.QueueUnload();
            }
        }

        public void ExitApplication()
        {
            Context.Log.Warn(nameof(ExitApplication), this);

            AppalachiaApplication.Quit();
        }

        public void GetAreaInfo(
            ApplicationArea area,
            out AreaSceneInformation info,
            out ApplicationAreaState state)
        {
            using (_PRF_GetAreaInfo.Auto())
            {
                info = _mainAreaSceneInformationCollection.Lookup[area];
                state = _areaStates.Areas[area];
            }
        }

        public void LoadScene(ApplicationArea area, bool autoActivate = false)
        {
            using (_PRF_LoadScene.Auto())
            {
                Context.Log.Info(
                    ZString.Format(
                        "{0} instructed to {1}{2} area [{3}].",
                        nameof(ApplicationManager),
                        nameof(LoadScene),
                        autoActivate ? " and Activate" : string.Empty,
                        area.FormatEnumForLogging()
                    ),
                    this
                );

                var areaState = _areaStates.Areas[area];

                if (autoActivate)
                {
                    var areaSceneInfo = _mainAreaSceneInformationCollection.Lookup.Items.Get(area);

                    if (areaSceneInfo.entryScene == null)
                    {
                        areaSceneInfo.entryScene = new BootloadedScene(
                            areaSceneInfo.entrySceneReference,
                            areaSceneInfo.Area
                        );
                    }

                    areaSceneInfo.entryScene.OnLoadComplete -= SceneLoad_OnLoadComplete;
                    areaSceneInfo.entryScene.OnLoadComplete += SceneLoad_OnLoadComplete;

                    areaSceneInfo.entryScene.OnActivateComplete -= SceneLoad_OnActivateComplete;
                    areaSceneInfo.entryScene.OnActivateComplete += SceneLoad_OnActivateComplete;
                }

                areaState.QueueLoad();
            }
        }

        public void OnApplicationAreaStateLookupChanged()
        {
            using (_PRF_OnApplicationAreaStateLookupChanged.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                AppalachiaApplication.InitializeApplication();

                try
                {
                    if (!_hasStarted)
                    {
                        enabled = true;
                    }
                }
                catch (MissingReferenceException)
                {
                    return;
                }

                if (_lifetimeComponentManager == null)
                {
                    Context.Log.Error(
                        $"{nameof(LifetimeComponentManager)} should be initialized before {nameof(ApplicationManager)}!"
                    );

                    return;
                }

                if (!_lifetimeComponentManager.gameObject.activeSelf)
                {
                    _lifetimeComponentManager.gameObject.SetActive(true);
                }

                if (_lifetimeComponentManager.enabled)
                {
                    _lifetimeComponentManager.enabled = true;
                }

                if (_areaStates == null)
                {
                    _areaStates = ApplicationAreaStateCollection.CreateNew(this);
                }

                _areaStates.Areas.Changed.Event +=(OnChanged);
                _areaStates.Initialize(this);

                if (AppalachiaApplication.IsPlaying)
                {
                    if (!_hasStarted)
                    {
                        if (!RunningAsSubScene)
                        {
                            if ((_areaStates.LoadCount == 0) && (_areaStates.ActivateCount == 0))
                            {
                                LoadScene(ApplicationArea.StartEnvironment,   true);
                                LoadScene(ApplicationArea.DeveloperInterface, true);
                                LoadScene(ApplicationArea.SplashScreen,       true);
                            }
                        }
                    }

                    _hasStarted = true;
                }

                gameObject.transform.SetSiblingIndex(0);
                _lifetimeComponentManager.transform.SetSiblingIndex(1);

                DontDestroyOnLoadSafe();

#if UNITY_EDITOR
                if (AppalachiaApplication.IsPlaying)
                {
                    AppalachiaApplication.Editor.ExitingPlayMode.Event +=
                        AppalachiaApplication.Editor.ForceRecompile;
                }
#endif
            }
        }

        private static void SceneLoad_OnActivateComplete(BootloadedScene s, ApplicationArea area)
        {
            using (_PRF_SceneLoad_OnActivateComplete.Auto())
            {
                instance.Context.Log.Info(
                    ZString.Format(
                        "{0}: {1}.",
                        nameof(SceneLoad_OnActivateComplete),
                        area.FormatEnumForLogging()
                    ),
                    instance
                );
                var manager = AreaRegistry.GetManager(area);
                manager.Activate();
            }
        }

        private static void SceneLoad_OnLoadComplete(BootloadedScene s, ApplicationArea area)
        {
            using (_PRF_SceneLoad_OnLoadComplete.Auto())
            {
                instance.Context.Log.Info(
                    ZString.Format(
                        "{0}: {1}.",
                        nameof(SceneLoad_OnLoadComplete),
                        area.FormatEnumForLogging()
                    ),
                    instance
                );
                instance.ActivateManager(area);
            }
        }

        private static void SceneLoad_SetActive(BootloadedScene s, ApplicationArea area)
        {
            using (_PRF_SceneLoad_SetActive.Auto())
            {
                instance.Context.Log.Info(
                    ZString.Format("{0}: {1}.", nameof(SceneLoad_SetActive), area.FormatEnumForLogging()),
                    instance
                );
                SceneManager.SetActiveScene(s.Scene.Scene);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_DeactivateManager =
            new ProfilerMarker(_PRF_PFX + nameof(DeactivateManager));

        private static readonly ProfilerMarker _PRF_GetAreaInfo =
            new ProfilerMarker(_PRF_PFX + nameof(GetAreaInfo));

        private static readonly ProfilerMarker _PRF_OnApplicationAreaStateLookupChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplicationAreaStateLookupChanged));

        private static readonly ProfilerMarker _PRF_SceneLoad_SetActive =
            new ProfilerMarker(_PRF_PFX + nameof(SceneLoad_SetActive));

        private static readonly ProfilerMarker _PRF_SceneLoad_OnLoadComplete =
            new ProfilerMarker(_PRF_PFX + nameof(SceneLoad_OnLoadComplete));

        private static readonly ProfilerMarker _PRF_SceneLoad_OnActivateComplete =
            new ProfilerMarker(_PRF_PFX + nameof(SceneLoad_OnActivateComplete));

        private static readonly ProfilerMarker _PRF_ActivateManager =
            new ProfilerMarker(_PRF_PFX + nameof(ActivateScene));

        private static readonly ProfilerMarker _PRF_DestroyScene =
            new ProfilerMarker(_PRF_PFX + nameof(DestroyScene));

        private static readonly ProfilerMarker _PRF_LoadScene =
            new ProfilerMarker(_PRF_PFX + nameof(LoadScene));

        private static readonly ProfilerMarker _PRF_OnApplicationFocus =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplicationFocus));

        #endregion
    }
}
