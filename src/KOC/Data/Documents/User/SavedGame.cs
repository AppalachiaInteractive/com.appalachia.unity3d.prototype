using Appalachia.Data.Core.Documents;
using Appalachia.Prototype.KOC.Data.Collections.User;

namespace Appalachia.Prototype.KOC.Data.Documents.User
{
    public class SavedGame : AppaDocument<SavedGame, SavedGameCollection>
    {
        public SavedGame(string screenshotPath)
        {
            ScreenshotPath = screenshotPath;
        }

        

        public string ScreenshotPath { get; set; }


        protected override void SetDefaults()
        {
        }
    }
}
