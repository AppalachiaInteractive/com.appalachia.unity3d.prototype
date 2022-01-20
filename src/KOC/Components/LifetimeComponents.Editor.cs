#if UNITY_EDITOR
using System;
using Appalachia.Editing.Debugging.Testing;
using Appalachia.Prototype.KOC.Extensions;
using Appalachia.Prototype.KOC.Input;
using Appalachia.Spatial.MeshBurial.Processing;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Components
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

        private void InitializeEventsAndInputEditor(InputActionAsset asset, KOCInputActions actions)
        {
            using (_PRF_InitializeEventsAndInputEditor.Auto())
            {
                if (AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    return;
                }

                _inputSystemUIInputModule.UnassignActions();
                _inputSystemUIInputModule.actionsAsset = asset;
                _inputSystemUIInputModule.point =
                    asset.FindAction(actions.GenericMenu.Point.id).GetReference();
                _inputSystemUIInputModule.leftClick =
                    asset.FindAction(actions.GenericMenu.Click.id).GetReference();
                _inputSystemUIInputModule.move =
                    asset.FindAction(actions.GenericMenu.Navigate.id).GetReference();
                _inputSystemUIInputModule.submit =
                    asset.FindAction(actions.GenericMenu.Submit.id).GetReference();
                _inputSystemUIInputModule.cancel =
                    asset.FindAction(actions.GenericMenu.Cancel.id).GetReference();
            }
        }

        #region Nested type: EditorOnly

        [Serializable]
        [HideLabel, InlineProperty]
        [Title("Editor Only")]
        public struct EditorOnly
        {
            #region Fields and Autoproperties

            [SerializeField] private Bazooka _bazooka;

            [SerializeField] private MeshBurialExecutionManager _meshBurialExecutionManager;

            #endregion

            public Bazooka Bazooka => _bazooka;

            public MeshBurialExecutionManager MeshBurialExecutionManager => _meshBurialExecutionManager;

            public void Initialize(GameObject gameObject)
            {
                using (_PRF_Initialize.Auto())
                {
                    gameObject.GetOrAddLifetimeComponentInChild(
                        ref _meshBurialExecutionManager,
                        nameof(MeshBurialExecutionManager)
                    );
                    gameObject.GetOrAddLifetimeComponentInChild(ref _bazooka, nameof(Bazooka));
                }
            }

            #region Profiling

            private const string _PRF_PFX = nameof(EditorOnly) + ".";

            private static readonly ProfilerMarker _PRF_Initialize =
                new ProfilerMarker(_PRF_PFX + nameof(Initialize));

            #endregion
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeEditorOnly =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeEditorOnly));

        private static readonly ProfilerMarker _PRF_InitializeEventsAndInputEditor =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeEventsAndInputEditor));

        #endregion
    }
}

#endif