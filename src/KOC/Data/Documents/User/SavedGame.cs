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

        #region Fields and Autoproperties

        public string ScreenshotPath { get; set; }

        #endregion

        protected override void SetDefaults()
        {
        }
    }
}
