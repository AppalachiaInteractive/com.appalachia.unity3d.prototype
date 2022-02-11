#if UNITY_EDITOR
using Appalachia.Prototype.KOC.Extensions;
using Appalachia.Prototype.KOC.Input;
using Appalachia.Utility.Execution;
using Unity.Profiling;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Lifetime
{
    public partial class LifetimeComponentManager
    {
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

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeEventsAndInputEditor =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeEventsAndInputEditor));

        #endregion
    }
}

#endif
