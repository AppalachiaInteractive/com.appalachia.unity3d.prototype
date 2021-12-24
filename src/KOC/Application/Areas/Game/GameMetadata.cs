namespace Appalachia.Prototype.KOC.Application.Areas.Game
{
    public abstract class GameMetadata<T, TM> : AreaMetadata<T, TM>, IGameMetadata
        where T : GameManager<T, TM>
        where TM : GameMetadata<T, TM>
    {
        #region IGameMetadata Members

        public override ApplicationArea Area => ApplicationArea.Game;

        #endregion
    }
}
