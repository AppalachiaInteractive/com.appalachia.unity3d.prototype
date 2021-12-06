using System.Collections.Generic;
using Appalachia.Core.Scriptables;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen
{
    public sealed class SplashScreenMetadata : AreaMetadata<SplashScreenManager, SplashScreenMetadata>
    {
        #region Fields and Autoproperties

        public List<AppalachiaObject> splashScreens;

        #endregion

        public override ApplicationArea Area => ApplicationArea.SplashScreen;

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();
                
                initializer.Initialize(this, nameof(splashScreens),
                    () =>
                    {
                        splashScreens ??= new List<AppalachiaObject>();
                        
                        //var splashScreenManagerTypes = ReflectionExtensions.GetAllInheritors();
                    });
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(SplashScreenMetadata) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
