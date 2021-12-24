namespace Appalachia.Prototype.KOC.Application.Areas.StartScreen
{
    public abstract class StartScreenMetadata<T, TM> : AreaMetadata<T, TM>, IStartScreenMetadata
        where T : StartScreenManager<T, TM>
        where TM : StartScreenMetadata<T, TM>
    {
        #region IStartScreenMetadata Members

        public override ApplicationArea Area => ApplicationArea.StartScreen;

        #endregion
    }
}
