namespace Appalachia.Prototype.KOC.Areas.LoadingScreen
{
    public abstract class LoadingScreenMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>,
                                                                       ILoadingScreenMetadata
        where TManager : LoadingScreenManager<TManager, TMetadata>
        where TMetadata : LoadingScreenMetadata<TManager, TMetadata>
    {
        #region ILoadingScreenMetadata Members

        public override ApplicationArea Area => ApplicationArea.LoadingScreen;

        #endregion
    }
}
