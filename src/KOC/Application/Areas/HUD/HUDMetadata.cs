namespace Appalachia.Prototype.KOC.Application.Areas.HUD
{
    public abstract class HUDMetadata<T, TM> : AreaMetadata<T, TM>, IHUDMetadata
        where T : HUDManager<T, TM>
        where TM : HUDMetadata<T, TM>
    {
        #region IHUDMetadata Members

        public override ApplicationArea Area => ApplicationArea.HUD;

        #endregion
    }
}
