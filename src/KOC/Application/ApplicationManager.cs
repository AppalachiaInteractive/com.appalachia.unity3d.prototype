using System.Collections;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes;
using Appalachia.Core.Behaviours;
using Appalachia.Core.Execution.Hooks;
using Appalachia.Prototype.KOC.Application.Areas;
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

namespace Appalachia.Prototype.KOC.Application
{
    [InspectorIcon(Icons.Squirrel.Red)]
    [AlwaysInitializeOnLoad]
    [ExecutionOrder(-10000)]
    public class ApplicationManager : SingletonAppalachiaBehaviour<ApplicationManager>
    {
        #region Fields and Autoproperties

        [Title("Application State"), InlineProperty, HideLabel]
        public ApplicationState state;

        private ApplicationLifetimeComponents _lifetimeComponents;

        [Title("Bootload Data")]
        [SerializeField]
        private AreaSceneBootloadDataCollection _bootloads;

        private bool _isApplicationFocused;
        private bool _isSceneLoading;
        private FrameEnd _frameEnd;
        private FrameStart _frameStart;

        private ScreenFadeManager _screenFader;

        #endregion

        public bool IsApplicationFocused => _isApplicationFocused;

        public bool IsNextStateReady
        {
            get
            {
                using (_PRF_IsNextStateReady.Auto())
                {
                    if (state.next == null)
                    {
                        return false;
                    }

                    if (state.next.substate != ApplicationStates.LoadComplete)
                    {
                        return false;
                    }

                    return true;
                }
            }
        }

        public bool IsSceneLoading
        {
            get => _isSceneLoading;
            set => _isSceneLoading = value;
        }

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                AppaLog.Context.Bootload.Trace(nameof(Update));

                Initialize();

                var currentBootloadData = _bootloads.GetByArea(state.currentArea);

                SceneBootloader.CheckAreaLoadState(currentBootloadData, state.current);

                var nextBootloadData = _bootloads.GetByArea(state.nextArea);

                SceneBootloader.CheckAreaLoadState(nextBootloadData, state.next);
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

        public void ExitApplication()
        {
            AppaLog.Context.Application.Info(nameof(ExitApplication));

            UnityEngine.Application.Quit();
        }

        public void TransitionTo(ApplicationArea area)
        {
            using (_PRF_TransitionTo.Auto())
            {
                AppaLog.Context.Bootload.Info(nameof(TransitionTo));
                state.currentArea = area;
            }
        }

        protected override void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                base.Awake();
                
                AppaLog.Context.Bootload.Info(nameof(Awake));
                instance.Initialize();
            }
        }

        public override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();
                
                AppaLog.Context.Bootload.Info(nameof(Initialize));
                name = nameof(ApplicationManager);

                if (state == null)
                {
                    state = new ApplicationState();
                }

                _lifetimeComponents = ApplicationLifetimeComponents.instance;
                _lifetimeComponents.SetAsSiblingTo(transform);
                gameObject.GetOrCreateComponent(ref _frameStart);
                gameObject.GetOrCreateComponent(ref _frameEnd);
                gameObject.GetOrCreateComponent(ref _screenFader);

                if (_bootloads == null)
                {
                    _bootloads = AreaSceneBootloadDataCollection.instance;
                }

                if (state.currentArea == ApplicationArea.None)
                {
                    state.currentArea = ApplicationArea.SplashScreen;
                }

                if ((state.nextArea == ApplicationArea.None) &&
                    (state.currentArea == ApplicationArea.SplashScreen))
                {
                    state.nextArea = ApplicationArea.MainMenu;
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
            AppaLog.Context.Bootload.Info(nameof(LoadScene));

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

            var load = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);

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

        private static readonly ProfilerMarker _PRF_IsNextStateReady =
            new ProfilerMarker(_PRF_PFX + nameof(IsNextStateReady));

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        private static readonly ProfilerMarker _PRF_OnApplicationFocus =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplicationFocus));

        private static readonly ProfilerMarker _PRF_TransitionTo =
            new ProfilerMarker(_PRF_PFX + nameof(TransitionTo));

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
