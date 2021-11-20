using System;
using Sirenix.OdinInspector;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Appalachia.Prototype.KOC.Application.Scenes
{
    [Serializable]
    public class SceneBootloadProgress
    {
        #region Fields and Autoproperties

        public AsyncOperationHandle<SceneInstance> operation;
        public SceneInstance scene;

        #endregion

        [ShowInInspector] public float progress => operation.PercentComplete;
    }
}
