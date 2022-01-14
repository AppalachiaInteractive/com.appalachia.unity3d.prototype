namespace Appalachia.Prototype.KOC.Areas.SplashScreen
{
    public interface ISplashScreenManager : IAreaManager
    {
        public void NotifyTimelineCompleted(IAreaManager notifier);
    }
}
