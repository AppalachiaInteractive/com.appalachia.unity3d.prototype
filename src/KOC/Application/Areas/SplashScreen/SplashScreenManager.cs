using Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Base;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen
{
    public abstract class SplashScreenManager<T, TM> : AreaManager<T, TM>, ISplashScreenManager
        where T : SplashScreenManager<T, TM>
        where TM : SplashScreenMetadata<T, TM>
    {
        #region Fields and Autoproperties

        [ShowInInspector] private int _currentSplashScreenIndex;

        #endregion

        

        #region Event Functions

        protected override void Update()
        {
            using (_PRF_Update.Auto())
            {
                base.Update();
            }
        }

        #endregion

        protected override void OnActivation()
        {
            using (_PRF_Activate.Auto())
            {
                Context.Log.Info(nameof(OnActivation), this);

                areaMetadata.splashScreens.Sort(
                    (a, b) =>
                    {
                        var metadataA = AreaRegistry.GetMetadata(a.Area) as ISplashScreenSubMetadata;
                        var metadataB = AreaRegistry.GetMetadata(b.Area) as ISplashScreenSubMetadata;

                        return metadataA.Order.CompareTo(metadataB.Order);
                    }
                );

                areaMetadata.MarkAsModified();

                for (var index = 0; index < areaMetadata.splashScreens.Count; index++)
                {
                    var splashScreen = areaMetadata.splashScreens[index];

                    if (index == 0)
                    {
                        ApplicationManager.instance.LoadScene(splashScreen.Area, true);
                    }
                    else
                    {
                        ApplicationManager.instance.LoadScene(splashScreen.Area);
                    }
                }

                ApplicationManager.instance.LoadScene(ApplicationArea.StartScreen);
                ApplicationManager.instance.LoadScene(ApplicationArea.MainMenu);
                ApplicationManager.instance.LoadScene(ApplicationArea.MainMenu_Settings);
                ApplicationManager.instance.LoadScene(ApplicationArea.MainMenu_LoadGame);
                ApplicationManager.instance.LoadScene(ApplicationArea.MainMenu_NewGame);
                ApplicationManager.instance.LoadScene(ApplicationArea.LoadingScreen);
            }
        }

        protected override void OnDeactivation()
        {
            using (_PRF_Deactivate.Auto())
            {
                Context.Log.Info(nameof(OnDeactivation), this);
            }
        }

        protected override void ResetArea()
        {
            using (_PRF_ResetArea.Auto())
            {
                Context.Log.Info(nameof(ResetArea), this);
            }
        }

        protected void OnSplashScreenFinished()
        {
            using (_PRF_OnSplashScreenFinished.Auto())
            {
                Context.Log.Info(nameof(OnSplashScreenFinished), this);
            }
        }

        #region ISplashScreenManager Members

        public override ApplicationArea Area => ApplicationArea.SplashScreen;
        public override ApplicationArea ParentArea => ApplicationArea.None;

        public void NotifyTimelineCompleted(IAreaManager notifier)
        {
            notifier.Deactivate();

            ApplicationManager.instance.DestroyScene(notifier.Area);

            _currentSplashScreenIndex += 1;

            if (_currentSplashScreenIndex >= areaMetadata.splashScreens.Count)
            {
                Deactivate();
                ApplicationManager.instance.DestroyScene(ApplicationArea.SplashScreen);
            }
            else
            {
                var nextSplashScreen = areaMetadata.splashScreens[_currentSplashScreenIndex];
                ApplicationManager.instance.ActivateScene(nextSplashScreen.Area);
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(SplashScreenManager<T, TM>) + ".";

        private static readonly ProfilerMarker _PRF_OnSplashScreenFinished =
            new ProfilerMarker(_PRF_PFX + nameof(OnSplashScreenFinished));

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        private static readonly ProfilerMarker _PRF_Activate =
            new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        #endregion
    }
}
