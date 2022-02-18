namespace Appalachia.Prototype.KOC.Areas.HUD
{
    public abstract class HUDMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>, IHUDMetadata
        where TManager : HUDManager<TManager, TMetadata>
        where TMetadata : HUDMetadata<TManager, TMetadata>
    {
        #region IHUDMetadata Members

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.HUD;

        #endregion
    }
}
