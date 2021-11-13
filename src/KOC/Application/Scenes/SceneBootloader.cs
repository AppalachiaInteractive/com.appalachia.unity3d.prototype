using System;
using System.Collections;
using Appalachia.Prototype.KOC.Application.State;
using Appalachia.Utility.Logging;
using Unity.Profiling;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Appalachia.Prototype.KOC.Application.Scenes
{
    public static class SceneBootloader
    {
        #region Profiling

        private const string _PRF_PFX = nameof(SceneBootloader) + ".";

        private static readonly ProfilerMarker _PRF_CheckAreaLoadState =
            new ProfilerMarker(_PRF_PFX + nameof(CheckAreaLoadState));

        private static readonly ProfilerMarker _PRF_BootloadEntryScene =
            new ProfilerMarker(_PRF_PFX + nameof(BootloadEntryScene));

        #endregion

        public static void CheckAreaLoadState(SceneBootloadData bootloadData, ApplicationSubstate state)
        {
            using (_PRF_CheckAreaLoadState.Auto())
            {
                AppaLog.Context.Bootload.Trace(nameof(CheckAreaLoadState));

                if (state.substate == ApplicationStates.NotLoaded)
                {
                    state.substate = ApplicationStates.Loading;

                    BootloadEntryScene(bootloadData);
                }
                else if (state.substate == ApplicationStates.Loading)
                {
                    var operation = bootloadData.entrySceneBootloadProgress.operation;

                    if (operation.IsDone)
                    {
                        state.substate = operation.Status == AsyncOperationStatus.Succeeded
                            ? ApplicationStates.LoadComplete
                            : ApplicationStates.LoadFailed;
                    }
                }
            }
        }

        private static IEnumerator BootloadAllScenes(SceneBootloadData bootloadData)
        {
            AppaLog.Context.Bootload.Info(nameof(BootloadAllScenes));

            if (bootloadData.entryScene == null)
            {
                throw new NotSupportedException();
            }

            var sceneReferences = bootloadData.GetScenesToLoad();

            foreach (var sceneReference in sceneReferences)
            {
                var assetReference = sceneReference.sceneReference;

                var bootload = new SceneBootloadProgress
                {
                    operation = assetReference.LoadSceneAsync(LoadSceneMode.Additive, false)
                };

                bootload.operation.Completed += handle => bootload.scene = handle.Result;

                bootloadData.bootloadProgress.Add(bootload);
                yield return null;
            }
        }

        private static void BootloadEntryScene(SceneBootloadData bootloadData)
        {
            using (_PRF_BootloadEntryScene.Auto())
            {
                AppaLog.Context.Bootload.Info(nameof(BootloadEntryScene));

                if (bootloadData.entryScene == null)
                {
                    throw new NotSupportedException();
                }

                var assetReference = bootloadData.entryScene.sceneReference;

                var bootloadProgress = new SceneBootloadProgress
                {
                    operation = assetReference.LoadSceneAsync(LoadSceneMode.Additive, false)
                };

                bootloadProgress.operation.Completed += handle => bootloadProgress.scene = handle.Result;

                bootloadData.entrySceneBootloadProgress = bootloadProgress;
            }
        }
    }
}
