using Appalachia.Prototype.KOC.Areas.SplashScreen.Base;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen
{
    public abstract class SplashScreenManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>,
                                                                     ISplashScreenManager
        where TManager : SplashScreenManager<TManager, TMetadata>
        where TMetadata : SplashScreenMetadata<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [ShowInInspector] private int _currentSplashScreenIndex;

        #endregion

        #region Event Functions

        /// <inheritdoc />
        protected override void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                base.Update();
            }
        }

        #endregion

        /// <inheritdoc />
        protected override void OnActivation()
        {
            using (_PRF_Activate.Auto())
            {
                Context.Log.Info(nameof(OnActivation), this);

                areaMetadata.splashScreens.Sort(
                    (a, b) =>
                    {
                        if (a == null && b == null) return 0;
                        if (a == null) return 1;
                        if (b == null) return -1;
                        var metadataA = AreaRegistry.GetMetadata(a.Area) as ISplashScreenSubMetadata;
                        var metadataB = AreaRegistry.GetMetadata(b.Area) as ISplashScreenSubMetadata;

                        return metadataA.Order.CompareTo(metadataB.Order);
                    }
                );

                areaMetadata.MarkAsModified();

                for (var index = 0; index < areaMetadata.splashScreens.Count; index++)
                {
                    var splashScreen = areaMetadata.splashScreens[index];

                    if (splashScreen == null)
                    {
                        areaMetadata.splashScreens.RemoveAt(index);
                        index -= 1;
                        continue;
                    }

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
                ApplicationManager.instance.LoadScene(ApplicationArea.LoadingScreen);
            }
        }

        /// <inheritdoc />
        protected override void OnDeactivation()
        {
            using (_PRF_Deactivate.Auto())
            {
                Context.Log.Info(nameof(OnDeactivation), this);
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.SplashScreen;

        /// <inheritdoc />
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

        private static readonly ProfilerMarker _PRF_OnSplashScreenFinished =
            new ProfilerMarker(_PRF_PFX + nameof(OnSplashScreenFinished));

        #endregion
    }
}