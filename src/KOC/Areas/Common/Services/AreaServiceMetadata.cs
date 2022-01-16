namespace Appalachia.Prototype.KOC.Areas.Common.Services
{
    public abstract class
        AreaServiceMetadata<TService, TServiceMetadata, TAreaManager, TAreaMetadata> :
            AreaFunctionalityMetadata<TService, TServiceMetadata, TAreaManager, TAreaMetadata>
        where TService : AreaService<TService, TServiceMetadata, TAreaManager, TAreaMetadata>
        where TServiceMetadata : AreaServiceMetadata<TService, TServiceMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        public override void Apply(TService functionality)
        {
            using (_PRF_Apply.Auto())
            {
                base.Apply(functionality);
            }
        }
    }
}
