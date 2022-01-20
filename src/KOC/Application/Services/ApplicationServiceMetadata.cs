namespace Appalachia.Prototype.KOC.Application.Services
{
    public abstract class
        ApplicationServiceMetadata<TService, TServiceMetadata> : ApplicationFunctionalityMetadata<TService,
            TServiceMetadata>
        where TService : ApplicationService<TService, TServiceMetadata>
        where TServiceMetadata : ApplicationServiceMetadata<TService, TServiceMetadata>

    {
    }
}
