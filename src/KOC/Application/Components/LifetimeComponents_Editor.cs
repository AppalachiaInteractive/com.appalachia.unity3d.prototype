using System;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
#if UNITY_EDITOR
using Appalachia.Editing.Debugging.Testing;
using Appalachia.Spatial.MeshBurial.Processing;
#endif

namespace Appalachia.Prototype.KOC.Application.Components
{
    public sealed partial class LifetimeComponents
    {
        #region Fields and Autoproperties

        [FoldoutGroup("Editor Only")]
        [SerializeField]
        private EditorOnly _editorOnly;

        #endregion

        private void InitializeEditorOnly(GameObject gameObject)
        {
            using (_PRF_InitializeEditorOnly.Auto())
            {
                _editorOnly.Initialize(gameObject);
            }
        }

        #region Nested type: EditorOnly

        [Serializable]
        [HideLabel, InlineProperty]
        [Title("Editor Only")]
        public struct EditorOnly
        {
            public void Initialize(GameObject gameObject)
            {
#if UNITY_EDITOR
                using (_PRF_Initialize.Auto())
                {
                    gameObject.GetOrCreateLifetimeComponentInChild(
                        ref _meshBurialExecutionManager,
                        nameof(MeshBurialExecutionManager)
                    );
                    gameObject.GetOrCreateLifetimeComponentInChild(ref _bazooka, nameof(Bazooka));
                }
#endif
            }

            #region Profiling

            private const string _PRF_PFX = nameof(EditorOnly) + ".";

            private static readonly ProfilerMarker _PRF_Initialize =
                new ProfilerMarker(_PRF_PFX + nameof(Initialize));

            #endregion

#if UNITY_EDITOR

            #region Fields and Autoproperties

            [SerializeField] private MeshBurialExecutionManager _meshBurialExecutionManager;
            [SerializeField] private Bazooka _bazooka;

            #endregion

            public MeshBurialExecutionManager MeshBurialExecutionManager => _meshBurialExecutionManager;
            public Bazooka Bazooka => _bazooka;

#endif
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeEditorOnly =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeEditorOnly));

        #endregion
    }
}
