namespace Appalachia.Prototype.KOC.Application.Areas.StartEnvironment
{
    public abstract class StartEnvironmentMetadata<T, TM> : AreaMetadata<T, TM>, IStartEnvironmentMetadata
        where T : StartEnvironmentManager<T, TM>
        where TM : StartEnvironmentMetadata<T, TM>
    {
        #region IHUDMetadata Members

        public override ApplicationArea Area => ApplicationArea.StartEnvironment;

        #endregion
    }
}
