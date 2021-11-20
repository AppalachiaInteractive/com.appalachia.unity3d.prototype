namespace Appalachia.Prototype.KOC.Application.Areas
{
    public enum ApplicationArea
    {
        None = 0,

        SplashScreen = 1000,
        SplashScreen_Appalachia = 1100,
        SplashScreen_Partners = 1200,
        SplashScreen_Disclaimer = 1300,

        StartScreen = 2000,

        MainMenu = 3000,
        MainMenu_NewGame = 3100,
        MainMenu_LoadGame = 3200,
        MainMenu_Settings = 3300,

        LoadingScreen = 4000,

        PauseMenu = 5000,

        Game = 7000,

        InGameMenu = 8000,

        HUD = 9000,

        DebugOverlay = 90000
    }
}
