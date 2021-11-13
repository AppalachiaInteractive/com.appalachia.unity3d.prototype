namespace Appalachia.Prototype.KOC.Application.Areas.MainMenu.SubAreas
{
    public enum MainMenuSubArea
    {
        None = 0,
        
        NewGame = 1 << 0,
        
        LoadGame = 1 << 10,
        
        Settings = 1 << 20,
    }
}
