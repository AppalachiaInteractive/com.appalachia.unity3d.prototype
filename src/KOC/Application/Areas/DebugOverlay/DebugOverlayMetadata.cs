namespace Appalachia.Prototype.KOC.Application.Areas.DebugOverlay
{
    public abstract class DebugOverlayMetadata<T, TM> : AreaMetadata<T, TM>, IDebugOverlayMetadata
        where T : DebugOverlayManager<T, TM>
        where TM : DebugOverlayMetadata<T, TM>
    {
        #region IDebugOverlayMetadata Members

        public override ApplicationArea Area => ApplicationArea.DebugOverlay;

        #endregion
    }
}
