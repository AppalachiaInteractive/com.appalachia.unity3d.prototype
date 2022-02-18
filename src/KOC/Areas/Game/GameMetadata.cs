namespace Appalachia.Prototype.KOC.Areas.Game
{
    public abstract class GameMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>, IGameMetadata
        where TManager : GameManager<TManager, TMetadata>
        where TMetadata : GameMetadata<TManager, TMetadata>
    {
        #region IGameMetadata Members

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.Game;

        #endregion
    }
}
