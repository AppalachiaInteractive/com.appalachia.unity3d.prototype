using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.MainMenu_NewGame.Versions
{
    public class MainMenu_NewGameManager_V01 : MainMenu_NewGameManager<MainMenu_NewGameManager_V01,
        MainMenu_NewGameMetadata_V01>
    {
        public override AreaVersion Version => AreaVersion.V01;

        protected override async AppaTask SetFeaturesToInitialState()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask SetServicesToInitialState()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask SetWidgetsToInitialState()
        {
            await AppaTask.CompletedTask;
        }
    }
}
