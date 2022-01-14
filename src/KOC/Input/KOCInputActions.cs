using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Overrides.Implementations;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable SuspiciousTypeConversion.Global

namespace Appalachia.Prototype.KOC.Input
{
    public partial class KOCInputActions
    {
        #region Nested type: MapEnableState

        [Serializable, HideLabel]
        public struct MapEnableState
        {
            #region Fields and Autoproperties

            [SerializeField] public OverridableBool DeathMenu;

            [FormerlySerializedAs("Debug")]
            [SerializeField]
            public OverridableBool DeveloperInterface;

            [SerializeField] public OverridableBool GenericMenu;
            [SerializeField] public OverridableBool InGameMenu;
            [SerializeField] public OverridableBool LoadingScreen;
            [SerializeField] public OverridableBool MainMenu;
            [SerializeField] public OverridableBool PauseMenu;
            [SerializeField] public OverridableBool Player;
            [SerializeField] public OverridableBool SplashScreen;
            [SerializeField] public OverridableBool StartScreen;

            #endregion

            public void Apply<T>(KOCInputActions actions, T owner)
                where T : AppalachiaBehaviour<T>
            {
                Apply(
                    Player,
                    actions.Player.enabled,
                    () =>
                    {
                        actions.Player.Enable();
                        if (owner is IPlayerActions o)
                        {
                            actions.Player.SetCallbacks(o);
                        }
                    },
                    () =>
                    {
                        actions.Player.Disable();
                        actions.Player.SetCallbacks(null);
                    },
                    nameof(Player)
                );
                Apply(
                    DeveloperInterface,
                    actions.DeveloperInterface.enabled,
                    () =>
                    {
                        actions.DeveloperInterface.Enable();
                        if (owner is IDeveloperInterfaceActions o)
                        {
                            actions.DeveloperInterface.SetCallbacks(o);
                        }
                    },
                    () =>
                    {
                        actions.DeveloperInterface.Disable();
                        actions.DeveloperInterface.SetCallbacks(null);
                    },
                    nameof(DeveloperInterface)
                );
                Apply(
                    GenericMenu,
                    actions.GenericMenu.enabled,
                    () =>
                    {
                        actions.GenericMenu.Enable();
                        if (owner is IGenericMenuActions o)
                        {
                            actions.GenericMenu.SetCallbacks(o);
                        }
                    },
                    () =>
                    {
                        actions.GenericMenu.Disable();
                        actions.GenericMenu.SetCallbacks(null);
                    },
                    nameof(GenericMenu)
                );
                Apply(
                    MainMenu,
                    actions.MainMenu.enabled,
                    () =>
                    {
                        actions.MainMenu.Enable();
                        if (owner is IMainMenuActions o)
                        {
                            actions.MainMenu.SetCallbacks(o);
                        }
                    },
                    () =>
                    {
                        actions.MainMenu.Disable();
                        actions.MainMenu.SetCallbacks(null);
                    },
                    nameof(MainMenu)
                );
                Apply(
                    InGameMenu,
                    actions.InGameMenu.enabled,
                    () =>
                    {
                        actions.InGameMenu.Enable();
                        if (owner is IInGameMenuActions o)
                        {
                            actions.InGameMenu.SetCallbacks(o);
                        }
                    },
                    () =>
                    {
                        actions.InGameMenu.Disable();
                        actions.InGameMenu.SetCallbacks(null);
                    },
                    nameof(InGameMenu)
                );
                Apply(
                    PauseMenu,
                    actions.PauseMenu.enabled,
                    () =>
                    {
                        actions.PauseMenu.Enable();
                        if (owner is IPauseMenuActions o)
                        {
                            actions.PauseMenu.SetCallbacks(o);
                        }
                    },
                    () =>
                    {
                        actions.PauseMenu.Disable();
                        actions.PauseMenu.SetCallbacks(null);
                    },
                    nameof(PauseMenu)
                );
                Apply(
                    DeathMenu,
                    actions.DeathMenu.enabled,
                    () =>
                    {
                        actions.DeathMenu.Enable();
                        if (owner is IDeathMenuActions o)
                        {
                            actions.DeathMenu.SetCallbacks(o);
                        }
                    },
                    () =>
                    {
                        actions.DeathMenu.Disable();
                        actions.DeathMenu.SetCallbacks(null);
                    },
                    nameof(DeathMenu)
                );
                Apply(
                    LoadingScreen,
                    actions.LoadingScreen.enabled,
                    () =>
                    {
                        actions.LoadingScreen.Enable();
                        if (owner is ILoadingScreenActions o)
                        {
                            actions.LoadingScreen.SetCallbacks(o);
                        }
                    },
                    () =>
                    {
                        actions.LoadingScreen.Disable();
                        actions.LoadingScreen.SetCallbacks(null);
                    },
                    nameof(LoadingScreen)
                );
                Apply(
                    SplashScreen,
                    actions.SplashScreen.enabled,
                    () =>
                    {
                        actions.SplashScreen.Enable();
                        if (owner is ISplashScreenActions o)
                        {
                            actions.SplashScreen.SetCallbacks(o);
                        }
                    },
                    () =>
                    {
                        actions.SplashScreen.Disable();
                        actions.SplashScreen.SetCallbacks(null);
                    },
                    nameof(SplashScreen)
                );
                Apply(
                    StartScreen,
                    actions.StartScreen.enabled,
                    () =>
                    {
                        actions.StartScreen.Enable();
                        if (owner is IStartScreenActions o)
                        {
                            actions.StartScreen.SetCallbacks(o);
                        }
                    },
                    () =>
                    {
                        actions.StartScreen.Disable();
                        actions.StartScreen.SetCallbacks(null);
                    },
                    nameof(StartScreen)
                );
            }

            private void Apply(
                OverridableBool mapState,
                bool isEnabled,
                Action enable,
                Action disable,
                string name)
            {
                if (mapState.overrideEnabled)
                {
                    if (mapState.value)
                    {
                        if (!isEnabled)
                        {
                            AppaLog.Context.Input.Info(ZString.Format("Enabling InputMap [{0}].", name));
                            enable();
                        }
                    }
                    else if (isEnabled)
                    {
                        AppaLog.Context.Input.Info(ZString.Format("Disabling InputMap [{0}].", name));
                        disable();
                    }
                }
            }
        }

        #endregion
    }
}
