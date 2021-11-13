namespace Appalachia.Prototype.KOC.Application
{
    public enum ApplicationArea
    {
        None = 0,
        SplashScreen = 1 << 0,
        
        StartScreen = 1 << 1,
        MainMenu = 1 << 2,
        
        PauseMenu = 1 << 5,
        LoadingScreen = 1 << 6,
        
        Game = 1 << 15,
        InGameMenu = 1 << 16,
        HUD = 1 << 17,
        DebugOverlay = 1 << 31
    }
}