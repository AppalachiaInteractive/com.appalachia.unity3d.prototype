namespace Appalachia.Prototype.KOC.Application.Areas.LoadingScreen
{
    public abstract class LoadingScreenMetadata<T, TM> : AreaMetadata<T, TM>, ILoadingScreenMetadata
        where T : LoadingScreenManager<T, TM>
        where TM : LoadingScreenMetadata<T, TM>
    {
        #region ILoadingScreenMetadata Members

        public override ApplicationArea Area => ApplicationArea.LoadingScreen;

        #endregion
    }
}
