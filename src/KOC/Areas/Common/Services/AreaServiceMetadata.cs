using Appalachia.Prototype.KOC.Application.Services;

namespace Appalachia.Prototype.KOC.Areas.Common.Services
{
    public abstract class
        AreaServiceMetadata<TService, TServiceMetadata, TAreaManager, TAreaMetadata> :
            ApplicationServiceMetadata<TService, TServiceMetadata>
        where TService : AreaService<TService, TServiceMetadata, TAreaManager, TAreaMetadata>
        where TServiceMetadata : AreaServiceMetadata<TService, TServiceMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
