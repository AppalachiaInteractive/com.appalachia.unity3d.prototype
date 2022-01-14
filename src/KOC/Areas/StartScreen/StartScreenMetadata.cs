namespace Appalachia.Prototype.KOC.Areas.StartScreen
{
    public abstract class StartScreenMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>,
                                                                     IStartScreenMetadata
        where TManager : StartScreenManager<TManager, TMetadata>
        where TMetadata : StartScreenMetadata<TManager, TMetadata>
    {
        #region IStartScreenMetadata Members

        public override ApplicationArea Area => ApplicationArea.StartScreen;

        #endregion
    }
}
