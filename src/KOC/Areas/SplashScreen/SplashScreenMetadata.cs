using System.Collections.Generic;
using System.Linq;
using Appalachia.Core.Collections.Extensions;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Scenes;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen
{
    public abstract class SplashScreenMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>,
                                                                      ISplashScreenMetadata
        where TManager : SplashScreenManager<TManager, TMetadata>
        where TMetadata : SplashScreenMetadata<TManager, TMetadata>
    {
        static SplashScreenMetadata()
        {
            RegisterDependency<MainAreaSceneInformationCollection>(i => _mainAreaSceneInformationCollection = i);
        }

        #region Static Fields and Autoproperties

        private static MainAreaSceneInformationCollection _mainAreaSceneInformationCollection;

        #endregion

        #region Fields and Autoproperties

        public List<AreaSceneInformation> splashScreens;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(
                this,
                nameof(splashScreens),
                (splashScreens == null) || (splashScreens.Count == 0),
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        splashScreens ??= new List<AreaSceneInformation>();

                        var lookup = _mainAreaSceneInformationCollection.Lookup;

                        var splashAreas = lookup.Items.Where(a => a.Key.ToString().StartsWith("SplashScreen_"));

                        splashScreens.AddRange(splashAreas.Select(sa => sa.Value));
                    }
                }
            );
        }

        #region ISplashScreenMetadata Members

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.SplashScreen;

        #endregion
    }
}
