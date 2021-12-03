using System.Collections;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes;
using Appalachia.Core.Execution.Hooks;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Prototype.KOC.Application.Behaviours;
using Appalachia.Prototype.KOC.Application.Components;
using Appalachia.Prototype.KOC.Application.Scenes;
using Appalachia.Prototype.KOC.Application.Screens.Fading;
using Appalachia.Prototype.KOC.Application.State;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Application
{
    [InspectorIcon(Icons.Squirrel.Red)]
    [AlwaysInitializeOnLoad]
    [ExecutionOrder(ExecutionOrders.ApplicationManager)]
    public class ApplicationManager : SingletonAppalachiaApplicationBehaviour<ApplicationManager>
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("applicationAreaStateCollection")]
        [FormerlySerializedAs("applicationAreaStates")]
        [FormerlySerializedAs("applicationState")]
        [FormerlySerializedAs("state")]
        [Title("Application State"), InlineProperty, HideLabel]
        public ApplicationAreaStateCollection areaStates;

        private ApplicationLifetimeComponents _lifetimeComponents;

        [FormerlySerializedAs("_lifetimeComponentsAsset")]
        [SerializeField]
        private LifetimeMetadata lifetimeMetadata;

        [FormerlySerializedAs("_bootloads")]
        [Title("Area Scene Information")]
        [SerializeField]
        private AreaSceneInformationCollection _areaSceneInfos;

        private bool _isApplicationFocused;
        private bool _isSceneLoading;
        private FrameEnd _frameEnd;
        private FrameStart _frameStart;
        private Canvas _canvas;
        private ScreenFadeManager _screenFader;

        private bool _hasStarted;

        #endregion

        public bool IsApplicationFocused => _isApplicationFocused;

        public bool IsSceneLoading
        {
            get => _isSceneLoading;
            set => _isSceneLoading = value;
        }

        #region Event Functions

        protected override void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                base.Awake();

                AppaLog.Context.Application.Info(nameof(Awake));
                instance.Initialize();

                AreaRegistry.RegisterApplicationManager(this);
            }
        }

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (!UnityEngine.Application.isPlaying)
                {
                    return;
                }

                AppaLog.Context.Bootload.Trace(nameof(Update));

                foreach (var areaStateEntry in areaStates.Areas)
                {
                    var area = areaStateEntry.Key;
                    var areaState = areaStateEntry.Value;

                    var areaSceneInfo = _areaSceneInfos.GetByArea(area);

                    areaSceneInfo.CheckAreaLoadState(areaState);

                    if ((areaState.State == ApplicationAreaStates.Load) &&
                        (areaState.Substate == ApplicationAreaSubstates.Complete))
                    {
                        var areaMetadata = AreaRegistry.GetMetadata(area);

                        if (areaMetadata.LoadBehaviour == LoadBehaviour.ActivateImmediately)
                        {
                            areaState.QueueActivate();
                        }

                        if (areaMetadata.SetEntrySceneActive)
                        {
                            areaSceneInfo.entryScene.OnActivateComplete += scene =>
                                SceneManager.SetActiveScene(scene.Scene.Scene);
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

        public void Activate(ApplicationArea area)
        {
            var areaState = areaStates.Areas[area];

            areaState.QueueLoad();
        }

        public void Deactivate(ApplicationArea area)
        {
            var areaState = areaStates.Areas[area];

            areaState.QueueUnload();
        }

        public void ExitApplication()
        {
            AppaLog.Context.Application.Info(nameof(ExitApplication));

            UnityEngine.Application.Quit();
        }

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                AppaLog.Context.Application.Info(nameof(Initialize));
                name = nameof(ApplicationManager);

                if (areaStates == null)
                {
                    areaStates = ApplicationAreaStateCollection.CreateNew();
                }

                _lifetimeComponents = ApplicationLifetimeComponents.instance;
                _lifetimeComponents.gameObject.SetAsSiblingTo(transform);

                lifetimeMetadata = _lifetimeComponents.metadata;

                gameObject.CreateOrGetComponent(ref _frameStart);
                gameObject.CreateOrGetComponent(ref _frameEnd);

                gameObject.CreateOrGetComponentInChild(ref _canvas, "Canvas - FullScreenBlack", false);

                _canvas.gameObject.CreateOrGetComponent(ref _screenFader);

                if (_areaSceneInfos == null)
                {
                    _areaSceneInfos = AreaSceneInformationCollection.instance;
                }

                areaStates.Initialize();

                if (!_hasStarted && (areaStates.LoadCount == 0) && (areaStates.ActivateCount == 0))
                {
                    Activate(ApplicationArea.SplashScreen);
                    _hasStarted = true;
                }

                DontDestroyOnLoadSafe(gameObject);
            }
        }

        private IEnumerator LoadScene(
            SceneReference sceneToLoad,
            Scene sceneToUnload = default(Scene),
            bool fadeOut = true,
            bool fadeIn = true)
        {
            AppaLog.Context.Application.Info(nameof(LoadScene));

            _isSceneLoading = true;

            if (fadeOut)
            {
                _screenFader.FadeOut();

                while (_screenFader.IsFading)
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            if (sceneToUnload != default)
            {
                var unload = SceneManager.UnloadSceneAsync(sceneToUnload);

                while (!unload.isDone)
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            var load = sceneToLoad.reference.LoadSceneAsync(LoadSceneMode.Additive);

            while (!load.IsDone)
            {
                yield return new WaitForEndOfFrame();
            }

            if (fadeIn)
            {
                _screenFader.FadeIn();

                while (_screenFader.IsFading)
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            _isSceneLoading = false;
            yield return null;
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ApplicationManager) + ".";

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        private static readonly ProfilerMarker _PRF_OnApplicationFocus =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplicationFocus));

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
