using System;
using Appalachia.Core.Overrides.Implementations;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Input
{
    public partial class KOCInputActions
    {
        public enum Maps
        {
            Player,
            Debug,
            GenericMenu,
            MainMenu,
            InGameMenu,
            PauseMenu,
            DeathMenu,
            LoadingScreen,
        }

        [Serializable]
        public struct MapEnableState
        {
            [SerializeField] public OverridableBool Player;
            [SerializeField] public OverridableBool Debug;
            [SerializeField] public OverridableBool GenericMenu;
            [SerializeField] public OverridableBool MainMenu;
            [SerializeField] public OverridableBool InGameMenu;
            [SerializeField] public OverridableBool PauseMenu;
            [SerializeField] public OverridableBool DeathMenu;
            [SerializeField] public OverridableBool LoadingScreen;

            private void Apply(OverridableBool mapState, bool isEnabled, Action enable, Action disable)
            {
                if (mapState.overrideEnabled)
                {
                    if (mapState.value)
                    {
                        if (!isEnabled)
                        {
                            enable();
                        }
                    }
                    else if (isEnabled)
                    {
                        disable();
                    }
                }
            }

            public void Apply(KOCInputActions actions)
            {
                Apply(Player, actions.Player.enabled, actions.Player.Enable, actions.Player.Disable);
                Apply(Debug, actions.Debug.enabled, actions.Debug.Enable, actions.Debug.Disable);
                Apply(GenericMenu, actions.GenericMenu.enabled, actions.GenericMenu.Enable, actions.GenericMenu.Disable);
                Apply(MainMenu, actions.MainMenu.enabled, actions.MainMenu.Enable, actions.MainMenu.Disable);
                Apply(InGameMenu, actions.InGameMenu.enabled, actions.InGameMenu.Enable, actions.InGameMenu.Disable);
                Apply(PauseMenu, actions.PauseMenu.enabled, actions.PauseMenu.Enable, actions.PauseMenu.Disable);
                Apply(DeathMenu, actions.DeathMenu.enabled, actions.DeathMenu.Enable, actions.DeathMenu.Disable);
                Apply(LoadingScreen, actions.LoadingScreen.enabled, actions.LoadingScreen.Enable, actions.LoadingScreen.Disable);
            }
        }
    }
}
