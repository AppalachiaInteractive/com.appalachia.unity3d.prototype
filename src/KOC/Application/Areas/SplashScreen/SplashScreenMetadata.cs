using System.Collections.Generic;
using System.Linq;
using Appalachia.Core.Collections.Extensions;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Scenes;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen
{
    public abstract class SplashScreenMetadata<T, TM> : AreaMetadata<T, TM>, ISplashScreenMetadata
        where T : SplashScreenManager<T, TM>
        where TM : SplashScreenMetadata<T, TM>
    {
        
        #region Fields and Autoproperties

        public List<AreaSceneInformation> splashScreens;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                await initializer.Do(
                    this,
                    nameof(splashScreens),
                    (splashScreens == null) || (splashScreens.Count == 0),
                    () =>
                    {
                        splashScreens ??= new List<AreaSceneInformation>();

                        var lookup = MainAreaSceneInformationCollection.instance.Lookup;

                        var splashAreas =
                            lookup.Items.Where(a => a.Key.ToString().StartsWith("SplashScreen_"));

                        splashScreens.AddRange(splashAreas.Select(sa => sa.Value));
                    }
                );
            }
        }

        #region ISplashScreenMetadata Members

        public override ApplicationArea Area => ApplicationArea.SplashScreen;

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(SplashScreenMetadata<T, TM>) + ".";

        

        #endregion
    }
}
