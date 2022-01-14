namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public abstract class DeveloperInterfaceMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>,
        IDeveloperInterfaceMetadata
        where TManager : DeveloperInterfaceManager<TManager, TMetadata>
        where TMetadata : DeveloperInterfaceMetadata<TManager, TMetadata>
    {
        #region IDeveloperInterfaceMetadata Members

        public override ApplicationArea Area => ApplicationArea.DeveloperInterface;

        #endregion
    }
}
