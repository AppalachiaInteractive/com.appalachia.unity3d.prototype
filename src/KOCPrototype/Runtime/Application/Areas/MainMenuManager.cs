using Appalachia.Utility.Logging;

namespace Appalachia.Prototype.KOCPrototype.Application.Areas
{
    public class MainMenuManager : AreaManager<MainMenuManager>
    {
        public void NewGame()
        {
            AppaLog.Context.MenuEvent.Info(nameof(NewGame));
        }

        public void LoadGame()
        {
            AppaLog.Context.MenuEvent.Info(nameof(LoadGame));
        }

        public void Settings()
        {
            AppaLog.Context.MenuEvent.Info(nameof(Settings));
        }

        public void Quit()
        {
            AppaLog.Context.MenuEvent.Info(nameof(Quit));
        }
    }
}
