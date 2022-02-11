using System;
using Appalachia.Prototype.KOC.Areas;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

// ReSharper disable NotAccessedField.Local
#pragma warning disable 67
#pragma warning disable CS0414

namespace Appalachia.Prototype.KOC.Scenes
{
    [Serializable]
    public class BootloadedScene
    {
        public delegate void BootloadSceneEventHandler(BootloadedScene scene, ApplicationArea area);

        public event BootloadSceneEventHandler OnActivateComplete;
        public event BootloadSceneEventHandler OnActivateStarted;
        public event BootloadSceneEventHandler OnLoadComplete;
        public event BootloadSceneEventHandler OnLoadFailed;
        public event BootloadSceneEventHandler OnLoadStarted;
        public event BootloadSceneEventHandler OnRemoveComplete;
        public event BootloadSceneEventHandler OnRemoveFailed;
        public event BootloadSceneEventHandler OnRemoveStarted;
        public event BootloadSceneEventHandler OnUnloadComplete;
        public event BootloadSceneEventHandler OnUnloadFailed;
        public event BootloadSceneEventHandler OnUnloadStarted;

        public BootloadedScene(SceneReference sceneReference, ApplicationArea area)
        {
            _sceneReference = sceneReference;
            _area = area;
        }

        #region Fields and Autoproperties

        private ApplicationArea _area;

        private AsyncOperation _activateOperation;
        private AsyncOperation _unloadOperation;
        private AsyncOperationHandle<SceneInstance> _loadOperation;
        private AsyncOperationStatus _status;
        private SceneInstance _scene;
        private SceneReference _sceneReference;

        private SceneState _sceneState;

        #endregion

        public AsyncOperation ActivateOperation => _activateOperation;

        public AsyncOperation UnloadOperation => _unloadOperation;

        public AsyncOperationHandle<SceneInstance> LoadOperation => _loadOperation;

        public SceneInstance Scene => _scene;

        public void Activate()
        {
            using (_PRF_Activate.Auto())
            {
                AppaLog.Context.Bootload.Info(
                    ZString.Format("{0}: {1}", nameof(Activate), _area),
                    _sceneReference
                );

                AppaLog.Context.Bootload.Info(
                    ZString.Format("{0}: {1}", nameof(OnActivateStarted), _area),
                    _sceneReference
                );
                OnActivateStarted?.Invoke(this, _area);

                _activateOperation = _scene.ActivateAsync();

                _activateOperation.completed += _ =>
                {
                    _status = AsyncOperationStatus.Succeeded;
                    _sceneState = SceneState.Activated;

                    AppaLog.Context.Bootload.Info(
                        ZString.Format("{0}: {1}", nameof(OnActivateComplete), _area),
                        _sceneReference
                    );

                    OnActivateComplete?.Invoke(this, _area);
                };
            }
        }

        public void Load()
        {
            using (_PRF_Load.Auto())
            {
                AppaLog.Context.Bootload.Info(
                    ZString.Format("{0}: {1}", nameof(Load), _area),
                    _sceneReference
                );

                AppaLog.Context.Bootload.Info(
                    ZString.Format("{0}: {1}", nameof(OnLoadStarted), _area),
                    _sceneReference
                );
                OnLoadStarted?.Invoke(this, _area);

                _loadOperation = _sceneReference.reference.LoadSceneAsync(LoadSceneMode.Additive, false);

                _loadOperation.Completed += handle =>
                {
                    _status = handle.Status;

                    if (_status == AsyncOperationStatus.Succeeded)
                    {
                        _scene = handle.Result;
                        _sceneState = SceneState.Loaded;

                        AppaLog.Context.Bootload.Info(
                            ZString.Format("{0}: {1}", nameof(OnLoadComplete), _area),
                            _sceneReference
                        );
                        OnLoadComplete?.Invoke(this, _area);
                    }
                    else
                    {
                        AppaLog.Context.Bootload.Info(
                            ZString.Format("{0}: {1}", nameof(OnLoadFailed), _area),
                            _sceneReference
                        );
                        OnLoadFailed?.Invoke(this, _area);
                    }
                };
            }
        }

        public void Unload()
        {
            using (_PRF_Unload.Auto())
            {
                AppaLog.Context.Bootload.Info(
                    ZString.Format("{0}: {1}", nameof(Unload), _area),
                    _sceneReference
                );

                AppaLog.Context.Bootload.Info(
                    ZString.Format("{0}: {1}", nameof(OnUnloadStarted), _area),
                    _sceneReference
                );
                OnUnloadStarted?.Invoke(this, _area);

                _scene.Scene.DestroySafely();

                _unloadOperation = SceneManager.UnloadSceneAsync(
                    _scene.Scene,
                    UnloadSceneOptions.UnloadAllEmbeddedSceneObjects
                );

                _loadOperation = default;
                _scene = default;

                _unloadOperation.completed += _ =>
                {
                    _status = AsyncOperationStatus.Succeeded;

                    Resources.UnloadUnusedAssets();

                    if (_status == AsyncOperationStatus.Succeeded)
                    {
                        _sceneState = SceneState.Unloaded;

                        AppaLog.Context.Bootload.Info(
                            ZString.Format("{0}: {1}", nameof(OnUnloadComplete), _area),
                            _sceneReference
                        );
                        OnUnloadComplete?.Invoke(this, _area);
                    }
                    /*else
                    {
                        AppaLog.Context.Bootload.Info($"{nameof(OnUnloadFailed)}: {_area}", _sceneReference);
                        OnUnloadFailed?.Invoke(this, _area);
                    }*/
                };
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(BootloadedScene) + ".";

        private static readonly ProfilerMarker _PRF_Unload = new ProfilerMarker(_PRF_PFX + nameof(Unload));

        private static readonly ProfilerMarker
            _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(Activate));

        private static readonly ProfilerMarker _PRF_Load = new ProfilerMarker(_PRF_PFX + nameof(Load));

        #endregion

        /*
        private bool IsInInvalidState(
            SceneState state,
            bool invert = false,
            [CallerMemberName] string memberName = null)
        {
            var message = (invert ? "not" : "already") + " " + state.ToString().ToLowerInvariant();

            if ((_sceneState == state) || (invert && (_sceneState != state)))
            {
                Context.Log.Warn(
                    $"[{memberName}] was called when the scene [{_sceneReference.name}]] was {message}."
                );

                return true;
            }

            return false;
        }*/

        /*private bool IsNotInState(SceneState state, [CallerMemberName] string memberName = null)
        {
            return IsInInvalidState(state, true, memberName);
        }*/
    }
#pragma warning restore 67
#pragma warning restore CS0414
}
