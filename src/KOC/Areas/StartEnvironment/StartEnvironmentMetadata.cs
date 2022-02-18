namespace Appalachia.Prototype.KOC.Areas.StartEnvironment
{
    public abstract class StartEnvironmentMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>,
        IStartEnvironmentMetadata
        where TManager : StartEnvironmentManager<TManager, TMetadata>
        where TMetadata : StartEnvironmentMetadata<TManager, TMetadata>
    {
        #region IStartEnvironmentMetadata Members

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.StartEnvironment;

        #endregion
    }
}
