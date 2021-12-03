using System;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

#pragma warning disable 67

namespace Appalachia.Prototype.KOC.Application.Scenes
{
    [Serializable]
    public class BootloadedScene
    {
        public delegate void BootloadSceneEventHandler(BootloadedScene scene);

        public BootloadedScene(SceneReference sceneReference)
        {
            _sceneReference = sceneReference;
        }

        #region Fields and Autoproperties

        private AsyncOperation _activateOperation;
        private AsyncOperationHandle<SceneInstance> _loadOperation;
        private AsyncOperationHandle<SceneInstance> _unloadOperation;
        private AsyncOperationStatus _status;
        private SceneInstance _scene;
        private SceneReference _sceneReference;

        private SceneState _sceneState;

        #endregion

        public AsyncOperation ActivateOperation => _activateOperation;

        public AsyncOperationHandle<SceneInstance> LoadOperation => _loadOperation;

        public AsyncOperationHandle<SceneInstance> UnloadOperation => _unloadOperation;

        public SceneInstance Scene => _scene;
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

        public void Activate()
        {
            using (_PRF_Activate.Auto())
            {
                OnActivateStarted?.Invoke(this);

                _activateOperation = _scene.ActivateAsync();

                _activateOperation.completed += _ =>
                {
                    _status = AsyncOperationStatus.Succeeded;
                    _sceneState = SceneState.Activated;

                    OnActivateComplete?.Invoke(this);
                };
            }
        }

        public void Load()
        {
            using (_PRF_Load.Auto())
            {
                OnLoadStarted?.Invoke(this);

                _loadOperation = _sceneReference.reference.LoadSceneAsync(LoadSceneMode.Additive, false);

                _loadOperation.Completed += handle =>
                {
                    _status = handle.Status;

                    if (_status == AsyncOperationStatus.Succeeded)
                    {
                        _scene = handle.Result;
                        _sceneState = SceneState.Loaded;

                        OnLoadComplete?.Invoke(this);
                    }
                    else
                    {
                        OnLoadFailed?.Invoke(this);
                    }
                };
            }
        }

        public void Unload()
        {
            using (_PRF_Unload.Auto())
            {
                OnUnloadStarted?.Invoke(this);

                _unloadOperation = _sceneReference.reference.UnLoadScene();
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
                AppaLog.Warn(
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
}
