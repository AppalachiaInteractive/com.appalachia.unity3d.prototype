namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen
{
    public interface ISplashScreenManager : IAreaManager
    {
        public void NotifyTimelineCompleted(IAreaManager notifier);
    }
}
