namespace Appalachia.Prototype.KOC.Areas.StartEnvironment.Versions
{
    public class StartEnvironmentMetadata_V01 : StartEnvironmentMetadata<StartEnvironmentManager_V01,
        StartEnvironmentMetadata_V01>
    {
        /// <inheritdoc />
        public override AreaVersion Version => AreaVersion.V01;
    }
}
