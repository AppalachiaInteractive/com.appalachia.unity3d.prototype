using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.MaInMenu_LoadGame.Versions
{
    public class MainMenu_LoadGameManager_V01 : MainMenu_LoadGameManager<MainMenu_LoadGameManager_V01,
        MainMenu_LoadGameMetadata_V01>
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
